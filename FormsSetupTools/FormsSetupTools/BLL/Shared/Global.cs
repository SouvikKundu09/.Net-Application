using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Shared
{
    public static class Global
    {
        public const string AppName = "Forms Setup Tool";
        public static string[] EscapeSpecialChar = { ",", ".", "|", ":" };
        public static string[] DBConfig = { "Not Connected", "Not Connected" };
        public static string JIRAHomeLink = "http://acejira.aceprs.intr";
        public static string JIRABrowseLink = "http://acejira.aceprs.intr/jira/browse/";
        public static string FormsWorkbookLink = "http://acejira.aceprs.intr/jira/browse/PLS-30968";

        // Message display in MessageBox
        public const string MsgProdIndFetchError = "Invalid cobmination. Unable to fetch 'Product Indicator'.";
        public const string MsgExportCanceled = "Export Cancel";
        public const string MsgFormInsertModifyScript = @"Please do necessary modification before exporting to SQL.
Example: a81_sort_key, a81_end_no_short etc.";
        public const string MsgFormNotFoundError = "Form not found in the database. Please enter valid data.";

        // Script Parameter
        public const string Param_form_no = "{form_no}";
        public const string Param_state = "{state}";
        public const string Param_userline = "{userline}";
        public const string Param_version = "{version}";
        public const string Param_old_version = "{old_version}";
        public const string Param_new_version = "{new_version}";
        public const string Param_company = "{company}";
        public const string Param_end_fdate = "{end_fdate}";
        public const string Param_nb_fDate = "{nb_fDate}";
        public const string Param_ren_fDate = "{ren_fDate}";
        public const string Param_product_ind = "{product_ind}";
        public const string Param_name = "{name}";
        public const string Param_name_var = "@name";
        public const string Param_name_abbr = "{name_abbr}";
        public const string Param_name_abbr_var = "@name_abbr";
        public const string Param_MdlAftr_form_no = "{model_after_form_no}";
        public const string Param_MdlAftr_state = "{model_after_state}";
        public const string Param_MdlAftr_userline = "{model_after_userline}";
        public const string Param_MdlAftr_version = "{model_after_version}";
        public const string Param_MdlAftr_company = "{model_after_company}";
        public const string Param_MdlAftr_product_ind = "{model_after_product_ind}";
        public const string Param_AddCompanyCondition = "{company condition}";
        public const string Param_AddCustomName = "{name condition}";
        public const string Param_AddCustomNameAbbr = "{name abbreviation condition}";
        public const string Param_AddFormExpiryTriggerScript = "{form expiry trigger script}";
        public const string Param_AddFormUpdateTriggerScript = "{form update trigger script}";
        public const string Param_AddFormInsertTriggerScript = "{form insert trigger script}";

        #region Script Template
        public const string SubScript_AddCompanyCondition = "AND a01_company = @company";
        public const string SubScript_AddCustomName = "r32_name";
        public const string SubScript_AddCustomNameAbbr = "r32_name_abbr";
        public const string SubScript_AddFormExpiryTriggerScript = @"

    -- Form Expiring Trigger Query
	IF EXISTS(
		SELECT 1 
		FROM end_triggers(NOLOCK)
		WHERE a02_state = @state
			  AND b80_userline = @userline
			  AND a81_end_no = @form_no
			  AND product_ind = @product_ind 
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
		  )
	BEGIN
		UPDATE end_triggers 
		SET r88_end_xdate = @nb_fDate,
			r88_ren_xdate = @ren_fDate
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no
			  AND product_ind = @product_ind 
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
    END";

        public const string SubScript_AddFormUpdateTriggerScript = @"

	-- Form Update Trigger Query
	IF EXISTS(
		SELECT 1 
		FROM end_triggers(NOLOCK) 
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no 
			  AND product_ind = @product_ind 
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
        )
	BEGIN
		INSERT INTO end_triggers
			(
				a81_end_no,
				a02_state,
				b80_userline,
				r88_end_fdate,
				r88_end_xdate,
				ama_trigger_type,
				amc_trigger_value,
				amb_trigger_dfname,
				amd_trigger_dfloc,
				ame_trigger_dftype,
				r88_ren_xdate,
				r88_ren_fdate,
				a01_company,
				product_ind
			)
		SELECT
				a81_end_no,
				a02_state,
				b80_userline,
				@nb_fDate,
				NULL,
				ama_trigger_type,
				amc_trigger_value,
				amb_trigger_dfname,
				amd_trigger_dfloc,
				ame_trigger_dftype,
				NULL,
				@ren_fDate,
				a01_company,
				@product_ind
		FROM end_triggers(NOLOCK)
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no 
			  AND product_ind = @product_ind 
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"

		UPDATE end_triggers
		SET [r88_end_xdate] = @nb_fDate, 
			[r88_ren_xdate] = @ren_fDate
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no 
			  AND product_ind = @product_ind 
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  AND r88_end_fdate != @end_fDate
			  " + Param_AddCompanyCondition + @"
	END";

        public const string SubScript_AddFormInsertTriggerScript = @"

	-- Form Insert Trigger Query
	IF NOT EXISTS(
		SELECT 1 
		FROM end_triggers(NOLOCK) 
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no 
			  AND product_ind = @product_ind 
			  AND a01_company = @company
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL)
	BEGIN
		INSERT INTO end_triggers
			(
				a81_end_no,
				a02_state,
				b80_userline,
				r88_end_fdate,
				r88_end_xdate,
				ama_trigger_type,
				amc_trigger_value,
				amb_trigger_dfname,
				amd_trigger_dfloc,
				ame_trigger_dftype,
				r88_ren_xdate,
				r88_ren_fdate,
				a01_company,
				product_ind
			)
		SELECT
				@form_no,
				@state,
				@userline,
				@nb_fDate,
				NULL,
				ama_trigger_type,
				amc_trigger_value,
				amb_trigger_dfname,
				amd_trigger_dfloc,
				ame_trigger_dftype,
				NULL,
				@ren_fDate,
				@company,
				@product_ind
		FROM end_triggers(NOLOCK)
		WHERE a02_state = @mdl_aftr_state 
			  AND b80_userline = @mdl_aftr_userline 
			  AND a81_end_no = @mdl_aftr_form_no 
			  AND product_ind = @mdl_aftr_product_ind 
			  AND a01_company = @mdl_aftr_company
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
	END";

        public const string GetDBListSQL = @"SELECT NAME FROM sys.databases(NOLOCK) WHERE OWNER_SID != 1 AND NAME LIKE 'SQS%'";

        public const string GetStateListSQL = @"SELECT DISTINCT a02_state AS code, a02_state AS description FROM end_forms (NOLOCK) WHERE r88_end_xdate IS NULL AND r88_ren_xdate IS NULL ORDER BY a02_state";

        public const string GetProductIndSQL = @" -- Get Product Indicator for specific State
SELECT home_product_ind, auto_product_ind, umb_product_ind, val_product_ind, wcft_product_ind, fire_product_ind 
FROM la02_product_ind(NOLOCK) 
WHERE a02_abbr = @state AND (nb_xdate IS NULL OR rn_xdate IS NULL)";

        public const string GetFormTypeSQL = @"SELECT c89_type AS 'Type' 
FROM end_forms(NOLOCK) 
WHERE a02_state = @state 
	AND b80_userline = @userline 
	AND a81_end_no = @form_no
	AND a82_version = @version 
	AND product_ind = @product_ind 
	AND r88_end_xdate IS NULL 
	AND r88_ren_xdate IS NULL";

        public const string FormExpirySQL = @" -- Form Expiring Script
DECLARE @form_no VARCHAR(18)
DECLARE @state VARCHAR(2)
DECLARE @userline VARCHAR(2)
DECLARE @version VARCHAR(5)
DECLARE @company VARCHAR(2)
DECLARE @product_ind NUMERIC(5,1)
DECLARE @end_fDate DATETIME
DECLARE @nb_fDate DATETIME
DECLARE @ren_fDate DATETIME

BEGIN TRANSACTION
BEGIN TRY
	SET @form_no = '" + Param_form_no + @"'
	SET @state = '" + Param_state + @"'
	SET @userline = '" + Param_userline + @"'
	SET @version = '" + Param_version + @"'
	SET @company = '" + Param_company + @"'
	SET @end_fDate = '" + Param_end_fdate + @"'
	SET @nb_fDate = '" + Param_nb_fDate + @"'
	SET @ren_fDate = '" + Param_ren_fDate + @"'
	SET @product_ind = '" + Param_product_ind + @"'

    -- Form Expiring Query
	IF EXISTS(
		SELECT 1 
		FROM end_forms(NOLOCK)
		WHERE a02_state = @state
			  AND b80_userline = @userline
			  AND a81_end_no = @form_no
			  AND product_ind = @product_ind 
			  AND a82_version = @version
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
		)
	BEGIN
		UPDATE end_forms 
		SET r88_end_xdate = @nb_fDate,
			r88_ren_xdate = @ren_fDate,
			entry_xdate = @end_fDate
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no
			  AND product_ind = @product_ind 
			  AND a82_version = @version
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
	END" + Param_AddFormExpiryTriggerScript + @"

	PRINT 'Form expired successfully'
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	PRINT 'Error occured ' + @@ERROR
	ROLLBACK TRANSACTION
END CATCH
";

        public const string FormUpdateSQL = @"-- Form Update Script
DECLARE @form_no VARCHAR(18)
DECLARE @state VARCHAR(2)
DECLARE @userline VARCHAR(2)
DECLARE @old_version VARCHAR(5)
DECLARE @new_version VARCHAR(5)
DECLARE @company VARCHAR(2)
DECLARE @product_ind NUMERIC(5,1)
DECLARE @end_fDate DATETIME
DECLARE @nb_fDate DATETIME
DECLARE @ren_fDate DATETIME
DECLARE @name VARCHAR(45)
DECLARE @name_abbr VARCHAR(25)

BEGIN TRANSACTION
BEGIN TRY
	SET @form_no = '" + Param_form_no + @"'
	SET @state = '" + Param_state + @"'
	SET @userline = '" + Param_userline + @"'
	SET @old_version = '" + Param_old_version + @"'
	SET @new_version = '" + Param_new_version + @"'
	SET @company = '" + Param_company + @"'
	SET @end_fDate = '" + Param_end_fdate + @"'
	SET @nb_fDate = '" + Param_nb_fDate + @"'
	SET @ren_fDate = '" + Param_ren_fDate + @"'
	SET @product_ind = '" + Param_product_ind + @"'
	SET @name = '" + Param_name + @"'
	SET @name_abbr = '" + Param_name_abbr + @"'

	-- Form Update Query
	IF EXISTS(
		SELECT 1 
		FROM end_forms 
		WHERE a02_state = @state
			  AND b80_userline = @userline
			  AND a81_end_no = @form_no
			  AND product_ind = @product_ind
			  AND a82_version = @old_version
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
        )
	 BEGIN
		INSERT INTO end_forms
			(
				a01_company,
				a02_state,
				a81_edl_id,
				a81_eem_instr,
				a81_end_no,
				a81_sort_key,
				c89_type,
				a82_version,
				am9_trigger_level,
				b80_userline,
				b97_sbl,
				e62_asl,
				c07_limit_sw,
				c07_mcov_limit,
				c10_form1,
				c10_form2,
				c10_form3,
				c10_form4,
				c10_form5,
				c10_form6,
				c10_form8,
				c87_coverage,
				c89_after_date,
				c89_disc_srchg,
				c89_print_after,
				c89_print_always,
				c89_print_ind,
				c89_print_once,
				c91_freeform_sw,
				c91_parm1_hdr,
				c91_ron,
				c92_freeform_sw,
				c92_parm2_hdr,
				c92_ron,
				c93_freeform_sw,
				c93_parm3_hdr,
				c93_ron,
				c94_freeform_sw,
				c94_parm4_hdr,
				c94_ron,
				c95_freeform_sw,
				c95_parm5_hdr,
				c95_ron,
				comm_nl,
				comm_ren,
				d42_written_sw,
				entry_fdate,
				entry_xdate,
				f20_claims_sw,
				f20_res_pay,
				g57_descr_sw,
				i42_autorate_sw,
				product_ind,
				r02_delete,
				r32_name,
				r32_name_abbr,
				r88_end_fdate,
				r88_end_xdate,
				r88_ren_fdate,
				r88_ren_xdate,
				r89_bureau,
				a81_end_no_short
			)
		SELECT 
				a01_company,
				a02_state,
				a81_edl_id,
				a81_eem_instr,
				a81_end_no,
				a81_sort_key,
				c89_type,
				@new_version,
				am9_trigger_level,
				b80_userline,
				b97_sbl,
				e62_asl,
				c07_limit_sw,
				c07_mcov_limit,
				c10_form1,
				c10_form2,
				c10_form3,
				c10_form4,
				c10_form5,
				c10_form6,
				c10_form8,
				c87_coverage,
				c89_after_date,
				c89_disc_srchg,
				c89_print_after,
				c89_print_always,
				c89_print_ind,
				c89_print_once,
				c91_freeform_sw,
				c91_parm1_hdr,
				c91_ron,
				c92_freeform_sw,
				c92_parm2_hdr,
				c92_ron,
				c93_freeform_sw,
				c93_parm3_hdr,
				c93_ron,
				c94_freeform_sw,
				c94_parm4_hdr,
				c94_ron,
				c95_freeform_sw,
				c95_parm5_hdr,
				c95_ron,
				comm_nl,
				comm_ren,
				d42_written_sw,
				@end_fDate,
				NULL,
				f20_claims_sw,
				f20_res_pay,
				g57_descr_sw,
				i42_autorate_sw,
				@product_ind,
				r02_delete,
				" + Param_AddCustomName + @",
				" + Param_AddCustomNameAbbr + @",
				@nb_fDate,
				NULL,
				@ren_fDate,
				NULL,
				r89_bureau,
				a81_end_no_short
		FROM end_forms
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no 
			  AND product_ind = @product_ind 
			  AND a82_version = @old_version 
			  AND r88_end_xdate IS NULL
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"

		UPDATE end_forms 
		SET [entry_xdate] = @end_fDate, 
			[r88_end_xdate] = @nb_fDate, 
			[r88_ren_xdate] = @ren_fDate
		WHERE a02_state = @state 
			  AND b80_userline = @userline 
			  AND a81_end_no = @form_no 
			  AND product_ind = @product_ind 
			  AND a82_version = @old_version
			  AND r88_end_xdate IS NULL
			  AND r88_ren_xdate IS NULL
			  " + Param_AddCompanyCondition + @"
	END" + Param_AddFormUpdateTriggerScript + @"

	PRINT 'Form updated successfully'
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	PRINT 'Error occured ' + @@ERROR
	ROLLBACK TRANSACTION
END CATCH
";

        public const string FormInsertSQL = @"-- Form Insert Script
DECLARE @form_no VARCHAR(18)
DECLARE @state VARCHAR(2)
DECLARE @userline VARCHAR(2)
DECLARE @version VARCHAR(5)
DECLARE @company VARCHAR(2)
DECLARE @product_ind NUMERIC(5,1)
DECLARE @end_fDate DATETIME
DECLARE @nb_fDate DATETIME
DECLARE @ren_fDate DATETIME
DECLARE @name VARCHAR(45)
DECLARE @name_abbr VARCHAR(25)
DECLARE @mdl_aftr_form_no VARCHAR(18)
DECLARE @mdl_aftr_state VARCHAR(2)
DECLARE @mdl_aftr_userline VARCHAR(2)
DECLARE @mdl_aftr_version VARCHAR(5)
DECLARE @mdl_aftr_company VARCHAR(2)
DECLARE @mdl_aftr_product_ind NUMERIC(5,1)

BEGIN TRANSACTION
BEGIN TRY
	SET @form_no = '" + Param_form_no + @"'
	SET @state = '" + Param_state + @"'
	SET @userline = '" + Param_userline + @"'
	SET @version = '" + Param_version + @"'
	SET @company = '" + Param_company + @"'
	SET @end_fDate = '" + Param_end_fdate + @"'
	SET @nb_fDate = '" + Param_nb_fDate + @"'
	SET @ren_fDate = '" + Param_ren_fDate + @"'
	SET @product_ind = '" + Param_product_ind + @"'
	SET @name = '" + Param_name + @"'
	SET @name_abbr = '" + Param_name_abbr + @"'
	SET @mdl_aftr_form_no = '" + Param_MdlAftr_form_no + @"'
	SET @mdl_aftr_state = '" + Param_MdlAftr_state + @"'
	SET @mdl_aftr_userline = '" + Param_MdlAftr_userline + @"'
	SET @mdl_aftr_version = '" + Param_MdlAftr_version + @"'
	SET @mdl_aftr_company = '" + Param_MdlAftr_company + @"'
	SET @mdl_aftr_product_ind = '" + Param_MdlAftr_product_ind + @"'

	-- Form Insert Query
	IF NOT EXISTS(
		SELECT 1 
		FROM end_forms 
		WHERE a02_state = @state
			  AND b80_userline = @userline
			  AND a81_end_no = @form_no
			  AND product_ind = @product_ind
			  AND a82_version = @version
			  AND a01_company = @company
			  AND r88_end_xdate IS NULL 
			  AND r88_ren_xdate IS NULL)
	 BEGIN
		INSERT INTO end_forms
			(
				a01_company,
				a02_state,
				a81_edl_id,
				a81_eem_instr,
				a81_end_no,
				a81_sort_key,
				c89_type,
				a82_version,
				am9_trigger_level,
				b80_userline,
				b97_sbl,
				e62_asl,
				c07_limit_sw,
				c07_mcov_limit,
				c10_form1,
				c10_form2,
				c10_form3,
				c10_form4,
				c10_form5,
				c10_form6,
				c10_form8,
				c87_coverage,
				c89_after_date,
				c89_disc_srchg,
				c89_print_after,
				c89_print_always,
				c89_print_ind,
				c89_print_once,
				c91_freeform_sw,
				c91_parm1_hdr,
				c91_ron,
				c92_freeform_sw,
				c92_parm2_hdr,
				c92_ron,
				c93_freeform_sw,
				c93_parm3_hdr,
				c93_ron,
				c94_freeform_sw,
				c94_parm4_hdr,
				c94_ron,
				c95_freeform_sw,
				c95_parm5_hdr,
				c95_ron,
				comm_nl,
				comm_ren,
				d42_written_sw,
				entry_fdate,
				entry_xdate,
				f20_claims_sw,
				f20_res_pay,
				g57_descr_sw,
				i42_autorate_sw,
				product_ind,
				r02_delete,
				r32_name,
				r32_name_abbr,
				r88_end_fdate,
				r88_end_xdate,
				r88_ren_fdate,
				r88_ren_xdate,
				r89_bureau,
				a81_end_no_short
			)
		SELECT TOP 1
				@company,
				@state,
				a81_edl_id,
				a81_eem_instr,
				@form_no,
				a81_sort_key,
				c89_type,
				@version,
				am9_trigger_level,
				@userline,
				b97_sbl,
				e62_asl,
				c07_limit_sw,
				c07_mcov_limit,
				c10_form1,
				c10_form2,
				c10_form3,
				c10_form4,
				c10_form5,
				c10_form6,
				c10_form8,
				c87_coverage,
				c89_after_date,
				c89_disc_srchg,
				c89_print_after,
				c89_print_always,
				c89_print_ind,
				c89_print_once,
				c91_freeform_sw,
				c91_parm1_hdr,
				c91_ron,
				c92_freeform_sw,
				c92_parm2_hdr,
				c92_ron,
				c93_freeform_sw,
				c93_parm3_hdr,
				c93_ron,
				c94_freeform_sw,
				c94_parm4_hdr,
				c94_ron,
				c95_freeform_sw,
				c95_parm5_hdr,
				c95_ron,
				comm_nl,
				comm_ren,
				d42_written_sw,
				@end_fDate,
				NULL,
				f20_claims_sw,
				f20_res_pay,
				g57_descr_sw,
				i42_autorate_sw,
				@product_ind,
				r02_delete,
				" + Param_AddCustomName + @",
				" + Param_AddCustomNameAbbr + @",
				@nb_fDate,
				NULL,
				@ren_fDate,
				NULL,
				r89_bureau,
				@form_no
		FROM end_forms
		WHERE a02_state = @mdl_aftr_state 
			  AND b80_userline = @mdl_aftr_userline 
			  AND a81_end_no = @mdl_aftr_form_no 
			  AND product_ind = @mdl_aftr_product_ind 
			  AND a82_version = @mdl_aftr_version 
			  AND a01_company = @mdl_aftr_company
			  AND r88_end_xdate IS NULL
			  AND r88_ren_xdate IS NULL
	END" + Param_AddFormInsertTriggerScript + @"

	PRINT 'Form inserted successfully'
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	PRINT 'Error occured ' + @@ERROR
	ROLLBACK TRANSACTION
END CATCH
";
        #endregion

        #region Custom Function

        public static bool IsExistInString(string sourceString, string[] characterList)
        {
            foreach (string str in characterList)
            {
                if (sourceString.Contains(str))
                    return true;
            }
            return false;
        }

        public static string GetProductIndicator(string state, string userline)
        {
            string productIndicator = string.Empty;

            var param = new System.Data.SqlClient.SqlParameter[0];
            DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@state", state));

            System.Data.DataSet ds = DAL.SQLHelper.GetDatasetByQuery(GetProductIndSQL, param);
            try
            {
                if (!ReferenceEquals(ds, null) && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    switch (userline)
                    {
                        case "24":
                            productIndicator = ds.Tables[0].Rows[0]["home_product_ind"].ToString();
                            break;
                        case "01":
                            productIndicator = ds.Tables[0].Rows[0]["auto_product_ind"].ToString();
                            break;
                        case "44":
                            productIndicator = ds.Tables[0].Rows[0]["umb_product_ind"].ToString();
                            break;
                        case "68":
                            productIndicator = ds.Tables[0].Rows[0]["val_product_ind"].ToString();
                            break;
                        case "34":
                            productIndicator = ds.Tables[0].Rows[0]["wcft_product_ind"].ToString();
                            break;
                        case "22":
                            productIndicator = ds.Tables[0].Rows[0]["fire_product_ind"].ToString();
                            break;
                        default:
                            productIndicator = string.Empty;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                productIndicator = string.Empty;
            }
            return productIndicator;
        }

        public static Dictionary<string, string> GetStateList()
        {
            Dictionary<string, string> states = new Dictionary<string, string>();

            var param = new System.Data.SqlClient.SqlParameter[0];

            System.Data.DataSet ds = DAL.SQLHelper.GetDatasetByQuery(GetStateListSQL, param);
            try
            {
                if (!ReferenceEquals(ds, null) && ds.Tables.Count > 0)
                {
                    states.Add("Select", "-1");
                    foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                    {
                        states.Add(row["description"].ToString(), row["code"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                states = new Dictionary<string, string>();
            }
            return states;
        }

        public static string GetFormType(string state, string userline, string formNo, string version, string productInd, string company)
        {
            string formType = string.Empty;

            var param = new System.Data.SqlClient.SqlParameter[0];
            DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@state", state));
            DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@userline", userline));
            DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@form_no", formNo));
            DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@version", version));
            DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@product_ind", productInd));

            System.Data.DataSet ds = new System.Data.DataSet();
            if (company != "-1")
            {
                DAL.SQLHelper.AddParameter(ref param, new System.Data.SqlClient.SqlParameter("@company", company));
                ds = DAL.SQLHelper.GetDatasetByQuery(string.Concat(GetFormTypeSQL, Environment.NewLine, SubScript_AddCompanyCondition), param);
            }
            else
            {
                ds = DAL.SQLHelper.GetDatasetByQuery(GetFormTypeSQL, param);
            }

            try
            {
                if (!ReferenceEquals(ds, null) && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    formType = ds.Tables[0].Rows[0]["Type"].ToString();
                }
            }
            catch (Exception ex)
            {
                formType = string.Empty;
            }
            return formType;
        }

        public static string[] GetDatabaseList(string oldServer, string currentServer, string oldDB)
        {
            System.Data.SqlClient.SqlConnection customDBCon = new System.Data.SqlClient.SqlConnection();
            string[] databaseList = null;

            var param = new System.Data.SqlClient.SqlParameter[0];
            customDBCon.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqs"].ToString().Replace(oldServer, currentServer).Replace(oldDB, "master");

            System.Data.DataSet ds = DAL.SQLHelper.GetDatasetByQuery(GetDBListSQL, param, customDBCon);
            try
            {
                if (!ReferenceEquals(ds, null) && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    databaseList = new string[ds.Tables[0].Rows.Count];
                    foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                    {
                        databaseList[index] = row["NAME"].ToString();
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                databaseList = null;
            }
            return databaseList;
        }

        #endregion
    }
}
