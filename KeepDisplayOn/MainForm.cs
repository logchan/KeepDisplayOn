using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepDisplayOn
{
    public partial class MainForm : Form
    {
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        private int _timerCnt = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void setExecutionState()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            _timerCnt = (_timerCnt + 1) % 60;
            if (_timerCnt == 0)
            {
                setExecutionState();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainNotifyIcon.Visible = false;
            Close();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
