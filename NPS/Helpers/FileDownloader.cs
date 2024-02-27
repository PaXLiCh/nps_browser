using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace NPS.Helpers
{
    public class FileDownloader
    {
        private static readonly Lazy<FileDownloader> _instance = new Lazy<FileDownloader>();
        public static FileDownloader Instance => _instance.Value;


        private static SemaphoreSlim _semaphore = new SemaphoreSlim(Settings.Instance.simultaneousDl, Settings.Instance.simultaneousDl);

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public delegate void DownloadHandler(FileDownloader sender, bool isOk, string uri, string localPath);

        public event DownloadHandler FileDownloaded;

        /// <summary>
        /// Download file to specified directory name from specified <see cref="Uri"/>.
        /// Skip downloading if local file exist.
        /// </summary>
        /// <param name="uri"> <see cref="Uri"/> of remote file. </param>
        /// <param name="localDir"> Directory path to downloaded file. </param>
        /// <param name="onFileDownloaded"> Download handler. </param>
        public void DownloadFile([NotNull] Uri uri, [NotNull] string localDir, DownloadHandler onFileDownloaded)
        {
            var fileName = Path.GetFileName(uri.LocalPath);
            DownloadFile(uri, localDir, fileName, onFileDownloaded, _cancellationTokenSource.Token);
        }

        public void DownloadFile([NotNull] Uri uri, [NotNull] string localDir, [NotNull] string localName, DownloadHandler onFileDownloaded)
        {
            DownloadFile(uri, localDir, localName, onFileDownloaded, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// Download file with specified name from specified <see cref="Uri"/>.
        /// Skip downloading if local file exist.
        /// </summary>
        /// <param name="uri"> <see cref="Uri"/> of remote file. </param>
        /// <param name="localDir"> Directory path to downloaded file. </param>
        /// <param name="localName"> Name of downloaded file. </param>
        /// <param name="onFileDownloaded"> Download handler. </param>
        public Task DownloadFile([NotNull] Uri uri, [NotNull] string localDir, [NotNull] string localName, DownloadHandler onFileDownloaded, CancellationToken cancellationToken)
        {
            var task = Task.Run(() =>
            {
                var filePath = Path.Combine(localDir, localName);
                var isOk = false;

                if (File.Exists(filePath))
                {
                    // Skip existing file
                    return;
                }

                _semaphore.Wait(cancellationToken);

                try
                {
                    WebClient wc = new WebClient
                    {
                        Encoding = Encoding.UTF8,
                        Proxy = Settings.Instance.proxy
                    };
                    var bytes = wc.DownloadData(uri);
                    using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        file.Write(bytes, 0, bytes.Length);
                    }
                    isOk = true;
                }
                catch (OperationCanceledException)
                {
                    // do nothing
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
                finally
                {
                    _semaphore.Release();

                    onFileDownloaded?.Invoke(this, isOk, uri.AbsolutePath, filePath);
                    FileDownloaded?.Invoke(this, isOk, uri.AbsolutePath, filePath);
                }
            }, cancellationToken);

            return task;
        }
    }
}
