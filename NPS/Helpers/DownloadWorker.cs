using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using NPS.Helpers;

namespace NPS
{
    [Serializable]
    public class DownloadWorker
    {
        //private WebClient webClient;
        private DateTime lastUpdate;
        private long lastBytes;

        private string pkgOutputPath;

        private long totalSize = 0;
        private long completedSize = 0;
        private long _downloadSpeed = 0;

        [NonSerialized]
        public ProgressBar progress = new ProgressBar();

        [NonSerialized]
        private Process _unpackProcess = null;

        [NonSerialized]
        private Timer timer = new Timer();

        [NonSerialized]
        private Form formCaller;

        [NonSerialized]
        private Stream smRespStream;

        [NonSerialized]
        private FileStream saveFileStream;

        [NonSerialized]
        private readonly List<string> _errors = new List<string>();

        public Item currentDownload;
        public ListViewItem lvi;
        public int progressValue = 0;
        public WorkerStatus Status { get; private set; }
        public string Pkg => pkgOutputPath;


        public DownloadWorker(Item itm, Form f)
        {
            currentDownload = itm;
            lvi = new ListViewItem(itm.TitleName);
            lvi.SubItems.Add("Waiting");
            lvi.SubItems.Add("");
            lvi.SubItems.Add("");
            lvi.Tag = this;

            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            formCaller = f;
            Status = WorkerStatus.Queued;
        }

        public void Recreate(Form formCaller)
        {
            this.formCaller = formCaller;
            progress = new ProgressBar();
            if (progressValue > 100) progressValue = 100;
            progress.Value = progressValue;
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Tick += Timer_Tick;
            lvi.Tag = this;

            if (Status == WorkerStatus.Running)
            {
                Start();
            }
            else if (Status == WorkerStatus.Downloaded)
            {
                Unpack();
            }
            else if (Status == WorkerStatus.Completed)
            {
                lvi.SubItems[1].Text = "";
                lvi.SubItems[2].Text = "Completed";
            }
        }

        public void Start()
        {
            Console.WriteLine("Start downloading {0}", currentDownload.TitleName);
            timer.Start();

            Status = WorkerStatus.Running;

            string pkgOutputDirectory = Path.Combine(Settings.Instance.downloadDir, currentDownload.PkgsDir);
            pkgOutputPath = Path.Combine(pkgOutputDirectory, currentDownload.PackageFileName + currentDownload.PackageFileExtension);
            if (!Directory.Exists(pkgOutputDirectory))
            {
                Directory.CreateDirectory(pkgOutputDirectory);
            }

            Task.Run(() =>
            {
                DownloadFile(currentDownload.pkg, pkgOutputPath);
            });
        }

        public void Cancel()
        {
            timer.Stop();
            if (Status == WorkerStatus.Completed) return;

            Status = WorkerStatus.Canceled;

            smRespStream?.Close();
            saveFileStream?.Close();
            if (_unpackProcess != null && !_unpackProcess.HasExited)
            {
                _unpackProcess.Kill();
            }

            lvi.SubItems[1].Text = "";
            lvi.SubItems[2].Text = "Canceled";
            progressValue = 0;
            progress.Value = progressValue;
            DeletePkg();
        }

        public void Pause()
        {
            if (Status != WorkerStatus.Running && Status != WorkerStatus.Queued)
            {
                return;
            }
            timer.Stop();

            Status = WorkerStatus.Paused;

            smRespStream?.Close();
            saveFileStream?.Close();
            if (_unpackProcess != null && !_unpackProcess.HasExited)
            {
                _unpackProcess.Kill();
            }

            lvi.SubItems[1].Text = "Paused";
            //progress.Value = 0;
        }

        public void Resume()
        {
            if (Status != WorkerStatus.Paused && Status != WorkerStatus.DownloadError)
            {
                return;
            }
            lvi.SubItems[1].Text = "Queued";
            Status = WorkerStatus.Queued;
        }

        public void DeletePkg()
        {
            if (currentDownload == null)
            {
                return;
            }
            for (int i = 0; i < 1; ++i)
            {
                try
                {
                    if (File.Exists(pkgOutputPath))
                    {
                        System.Threading.Thread.Sleep(400);
                        File.Delete(pkgOutputPath);
                    }
                }
                catch { i = 5; }
            }
        }

