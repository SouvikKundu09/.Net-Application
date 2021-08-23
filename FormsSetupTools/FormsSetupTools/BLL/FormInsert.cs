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
    public class FormInsert
    {
        public string GenerateFormsExpiringSQL(FormInsertModel model)
        {
            string script = string.Empty;
            try
            {
                model.ProductIndicator = Global.GetProductIndicator(model.State, model.Userline);
                model.MdlAftrProductIndicator = Global.GetProductIndicator(model.MdlAftrState, model.MdlAftrUserline);
                model.FormType = Global.GetFormType(model.MdlAftrState, model.MdlAftrUserline, model.MdlAftrFormNo, model.MdlAftrFormVersion, model.MdlAftrProductIndicator, model.MdlAftrCompany);

                if (model.ProductIndicator.Equals(string.Empty) || model.MdlAftrProductIndicator.Equals(string.Empty))
                    return Global.MsgProdIndFetchError;
                if (model.FormType.Equals(string.Empty))
                    return Global.MsgFormNotFoundError;

                script = Global.FormInsertSQL;
                script = script.Replace(Global.Param_form_no, model.FormNo);
                script = script.Replace(Global.Param_state, model.State);
                script = script.Replace(Global.Param_userline, model.Userline);
                script = script.Replace(Global.Param_version, model.FormVersion);
                script = script.Replace(Global.Param_company, model.Company);
                script = script.Replace(Global.Param_end_fdate, model.EntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_nb_fDate, model.NewBusinessEntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_ren_fDate, model.RenewalEntryDate.ToString("yyyy-MM-dd"));
                script = script.Replace(Global.Param_product_ind, model.ProductIndicator);
                script = script.Replace(Global.Param_name, model.FormName);
                script = script.Replace(Global.Param_name_abbr, model.FormNameAbbr);
                script = script.Replace(Global.Param_MdlAftr_form_no, model.MdlAftrFormNo);
                script = script.Replace(Global.Param_MdlAftr_state, model.MdlAftrState);
                script = script.Replace(Global.Param_MdlAftr_userline, model.MdlAftrUserline);
                script = script.Replace(Global.Param_MdlAftr_version, model.MdlAftrFormVersion);
                script = script.Replace(Global.Param_MdlAftr_company, model.MdlAftrCompany);
                script = script.Replace(Global.Param_MdlAftr_product_ind, model.MdlAftrProductIndicator);
                if (model.FormType == "A")
                    script = script.Replace(Global.Param_AddFormInsertTriggerScript, Global.SubScript_AddFormInsertTriggerScript);
                else
                    script = script.Replace(Global.Param_AddFormInsertTriggerScript, string.Empty);

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
