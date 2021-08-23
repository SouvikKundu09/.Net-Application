using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLL.Shared;
using DAL;

namespace BLL
{
    public class FormUpdate
    {
        public string GenerateFormsExpiringSQL(FormUpdateModel model)
        {
            string script = string.Empty;
            try
            {
                model.ProductIndicator = Global.GetProductIndicator(model.State, model.Userline);
                model.FormType = Global.GetFormType(model.State, model.Userline, model.FormNo, model.OldFormVersion, model.ProductIndicator, model.Company);

                if (model.ProductIndicator.Equals(string.Empty))
                    return Global.MsgProdIndFetchError;
                if (model.FormType.Equals(string.Empty))
                    return Global.MsgFormNotFoundError;

                script = Global.FormUpdateSQL;
                script = script.Replace(Global.Param_form_no, model.FormNo);
                script = script.Replace(Global.Param_state, model.State);
                script = script.Replace(Global.Param_userline, model.Userline);
                script = script.Replace(Global.Param_old_version, model.OldFormVersion);
                script = script.Replace(Global.Param_new_version, model.NewFormVersion);
                script = script.Replace(Global.Param_company, model.Company == "-1" ? string.Empty : model.Company);
                script = script.Replace(Global.Param_end_fdate, model.EntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_nb_fDate, model.NewBusinessEntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_ren_fDate, model.RenewalEntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_product_ind, model.ProductIndicator);
                script = script.Replace(Global.Param_name, model.FormName);
                script = script.Replace(Global.Param_name_abbr, model.FormNameAbbr);
                if (model.FormType == "A")
                    script = script.Replace(Global.Param_AddFormUpdateTriggerScript, Global.SubScript_AddFormUpdateTriggerScript);
                else
                    script = script.Replace(Global.Param_AddFormUpdateTriggerScript, string.Empty);

                if (model.Company != "-1")
                    script = script.Replace(Global.Param_AddCompanyCondition, Global.SubScript_AddCompanyCondition);
                else
                    script = script.Replace(Global.Param_AddCompanyCondition, string.Empty);

                if (model.FormName.Trim().Length > 0)
                    script = script.Replace(Global.Param_AddCustomName, Global.Param_name_var);
                else
                    script = script.Replace(Global.Param_AddCustomName, Global.SubScript_AddCustomName);

                if (model.FormNameAbbr.Trim().Length > 0)
                    script = script.Replace(Global.Param_AddCustomNameAbbr, Global.Param_name_abbr_var);
                else
                    script = script.Replace(Global.Param_AddCustomNameAbbr, Global.SubScript_AddCustomNameAbbr);
            }
            catch (Exception ex) { }

            return script;
        }
    }
}