        public void Unpack()
        {
            if (currentDownload.Platform == Platform.PS3)
            {
                UnpackPS3();
                return;
            }

            if (currentDownload.ItsCompPack)
            {
                UnpackCompPack();
                return;
            }

            if (Status != WorkerStatus.Downloaded && Status != WorkerStatus.Completed)
            {
                return;
            }

            lvi.SubItems[2].Text = "Unpacking";

            string tempName = "";
            string dlc = "";
            if (currentDownload.ContentType == ContentType.DLC)
            {
                //dlc = "[DLC]";
                tempName = "[DLC] " + currentDownload.ParentGameTitle;
            }
            else
            {
                tempName = currentDownload.TitleName;
            }

            string fwVersion = "3.60";
            if (tempName.Contains("3.61") /*currentDownload.TitleName.Contains("3.61")*/)
            {
                fwVersion = "3.61";
            }
            string[] tempStr = tempName.Split();
            tempName = "";

            foreach (var i in tempStr)
            {
                if (i.Contains("3.6") && (!i.Contains("3.61+")))
                {
                    fwVersion = i;
                }
                if (!i.Contains("3.6"))
                {
                    tempName += i + " ";
                }
            }

            tempName = Regex.Replace(tempName, "[/:\"*?<>|]+", " ");
            tempName = Regex.Replace(tempName, "\\r\\n", string.Empty);
            tempName = tempName.Trim();

            var replacements = new Dictionary<string, string>
            {
                ["{pkgfile}"] = $"\"{pkgOutputPath}\"",
                ["{titleid}"] = currentDownload.TitleId.Substring(0, 9),
                ["{contentid}"] = currentDownload.ContentId,
                ["{gametitle}"] = tempName,
                ["{region}"] = currentDownload.Region,
                ["{zrifkey}"] = currentDownload.zRif,
                ["{fwversion}"] = fwVersion,
                ["{dlc}"] = dlc,
                ["  "] = " "
            };

            ProcessStartInfo a = new ProcessStartInfo
            {
                WorkingDirectory = Settings.Instance.downloadDir + Path.DirectorySeparatorChar,
                FileName = string.Format("\"{0}\"", Settings.Instance.unpackerPath),
                Arguments = replacements.Aggregate(Settings.Instance.unpackerParams.ToLowerInvariant(), (str, rep) => str.Replace(rep.Key, rep.Value)),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
            };

            _unpackProcess = new Process
            {
                StartInfo = a,
                EnableRaisingEvents = true
            };

            _unpackProcess.Exited += UnpackProcessExited;
            _unpackProcess.ErrorDataReceived += new DataReceivedEventHandler(UnpackProcessErrorDataReceived);

            _errors.Clear();

            _unpackProcess.Start();
            _unpackProcess.BeginErrorReadLine();
        }

        private void UnpackCompPack()
        {
            if (Status != WorkerStatus.Downloaded && Status != WorkerStatus.Completed)
            {
                return;
            }
            Status = WorkerStatus.Completed;
            try
            {
                lvi.SubItems[2].Text = "Processing";

                //if (Directory.Exists(Path.Combine(Settings.Instance.downloadDir, "rePatch", currentDownload.TitleId)))
                //{
                //    Directory.Delete(Path.Combine(Settings.Instance.downloadDir, "rePatch", currentDownload.TitleId), true);
                //}

                using (var archive = ZipFile.OpenRead(pkgOutputPath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Length == 0) continue;

                        string file = Path.Combine(Settings.Instance.downloadDir, "rePatch", currentDownload.TitleId, entry.FullName);
                        var dir = Path.GetDirectoryName(file);

                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        entry.ExtractToFile(file, true);
                    }
                }

                //System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(Settings.Instance.downloadDir, currentDownload.DownloadFileName + currentDownload.extension), Path.Combine(Settings.Instance.downloadDir, "rePatch", currentDownload.TitleId));
                lvi.SubItems[1].Text = "";
                lvi.SubItems[2].Text = "Completed";
                if (!Settings.Instance.history.completedDownloading.Contains(currentDownload))
                {
                    Settings.Instance.history.completedDownloading.Add(currentDownload);
                }

                if (Settings.Instance.deleteAfterUnpack)
                {
                    DeletePkg();
                }
            }
            catch (Exception err)
            {
                lvi.SubItems[1].Text = "Error!";
                lvi.SubItems[2].Text = err.Message;
            }
        }

