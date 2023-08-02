using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.Helpers
{
    public static class ControlStyle
    {
        public static void HideUpDownButtons(NumericUpDown numericUpDown)
        {
            // Get the handle of the control
            IntPtr handle = numericUpDown.Handle;

            // Modify the control's style to hide the up and down buttons
            int style = GetWindowLong(handle, GWL_STYLE);
            SetWindowLong(handle, GWL_STYLE, style & ~UDS_ARROWKEYS);
        }

        // Interop constants
        private const int GWL_STYLE = -16;
        private const int UDS_ARROWKEYS = 0x0002;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    }
}
