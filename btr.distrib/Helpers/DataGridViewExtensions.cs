using System.Collections.Generic;
using System.Windows.Forms;

namespace btr.distrib.Helpers
{
    public static class DataGridViewExtensions
    {
        public static DataGridViewColumn GetCol(this DataGridViewColumnCollection cols, string caption)
        {
            foreach (DataGridViewColumn item in cols)
                if (item.Name == caption)
                    return item;
            throw new KeyNotFoundException($"Column not found ({caption})");
        }
    }
}