        private void UnpackPS3()
        {
            if (Status != WorkerStatus.Downloaded && Status != WorkerStatus.Completed)
            {
                return;
            }
            Status = WorkerStatus.Completed;
            try
            {
                lvi.SubItems[2].Text = "Processing";
                // jon: custom PS3 file placement
                string gamePath = Path.Combine(Settings.Instance.downloadDir, "PS3", currentDownload.TitleId);
                string path = Path.Combine(gamePath, "packages");
                // jon: end
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //string newPkgName = String.Format("{0} [{1}] [{2}]", currentDownload.TitleName, currentDownload.TitleId, currentDownload.GetRegionCode());
                string newPkgName = currentDownload.ContentId;
                if (currentDownload.ContentType == ContentType.DLC)
                    newPkgName = "[DLC] " + newPkgName;
                if (currentDownload.ContentType == ContentType.UPDATE)
                    newPkgName = "[UPDATE] " + newPkgName;

                File.Move(pkgOutputPath, Path.Combine(path, newPkgName + currentDownload.PackageFileExtension));

                // jon: changing to custom directory location
                path = Path.Combine(gamePath, "exdata");
                // jon: end

                if (!string.IsNullOrEmpty(currentDownload.ContentId)
                    && currentDownload.ContentId.ToLowerInvariant() != "missing"
                    && currentDownload.zRif.ToLowerInvariant() != "not required"
                    && currentDownload.zRif.Length % 2 == 0)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    byte[] array = new byte[currentDownload.zRif.Length / 2];
                    for (int i = 0; i < currentDownload.zRif.Length / 2; ++i)
                    {
                        array[i] = Convert.ToByte(currentDownload.zRif.Substring(i * 2, 2), 16);
                    }

                    File.WriteAllBytes(Path.Combine(path, currentDownload.ContentId + ".rap"), array);
                }

                lvi.SubItems[1].Text = "";
                lvi.SubItems[2].Text = "Completed";

                if (!Settings.Instance.history.completedDownloading.Contains(currentDownload))
                {
                    Settings.Instance.history.completedDownloading.Add(currentDownload);
                }

            }
            catch (Exception err)
            {
                lvi.SubItems[1].Text = "Error!";
                lvi.SubItems[2].Text = err.Message;
            }
        }

        private void UnpackProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _errors.Add(e.Data);
        }

        private void UnpackProcessExited(object sender, EventArgs e)
        {
            Status = WorkerStatus.Completed;

            var proc = sender as Process;
            if (proc.ExitCode == 0)
            {
                formCaller.Invoke(new Action(() =>
                {
                    lvi.SubItems[1].Text = "";
                    lvi.SubItems[2].Text = "Completed";

                    if (!Settings.Instance.history.completedDownloading.Contains(currentDownload))
                    {
                        Settings.Instance.history.completedDownloading.Add(currentDownload);
                    }

                    if (Settings.Instance.deleteAfterUnpack)
                    {
                        DeletePkg();
                    }
                }));
            }
            else
            {
                formCaller.Invoke(new Action(() =>
                {
                    lvi.SubItems[1].Text = "PKG decrypt err!";
                    lvi.SubItems[2].Text = "";

                    _errors.Remove(null);
                    if (_errors.Count > 0)
                    {
                        if (_errors[0].Contains("pkg_dec - PS Vita PKG decryptor/unpacker"))
                        {
                            _errors.Remove(_errors[0]);
                        }
                        if (_errors.Count > 0)
                        {
                            lvi.SubItems[2].Text = _errors[0];
                        }
                    }
                }
                ));
            }
        }

        private void DecryptVita()
        {
            lvi.SubItems[2].Text = "Decrypting";

            string tempName = "";
            string dlc = "";
            if (currentDownload.ContentType == ContentType.DLC)
            {
                //dlc = "[DLC]";
                tempName = "[DLC] " + currentDownload.ParentGameTitle;
            }
            else
            {
                tempName = currentDownload.TitleName;
            }

            string fwVersion = "3.60";
            if (tempName.Contains("3.61") /*currentDownload.TitleName.Contains("3.61")*/)
            {
                fwVersion = "3.61";
            }
            string[] tempStr = tempName.Split();
            tempName = "";

            foreach (var i in tempStr)
            {
                if (i.Contains("3.6") && (!i.Contains("3.61+")))
                {
                    fwVersion = i;
                }
                if (!i.Contains("3.6"))
                {
                    tempName += i + " ";
                }
            }

            tempName = Regex.Replace(tempName, "[/:\"*?<>|]+", " ");
            tempName = Regex.Replace(tempName, "\\r\\n", string.Empty);
            tempName = tempName.Trim();

            var replacements = new Dictionary<string, string>
            {
                ["{pkgfile}"] = $"\"{pkgOutputPath}\"",
                ["{pathIn}"] = {},
                ["{pathOut}"] = { },
                ["{titleid}"] = currentDownload.TitleId.Substring(0, 9),
                ["{contentid}"] = currentDownload.ContentId,
                ["{gametitle}"] = tempName,
                ["{region}"] = currentDownload.Region,
                ["{zrifkey}"] = currentDownload.zRif,
                ["{fwversion}"] = fwVersion,
                ["{dlc}"] = dlc,
                ["  "] = " "
            };

            ProcessStartInfo a = new ProcessStartInfo
            {
                WorkingDirectory = Settings.Instance.downloadDir + Path.DirectorySeparatorChar,
                FileName = string.Format("\"{0}\"", Settings.Instance.psvParserPath),
                Arguments = replacements.Aggregate(Settings.Instance.psvParserParams.ToLowerInvariant(), (str, rep) => str.Replace(rep.Key, rep.Value)),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
            };

            _unpackProcess = new Process
            {
                StartInfo = a,
                EnableRaisingEvents = true
            };

            _unpackProcess.Exited += UnpackProcessExited;
            _unpackProcess.ErrorDataReceived += new DataReceivedEventHandler(UnpackProcessErrorDataReceived);

            _errors.Clear();

            _unpackProcess.Start();
            _unpackProcess.BeginErrorReadLine();
        }

        private void DownloadFile(string sSourceURL, string sDestinationPath)
        {
            try
            {
                long iFileSize = 0;
                int iBufferSize = 1024;
                iBufferSize *= 1000;
                long iExistLen = 0;

                if (File.Exists(sDestinationPath))
                {
                    FileInfo fINfo = new FileInfo(sDestinationPath);
                    iExistLen = fINfo.Length;
                }
                ;
                if (iExistLen > 0)
                {
                    saveFileStream = new FileStream(sDestinationPath,
                      FileMode.Append, FileAccess.Write,
                      FileShare.ReadWrite);
                }
                else
                {
                    saveFileStream = new FileStream(sDestinationPath,
                      FileMode.Create, FileAccess.Write,
                      FileShare.ReadWrite);
                }

                HttpWebRequest hwRq;
                HttpWebResponse hwRes;
                var uri = new Uri(sSourceURL);
                hwRq = (HttpWebRequest)WebRequest.Create(uri);
                hwRq.Proxy = Settings.Instance.proxy;
                hwRes = (HttpWebResponse)hwRq.GetResponse();
                hwRes.Close();

                totalSize = hwRes.ContentLength;
                if (totalSize != iExistLen)
                {
                    hwRq = (HttpWebRequest)WebRequest.Create(uri);
                    hwRq.Proxy = Settings.Instance.proxy;
                    hwRq.AddRange(iExistLen);

                    hwRes = (HttpWebResponse)hwRq.GetResponse();
                    smRespStream = hwRes.GetResponseStream();

                    iFileSize = hwRes.ContentLength;

                    byte[] downBuffer = new byte[iBufferSize];
                    int iByteSize;
                    while ((iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        if (Status == WorkerStatus.Paused || Status == WorkerStatus.Canceled) return;

                        saveFileStream.Write(downBuffer, 0, iByteSize);

                        completedSize = saveFileStream.Position;

                        if (lastBytes == 0)
                        {
                            lastUpdate = DateTime.Now;
                            lastBytes = completedSize;
                        }
                        else
                        {
                            var now = DateTime.Now;
                            var timeSpan = now - lastUpdate;
                            var bytesChange = completedSize - lastBytes;
                            if (timeSpan.Seconds != 0)
                            {
                                _downloadSpeed = bytesChange / timeSpan.Seconds;
                                lastBytes = completedSize;
                                lastUpdate = now;
                            }
                        }
                    }
                    smRespStream.Close();
                }


                saveFileStream.Close();
                formCaller.Invoke(new Action(() => { DownloadCompleted(); }));
            }
            catch (Exception err)
            {
                formCaller.Invoke(new Action(() =>
                {
                    Pause();
                    MessageBox.Show($"Unable to download \"{currentDownload.TitleName}\".{Environment.NewLine}{err.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        private void DownloadCompleted()
        {
            timer.Stop();

            Status = WorkerStatus.Downloaded;

            lvi.SubItems[1].Text = "";

            Unpack();

            progressValue = 100;
            progress.Value = progressValue;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string speed = $"{Utils.GetSizeShort(_downloadSpeed)}/s";

            lvi.SubItems[1].Text = speed;

            if (totalSize > 0)
            {
                float prgs = (float)completedSize / totalSize;
                progressValue = (int)(prgs * 100.0F);
                if (progressValue > 100)
                {
                    progressValue = 100;
                }
                else if (progressValue < 0)
                {
                    progressValue = 0;
                }
            }
            else
            {
                progressValue = 0;
            }

            progress.Value = progressValue;

            lvi.SubItems[2].Text = $"{Utils.GetSizeShort(completedSize)} / {Utils.GetSizeShort(totalSize)}";
        }
    }

    public enum WorkerStatus { Queued, Running, Paused, Completed, Downloaded, Canceled, DownloadError }
}
