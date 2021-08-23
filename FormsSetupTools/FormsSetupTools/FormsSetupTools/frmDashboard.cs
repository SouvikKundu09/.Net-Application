using FormSetupTools.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormSetupTools
{
    public partial class frmDashboard : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            SetToDefault();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            var body = new UCDashboard();
            body.ConfigUpdated += new EventHandler(ConfigUpdated_Completed);

            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(body);
            pnlActiveMenuHighlighter.Top = btnDashboard.Top;
        }

        private void btnFormsExpiry_Click(object sender, EventArgs e)
        {
            var body = new UCFormExpiry();
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(body);
            pnlActiveMenuHighlighter.Top = btnFormsExpiry.Top;
        }

        private void btnFormsUpdate_Click(object sender, EventArgs e)
        {
            var body = new UCFormUpdate();
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(body);
            pnlActiveMenuHighlighter.Top = btnFormsUpdate.Top;
        }

        private void btnFormsInsert_Click(object sender, EventArgs e)
        {
            var body = new UCFormInsert();
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(body);
            pnlActiveMenuHighlighter.Top = btnFormsInsert.Top;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void ConfigUpdated_Completed(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        #region Custom Methods
        /// <summary>
        /// Set form to default state
        /// </summary>
        private void SetToDefault()
        {
            btnDashboard_Click(new object(), new EventArgs());
            UpdateStatusBar();

            this.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            pnlHeaderBar.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            pnlLogoContainer.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            pnllogoImgContainer.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            pbLogo1.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            pbLogo2.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            pbLogo3.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
            lblAppHeading.MouseDown += (sender, e) => { if (e.Button == MouseButtons.Left)MoveForm(); };
        }

        private void MoveForm()
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void UpdateStatusBar()
        {
            statuslblServerName.Text = BLL.Shared.Global.DBConfig[0];
            statusLblDB.Text = BLL.Shared.Global.DBConfig[1];
        }

        #endregion
    }
}
