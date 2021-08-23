using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BO;
using BLL;

namespace FormSetupTools.UserControls
{
    public partial class UCDashboard : UserControl
    {
        DataList objData;
        DashboardConfigModel objConfigBO;
        Dashboard objBLL;
        public event EventHandler ConfigUpdated;

        public UCDashboard()
        {
            InitializeComponent();
        }

        private void UCDashboard_Load(object sender, EventArgs e)
        {
            PopulateCombobox();
            SetToDefault();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            objBLL = new Dashboard();

            BindConfigFormToModel();
            if (objBLL.ModifyConnectionString(objConfigBO))
            {
                MessageBox.Show("Database configuration successfully updated.", BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetConfigInfo();
                if (this.ConfigUpdated != null)
                    this.ConfigUpdated(this, e);
            }
            else
            {
                MessageBox.Show("Error in saving database configuration.", BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            objData = new DataList();

            try
            {
                cmbDatabase.Items.Clear();
                cmbDatabase.Items.AddRange(BLL.Shared.Global.GetDatabaseList(BLL.Shared.Global.DBConfig[0], ReferenceEquals(cmbServer.SelectedItem, null) ? string.Empty : cmbServer.SelectedItem.ToString(), BLL.Shared.Global.DBConfig[1]));
                cmbDatabase.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTicketNo_TextChanged(object sender, EventArgs e)
        {
            if (txtTicketNo.Text.Length > 0)
            {
                lnkJIRABRD.Text = BLL.Shared.Global.JIRABrowseLink + txtTicketNo.Text;
            }
            else
            {
                lnkJIRABRD.Text = BLL.Shared.Global.JIRAHomeLink;
            }
        }

        private void lnkJIRABRD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkJIRABRD.Text);
        }

        private void lnkJIRAFormWorkbook_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkJIRAFormWorkbook.Text);
        }

        #region Custom Methods

        private void BindConfigFormToModel()
        {
            objConfigBO = new DashboardConfigModel();

            try
            {
                objConfigBO.Server = cmbServer.SelectedItem.ToString();
                objConfigBO.Database = cmbDatabase.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateCombobox()
        {
            objData = new DataList();

            try
            {
                GetConfigInfo();

                // Adding Servers
                cmbServer.Items.Clear();
                cmbServer.Items.AddRange(objData.GetServers());
                cmbServer.SelectedItem = BLL.Shared.Global.DBConfig[0];

                // Adding Databases
                cmbDatabase.Items.Clear();
                cmbDatabase.Items.AddRange(BLL.Shared.Global.GetDatabaseList(BLL.Shared.Global.DBConfig[0], ReferenceEquals(cmbServer.SelectedItem, null) ? string.Empty : cmbServer.SelectedItem.ToString(), BLL.Shared.Global.DBConfig[1]));
                cmbDatabase.SelectedItem = BLL.Shared.Global.DBConfig[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetToDefault()
        {
            lnkJIRAFormWorkbook.Text = BLL.Shared.Global.FormsWorkbookLink;
        }

        private void GetConfigInfo()
        {
            string[] config = new Dashboard().GetDBConfig();
            BLL.Shared.Global.DBConfig = config;
        }

        #endregion
    }
}
