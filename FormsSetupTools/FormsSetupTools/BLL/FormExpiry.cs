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
    public class FormExpiry
    {
        public string GenerateFormsExpiringSQL(FormExpiryModel model)
        {
            string script = string.Empty;
            try
            {
                model.ProductIndicator = Global.GetProductIndicator(model.State, model.Userline);
                model.FormType = Global.GetFormType(model.State, model.Userline, model.FormNo, model.FormVersion, model.ProductIndicator, model.Company);

                if (model.ProductIndicator.Equals(string.Empty))
                    return Global.MsgProdIndFetchError;
                if (model.FormType.Equals(string.Empty))
                    return Global.MsgFormNotFoundError;

                script = Global.FormExpirySQL;
                script = script.Replace(Global.Param_form_no, model.FormNo);
                script = script.Replace(Global.Param_state, model.State);
                script = script.Replace(Global.Param_userline, model.Userline);
                script = script.Replace(Global.Param_version, model.FormVersion);
                script = script.Replace(Global.Param_company, model.Company == "-1" ? string.Empty : model.Company);
                script = script.Replace(Global.Param_end_fdate, model.EntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_nb_fDate, model.NewBusinessEntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_ren_fDate, model.RenewalEntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_product_ind, model.ProductIndicator);
                if (model.FormType == "A")
                    script = script.Replace(Global.Param_AddFormExpiryTriggerScript, Global.SubScript_AddFormExpiryTriggerScript);
                else
                    script = script.Replace(Global.Param_AddFormExpiryTriggerScript, string.Empty);

                if (model.Company != "-1")
                    script = script.Replace(Global.Param_AddCompanyCondition, Global.SubScript_AddCompanyCondition);
                else
                    script = script.Replace(Global.Param_AddCompanyCondition, string.Empty);
            }
            catch (Exception ex) { }

            return script;
        }
    }
}
