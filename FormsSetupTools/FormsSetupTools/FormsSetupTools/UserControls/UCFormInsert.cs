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
    public partial class UCFormInsert : UserControl
    {
        FormInsertModel objBO;
        FormInsert objBLL;

        public UCFormInsert()
        {
            InitializeComponent();
        }

        private void UCFormsInsert_Load(object sender, EventArgs e)
        {
            PopulateDropdown();
            ResetForm();
        }

        private void txtMdlAftrFormVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar)))
                e.Handled = true;
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

        private void cmbMdlAftrUserline_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbMdlAftrState_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbMdlAftrCompany_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            objBLL = new FormInsert();

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
                    MessageBox.Show(Global.MsgFormInsertModifyScript, Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                cmbMdlAftrUserline.DataSource = userline.ToList();
                cmbMdlAftrUserline.DisplayMember = "key";
                cmbMdlAftrUserline.ValueMember = "value";

                Dictionary<string, string> stateList = Global.GetStateList();
                cmbState.DataSource = stateList.ToList();
                cmbState.DisplayMember = "key";
                cmbState.ValueMember = "value";

                cmbMdlAftrState.DataSource = stateList.ToList();
                cmbMdlAftrState.DisplayMember = "key";
                cmbMdlAftrState.ValueMember = "value";

                Dictionary<string, string> company = new BO.DataList().GetCompanyList().Where(i => i.Value != "-1").ToDictionary(x => x.Key, x => x.Value);
                cmbCompany.DataSource = company.ToList();
                cmbCompany.DisplayMember = "key";
                cmbCompany.ValueMember = "value";

                cmbMdlAftrCompany.DataSource = company.ToList();
                cmbMdlAftrCompany.DisplayMember = "key";
                cmbMdlAftrCompany.ValueMember = "value";
            }
            catch (Exception ex) { }
        }

        private void ResetForm()
        {
            txtFormNo.Focus();

            txtFormNo.Text = string.Empty;
            cmbUserline.SelectedIndex = 0;
            cmbState.SelectedIndex = 0;
            txtFormVersion.Text = string.Empty;
            txtFormName.Text = string.Empty;
            txtFormNameAbbr.Text = string.Empty;
            cmbCompany.SelectedIndex = 0;
            dtpEntryDate.Value = DateTime.Today;
            dtpNBEntryDate.Value = DateTime.Today;
            dtpRenewalEntryDate.Value = DateTime.Today;

            txtMdlAftrFormNo.Text = string.Empty;
            cmbMdlAftrUserline.SelectedIndex = 0;
            cmbMdlAftrState.SelectedIndex = 0;
            txtMdlAftrFormVersion.Text = string.Empty;
            cmbMdlAftrCompany.SelectedIndex = 0;
        }

        private void BindFormToModel()
        {
            objBO = new FormInsertModel();

            try
            {
                objBO.FormNo = txtFormNo.Text;
                objBO.Userline = cmbUserline.SelectedValue.ToString();
                objBO.State = cmbState.SelectedValue.ToString();
                objBO.FormVersion = txtFormVersion.Text;
                objBO.FormName = txtFormName.Text;
                objBO.FormNameAbbr = txtFormNameAbbr.Text;
                objBO.Company = cmbCompany.SelectedValue.ToString();
                objBO.EntryDate = dtpEntryDate.Value;
                objBO.NewBusinessEntryDate = dtpNBEntryDate.Value;
                objBO.RenewalEntryDate = dtpRenewalEntryDate.Value;
                objBO.MdlAftrFormNo = txtMdlAftrFormNo.Text;
                objBO.MdlAftrUserline = cmbMdlAftrUserline.SelectedValue.ToString();
                objBO.MdlAftrState = cmbMdlAftrState.SelectedValue.ToString();
                objBO.MdlAftrFormVersion = txtMdlAftrFormVersion.Text;
                objBO.MdlAftrCompany = cmbMdlAftrCompany.SelectedValue.ToString();
            }
            catch (Exception ex) { }
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
            // Validate Model After Form Name
            if (txtMdlAftrFormNo.Text.Length == 0)
            {
                MessageBox.Show("Invalid Model After Form Name.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMdlAftrFormNo.Focus();
                return false;
            }
            else if (Global.IsExistInString(txtMdlAftrFormNo.Text, Global.EscapeSpecialChar))
            {
                MessageBox.Show("Invalid Model After Form Name.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMdlAftrFormNo.Focus();
                return false;
            }
            // Validate Model After Userline
            else if (cmbMdlAftrUserline.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Invalid Model After Userline.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbMdlAftrUserline.Focus();
                return false;
            }
            // Validate Model After State
            else if (cmbMdlAftrState.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Invalid Model After State.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbMdlAftrState.Focus();
                return false;
            }
            // Validate Model After Form Version
            else if (txtMdlAftrFormVersion.Text.Length == 0)
            {
                MessageBox.Show("Invalid Model After Form Version.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMdlAftrFormVersion.Focus();
                return false;
            }
            else if (Global.IsExistInString(txtMdlAftrFormVersion.Text, Global.EscapeSpecialChar))
            {
                MessageBox.Show("Invalid Model After Form Version.", Global.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMdlAftrFormVersion.Focus();
                return false;
            }
            return retVal;
        }

        #endregion
    }
}
