using System;
using System.Collections;
using System.Windows.Forms;

namespace NPS
{
    internal class ListViewItemComparer : IComparer
    {
        private readonly int _column;
        private readonly bool _invertOrder = false;
        public ListViewItemComparer()
        {
            _column = 0;
        }
        public ListViewItemComparer(int column, bool invertedOrder)
        {
            _column = column;
            _invertOrder = invertedOrder;
        }
        public int Compare(object x, object y)
        {
            int returnVal = -1;

            string sx = ((ListViewItem)x).SubItems[_column].Text;
            string sy = ((ListViewItem)y).SubItems[_column].Text;

            if (_column == 4)
            {
                float.TryParse(sx, out float fx);
                float.TryParse(sy, out float fy);

                returnVal = _invertOrder ? fy.CompareTo(fx) : fx.CompareTo(fy);
            }
            else if (_column == 5)
            {
                DateTime dtx = DateTime.MinValue;
                DateTime.TryParse(sx, out dtx);
                DateTime dty = DateTime.MinValue;
                DateTime.TryParse(sy, out dty);

                returnVal = _invertOrder ? dty.CompareTo(dtx) : dtx.CompareTo(dty);
            }
            else
            {
                returnVal = _invertOrder ? string.Compare(sy, sx) : string.Compare(sx, sy);
            }
            return returnVal;
        }
    }

}
