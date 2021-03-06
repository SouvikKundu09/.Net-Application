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
using BLL.Shared;

namespace FormSetupTools.UserControls
{
    public partial class UCFormExpiry : UserControl
    {
        FormExpiryModel objBO;
        FormExpiry objBLL;

        public UCFormExpiry()
        {
            InitializeComponent();
        }

        private void UCFormsExpiring_Load(object sender, EventArgs e)
        {
            PopulateDropdown();
            ResetForm();
        }

        private void txtFormVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void cmbUserline_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbState_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbCompany_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            objBLL = new FormExpiry();

            BindFormToModel();
            if (ValidateForm())
            {
                rtxtScript.Text = objBLL.GenerateFormsExpiringSQL(objBO);
                if (rtxtScript.Text == Global.MsgProdIndFetchError || rtxtScript.Text == Global.MsgFormNotFoundError)
                {
                    Decorator.DecorateErrorMessage(ref rtxtScript);
                }
                else
                {
                    Decorator.DecorateSQL(ref rtxtScript);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnExportSQL_Click(object sender, EventArgs e)
        {
            if (rtxtScript.Text.Trim().Length == 0)
            {
                MessageBox.Show("There are no content to export.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Export objExport = new Export();
            string exportedFilePath = objExport.ExportToSQL(rtxtScript.Text);
            if (exportedFilePath != Global.MsgExportCanceled)
            {
                if (exportedFilePath.Trim().Length > 0)
                {
                    if (MessageBox.Show(
                            "Script exported successfully." + Environment.NewLine + "Do you want to open?",
                            Global.AppName,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(exportedFilePath);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid file name.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #region Custom Methods

        private void PopulateDropdown()
        {
            try
            {
                Dictionary<string, string> userline = new BO.DataList().GetUserlineList();
                cmbUserline.DataSource = userline.ToList();
                cmbUserline.DisplayMember = "key";
                cmbUserline.ValueMember = "value";

                Dictionary<string, string> stateList = Global.GetStateList();
                cmbState.DataSource = stateList.ToList();
                cmbState.DisplayMember = "key";
                cmbState.ValueMember = "value";

                Dictionary<string, string> company = new BO.DataList().GetCompanyList();
                cmbCompany.DataSource = company.ToList();
                cmbCompany.DisplayMember = "key";
                cmbCompany.ValueMember = "value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtFormNo.Focus();

            txtFormNo.Text = string.Empty;
            cmbUserline.SelectedIndex = 0;
            cmbState.SelectedIndex = 0;
            txtFormVersion.Text = string.Empty;
            cmbCompany.SelectedIndex = 0;
            dtpEntryDate.Value = DateTime.Today;
            dtpNBEntryDate.Value = DateTime.Today;
            dtpRenewalEntryDate.Value = DateTime.Today;
        }

        private void BindFormToModel()
        {
            objBO = new FormExpiryModel();

            try
            {
                objBO.FormNo = txtFormNo.Text;
                objBO.Userline = cmbUserline.SelectedValue.ToString();
                objBO.State = cmbState.SelectedValue.ToString();
                objBO.FormVersion = txtFormVersion.Text;
                objBO.Company = cmbCompany.SelectedValue.ToString();
                objBO.EntryDate = dtpEntryDate.Value;
                objBO.NewBusinessEntryDate = dtpNBEntryDate.Value;
                objBO.RenewalEntryDate = dtpRenewalEntryDate.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, BLL.Shared.Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            bool retVal = true;
            // Validate Form Name
            if (txtFormNo.Text.Length == 0)
            {
                MessageBox.Show("Invalid Form Number.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFormNo.Focus();
                return false;
            }
            else if (Global.IsExistInString(txtFormNo.Text, Global.EscapeSpecialChar))
            {
                MessageBox.Show("Invalid Form Number.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFormNo.Focus();
                return false;
            }
            // Validate Userline
            else if (cmbUserline.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Invalid Userline.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbUserline.Focus();
                return false;
            }
            // Validate State
            else if (cmbState.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Invalid State.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbState.Focus();
                return false;
            }
            // Validate Form Version
            else if (txtFormVersion.Text.Length == 0)
            {
                MessageBox.Show("Invalid Form Version.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFormVersion.Focus();
                return false;
            }
            else if (Global.IsExistInString(txtFormVersion.Text, Global.EscapeSpecialChar))
            {
                MessageBox.Show("Invalid Form Version.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFormVersion.Focus();
                return false;
            }
            return retVal;
        }

        #endregion
    }
}
