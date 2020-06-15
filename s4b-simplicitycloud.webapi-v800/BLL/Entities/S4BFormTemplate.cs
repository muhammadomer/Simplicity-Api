using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class S4BFormTemplate : ResponseModel
    {
        public long? Sequence { get; set; }
        public int PageNo { get; set; }
        public string FieldName { get; set; }
        public string FieldData { get; set; }
        public string FieldPosition { get; set; }
        public string FieldType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string CreatedByUserName { get; set; }
        public string LastAmendedByUserName { get; set; }
        public List<TemplateImages> TemplateImages { get; set; }
    }

    public class Template5EnvirnData : ResponseModel
    {
        public long? Sequence { get; set; }
        public int PageNo { get; set; }
        public string FileCabId { get; set; }
        public string VARIABLE_PG1_JOB_SEQUENCE { get; set; }
        public string VARIABLE_PG1_JOB_ADDRESS_ID { get; set; }
        public string VARIABLE_PG1_DIARY_ENTRY_ID { get; set; }
        public string VARIABLE_PG1_JOB_CLIENT_ID { get;set;}
        public string VARIABLE_PG1_USER_ID { get;set;}
        public string FIELD_PG1_RA_01 { get;set;}
        public string FIELD_PG1_RA_02 { get;set;}
        public string FIELD_PG1_RA_03 { get; set; }
        public string FIELD_PG1_RA_04 { get; set; }
        public string FIELD_PG1_RA_05 { get;set;}
        public string FIELD_PG1_RA_06 { get; set; }
        public string FIELD_PG1_RA_07 { get; set; }
        public string FIELD_PG1_RA_08 { get;set;}
        public string FIELD_PG1_RA_09 { get; set; }
        public string FIELD_PG1_RA_10 { get;set;}
        public string FIELD_PG1_RA_11 { get; set; }
        public string FIELD_PG1_RA_12 { get; set; }
        public string FIELD_PG1_RA_13 { get; set; }
        public string FIELD_PG1_RA_14 { get; set; }
        public string FIELD_PG1_RA_15 { get; set; }
        public string FIELD_PG1_RA_16 { get; set; }
        public string FIELD_PG1_RA_17 { get; set; }
        public string FIELD_PG1_RA_18 { get; set; }
        public string FIELD_PG1_RA_19 { get; set; }
        public string FIELD_PG1_RA_20 { get; set; }
        public string FIELD_PG1_RA_21 { get; set; }
        public string FIELD_PG1_RA_22 { get; set; }
        public string FIELD_PG1_RA_23 { get; set; }
        public string FIELD_PG1_RA_24 { get; set; }
        public string FIELD_PG1_RA_25 { get; set; }
        public string FIELD_PG1_RA_26 { get; set; }
        public string FIELD_PG1_RA_27 { get; set; }
        public string FIELD_PG1_RA_28 { get; set; }
        public string FIELD_PG1_RA_29 { get; set; }
        public string COL_PG1_COLOUR_01 { get; set; }
        public string COL_PG1_COLOUR_02 { get;set;}
        public string COL_PG1_COLOUR_03 { get;set;}
        public string COL_PG1_COLOUR_04 { get;set;}
        public string COL_PG1_COLOUR_05 { get;set;}
        public string COL_PG1_COLOUR_06 { get;set;}
        public string FIELD_PG2_RA_30 { get; set; }
        public string FIELD_PG2_RA_31 { get; set; }
        public string FIELD_PG2_RA_32 { get;set;}
        public string FIELD_PG2_RA_33 { get;set;}
        public string FIELD_PG2_RA_34 { get; set; }
        public string FIELD_PG2_RA_35 { get; set; }
        public string FIELD_PG2_RA_36 { get;set;}
        public string FIELD_PG2_RA_37 { get; set; }
        public string FIELD_PG2_RA_38 { get; set; }
        public string FIELD_PG2_RA_39 { get;set;}
        public string FIELD_PG2_RA_40 { get; set; }
        public string FIELD_PG2_RA_41 { get; set; }
        public string FIELD_PG2_RA_42 { get; set; }
        public string FIELD_PG2_RA_43 { get; set; }
        public string FIELD_PG2_RA_44 { get; set; }
        public string FIELD_PG2_RA_45 { get; set; }
        public string FIELD_PG2_RA_46 { get; set; }
        public string FIELD_PG2_RA_47 { get; set; }       
        public string COL_PG2_COLOUR_07 { get;set;}
        public string COL_PG2_COLOUR_08 { get;set;}
        public string FIELD_PG2_DO_NOT_PROCEED_REASON { get; set; }
        public string FIELD_PG2_DO_NOT_PROCEED_REPORTED_TO { get; set; }
        public string FIELD_PG2_STOP_REASON { get; set; }
        public string FIELD_PG2_STOP_ACTION_TAKEN { get; set; }
        public string FIELD_PG2_SIGNED { get;set;}
        public string FIELD_PG2_PRINT { get;set;}
        public string FIELD_PG2_DECLARATION { get;set;}
        public string FIELD_PG2_ROW01_ITEM_NO { get; set; }
        public string FIELD_PG2_ROW01_ACTION { get; set; }
        public string FIELD_PG2_ROW01_SAFE_TO_WORK { get; set; }
        public string FIELD_PG2_ROW02_ITEM_NO { get; set; }
        public string FIELD_PG2_ROW02_ACTION { get; set; }
        public string FIELD_PG2_ROW02_SAFE_TO_WORK { get; set; }
        public string FIELD_PG2_ROW03_ITEM_NO { get; set; }
        public string FIELD_PG2_ROW03_ACTION { get; set; }
        public string FIELD_PG2_ROW03_SAFE_TO_WORK { get; set; }
        public string FIELD_PG2_ROW04_ITEM_NO { get; set; }
        public string FIELD_PG2_ROW04_ACTION { get; set; }
        public string FIELD_PG2_ROW04_SAFE_TO_WORK { get; set; }
        public string FIELD_PG2_COMMENTS { get; set; }
        public string FIELD_PG2_SIGNATURE { get; set; }
        public string FIELD_PG2_DATE { get; set; }
        public string VAR_PG1_JOB_REF { get; set; }
        public string VAR_PG1_DIARY_ENTRY_DATE { get; set; }
        public string VAR_PG1_DIARY_RESOURCE { get; set; }
        public string VAR_PG1_ASSISTED_BY { get; set; }
        public string VAR_PG1_JOB_ADDRESS { get; set; }
        public string VAR_PG1_JOB_CLIENT_NAME { get; set; }
        public string VAR_PG1_JOB_DESC { get; set; }
        public string VAR_PG1_USER_RATE { get; set; }
        public string VAR_PG3_JOB_OCCUPIER_NAME { get;set;}
        public string VAR_PG3_JOB_OCCUPIER_TEL_HOME { get;set;}
        public string VAR_PG3_JOB_OCCUPIER_TEL_MOBILE { get;set;}
        public string FIELD_PG3_VISIT_STATUS { get; set; }
        public string FIELD_PG3_PERMIT_TO_WORK_SIGN { get; set; }
        public string FIELD_PG3_TIME_START { get;set;}
        public string FIELD_PG3_CONFIRM { get;set;}
        public string FIELD_PG3_DO_NOT_PROCEED { get; set; }
        public string FIELD_PG3_STOP { get; set; }
        public string FIELD_PG3_SAFE { get; set; }
        public string FIELD_PG3_DESCRIPTION_OF_WORKS { get; set; }
        public string FIELD_PG3_FIRST_TIME_FIX { get; set; }
        public string FIELD_PG3_VAN_STOCK { get; set; }
        public string FIELD_PG3_ROW01_FURTHER_WORKS { get; set; }
        public string FIELD_PG3_ROW01_FURTHER_WORKS_HOURS { get;set;}
        public string FIELD_PG3_ROW02_FURTHER_WORKS { get; set; }
        public string FIELD_PG3_ROW02_FURTHER_WORKS_HOURS { get; set; }
        public string FIELD_PG3_ROW03_FURTHER_WORKS { get; set; }
        public string FIELD_PG3_ROW03_FURTHER_WORKS_HOURS { get; set; }
        public string FIELD_PG3_ROW04_FURTHER_WORKS { get; set; }
        public string FIELD_PG3_ROW04_FURTHER_WORKS_HOURS { get; set; }
        public string FIELD_PG3_ROW05_FURTHER_WORKS { get; set; }
        public string FIELD_PG3_ROW05_FURTHER_WORKS_HOURS { get; set; }
        public string FIELD_PG3_ROW06_FURTHER_WORKS { get; set; }
        public string FIELD_PG3_ROW06_FURTHER_WORKS_HOURS { get; set; }
        public string CAM_PG3_CAMERA_01 { get;set;}
        public string VAR_PG3_TOTAL_MAN_HOURS { get;set;}
        public string FIELD_PG5_FINISH_TIME { get; set; }
        public string VAR_PG3_JOB_CLIENT_REF { get; set; }
        public string VAR_PG3_JOB_WORK_CENTRE { get; set; }
        public string FIELD_PG4_CAMERA_02 { get; set; }
        public string FIELD_PG4_CAMERA_03 { get; set; }
        public string CAM_PG4_CAMERA_02 { get; set; }
        public string CAM_PG4_CAMERA_03 { get; set; }
        public string FIELD_PG4_ARE_PARTS_REQUIRED { get; set; }
        public string FIELD_PG4_ROW01_STATUS { get; set; }
        public string FIELD_PG4_ROW01_SUPPLIER { get; set; }
        public string FIELD_PG4_ROW01_CODE { get; set; }
        public string FIELD_PG4_ROW01_DESC { get; set; }
        public string FIELD_PG4_ROW01_QTY { get; set; }
        public string FIELD_PG4_ROW01_AMOUNT { get; set; }
        public string VAR_PG4_ROW01_TOTAL { get; set; }
        public string FIELD_PG4_ROW02_STATUS { get; set; }
        public string FIELD_PG4_ROW02_SUPPLIER { get; set; }
        public string FIELD_PG4_ROW02_CODE { get; set; }
        public string FIELD_PG4_ROW02_DESC { get; set; }
        public string FIELD_PG4_ROW02_QTY { get; set; }
        public string FIELD_PG4_ROW02_AMOUNT { get; set; }
        public string VAR_PG4_ROW02_TOTAL { get; set; }
        public string FIELD_PG4_ROW03_STATUS { get; set; }
        public string FIELD_PG4_ROW03_SUPPLIER { get; set; }
        public string FIELD_PG4_ROW03_CODE { get; set; }
        public string FIELD_PG4_ROW03_DESC { get; set; }
        public string FIELD_PG4_ROW03_QTY { get; set; }
        public string FIELD_PG4_ROW03_AMOUNT { get; set; }
        public string VAR_PG4_ROW03_TOTAL { get; set; }
        public string FIELD_PG4_ROW04_STATUS { get; set; }
        public string FIELD_PG4_ROW04_SUPPLIER { get; set; }
        public string FIELD_PG4_ROW04_CODE { get; set; }
        public string FIELD_PG4_ROW04_DESC { get; set; }
        public string FIELD_PG4_ROW04_QTY { get; set; }
        public string FIELD_PG4_ROW04_AMOUNT { get; set; }
        public string VAR_PG4_ROW04_TOTAL { get; set; }
        public string FIELD_PG4_ROW05_STATUS { get; set; }
        public string FIELD_PG4_ROW05_SUPPLIER { get; set; }
        public string FIELD_PG4_ROW05_CODE { get; set; }
        public string FIELD_PG4_ROW05_DESC { get; set; }
        public string FIELD_PG4_ROW05_QTY { get; set; }
        public string FIELD_PG4_ROW05_AMOUNT { get; set; }
        public string VAR_PG4_ROW05_TOTAL { get; set; }
        public string FIELD_PG4_MAN_HOURS { get; set; }
        public string VAR_PG4_UPLIFT_TOTAL { get; set; }
        public string VAR_PG4_MAN_HOURS_TOTAL { get; set; }
        public string FIELD_PG4_DUE_DATE { get; set; }
        public string VAR_PG4_SUB_TOTAL_EX_VAT { get;set;}
        public string FIELD_PG4_DELIVERY_NOTE_NO { get; set; }
        public string FIELD_PG4_PO_NO { get; set; }
        public string VAR_PG4_TOTAL_INC_VAT { get;set;}
        public string FIELD_PG4_FIRST_HOUR { get; set; }
        public string FIELD_PG4_SUBSEQUENT_HALF_HOURS { get; set; }
        public string VAR_PG4_SUB_TOTAL { get;set;}
        public string VAR_PG4_VAT { get; set; }
        public string FIELD_PG4_QUOTE_SUBMIT { get; set; }
        public string FIELD_PG4_QUOTE_SUBMIT_DATE { get; set; }
        public string FIELD_PG4_UPLIFT_APPROVED { get; set; }
        public string FIELD_PG4_UPLIFT_DATE { get; set; }
        public string FIELD_PG4_PARKING { get; set; }
        public string FIELD_PG4_PARKING_TICKET { get; set; }
        public string FIELD_PG4_CONGESTION_CHARGE { get; set; }
        public string CAM_PG4_CAMERA_04 { get; set; }
        public string FIELD_PG4_CAMERA_04 { get; set; }
        public string CAM_PG4_CAMERA_05 { get; set; }
        public string FIELD_PG4_CAMERA_05 { get; set; }
        public string FIELD_PG4_ROW06_HOURS { get; set; }
        public string FIELD_PG4_ROW06_DESC { get; set; }
        public string FIELD_PG4_ROW07_HOURS { get; set; }
        public string FIELD_PG4_ROW07_DESC { get; set; }
        public string FILED_PG5_CLIENT_NOT_PRESENT { get; set; }
        public string FIELD_PG5_ROW01_DESC { get;set;}
        public string FIELD_PG5_ROW01_HOURS { get;set;}
        public string FIELD_PG5_ROW02_DESC { get;set;}
        public string FIELD_PG5_ROW02_HOURS { get;set;}
        public string FIELD_PG5_CAMERA_04 { get;set;}
        public string CAM_PG5_CAMERA_04 { get;set;}
        public string FIELD_PG5_CAMERA_05 { get;set;}
        public string CAM_PG5_CAMERA_05 { get;set;}
        public string FIELD_PG5_ENGINEER_SIGNATURE { get;set;}
        public string FIELD_PG5_ENGINEER_SIGNATURE_DATE { get;set;}
        public string FIELD_PG5_CLIENT_SIGNATURE { get;set;}
        public string FIELD_PG5_CLIENT_SIGNATURE_DATE { get;set;}
        public string FIELD_PG5_CLIENT_NAME { get; set; }
        public string FIELD_PG5_CLIENT_POSITION { get; set; }
        public string FIELD_PG5_SUBMIT { get;set;}
        
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string CreatedByUserName { get; set; }
        public string LastAmendedByUserName { get; set; }
        public List<SubmissionsImagesFh> SiteInspectionImages { get; set; }

        internal Dictionary<string, string> GetFieldMappingDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("VARIABLE_PG1_JOB_SEQUENCE", this.VARIABLE_PG1_JOB_SEQUENCE);
            dictionary.Add("VARIABLE_PG1_JOB_ADDRESS_ID", this.VARIABLE_PG1_JOB_ADDRESS_ID);
            dictionary.Add("VARIABLE_PG1_DIARY_ENTRY_ID", this.VARIABLE_PG1_DIARY_ENTRY_ID);
            dictionary.Add("VARIABLE_PG1_JOB_CLIENT_ID", VARIABLE_PG1_JOB_CLIENT_ID);
            dictionary.Add("VARIABLE_PG1_USER_ID", VARIABLE_PG1_USER_ID);
            dictionary.Add("VAR_PG1_JOB_REF", VAR_PG1_JOB_REF);
            dictionary.Add("VAR_PG1_DIARY_ENTRY_DATE", VAR_PG1_DIARY_ENTRY_DATE.ToString());
            dictionary.Add("VAR_PG1_DIARY_RESOURCE", VAR_PG1_DIARY_RESOURCE);
            dictionary.Add("VAR_PG1_ASSISTED_BY", VAR_PG1_ASSISTED_BY);
            dictionary.Add("VAR_PG1_JOB_ADDRESS", VAR_PG1_JOB_ADDRESS);
            dictionary.Add("VAR_PG1_JOB_CLIENT_NAME", VAR_PG1_JOB_CLIENT_NAME);
            dictionary.Add("VAR_PG1_JOB_DESC", VAR_PG1_JOB_DESC);
            dictionary.Add("VAR_PG1_USER_RATE", VAR_PG1_USER_RATE);
            dictionary.Add("COL_PG1_COLOUR_01", COL_PG1_COLOUR_01);
            dictionary.Add("FIELD_PG2_DO_NOT_PROCEED_REASON", FIELD_PG2_DO_NOT_PROCEED_REASON);
            dictionary.Add("FIELD_PG2_DO_NOT_PROCEED_REPORTED_TO", FIELD_PG2_DO_NOT_PROCEED_REPORTED_TO);
            dictionary.Add("FIELD_PG2_STOP_REASON", FIELD_PG2_STOP_REASON);
            dictionary.Add("FIELD_PG2_STOP_ACTION_TAKEN", FIELD_PG2_STOP_ACTION_TAKEN);
            dictionary.Add("FIELD_PG2_SIGNED", FIELD_PG2_SIGNED);
            dictionary.Add("FIELD_PG2_PRINT", FIELD_PG2_PRINT);
            dictionary.Add("FIELD_PG2_DECLARATION", FIELD_PG2_DECLARATION);
            dictionary.Add("FIELD_PG2_COMMENTS", FIELD_PG2_COMMENTS);
            dictionary.Add("FIELD_PG2_SIGNATURE", FIELD_PG2_SIGNATURE);
            dictionary.Add("FIELD_PG2_DATE", FIELD_PG2_DATE.ToString());
            dictionary.Add("VAR_PG3_JOB_OCCUPIER_NAME", VAR_PG3_JOB_OCCUPIER_NAME);
            dictionary.Add("VAR_PG3_JOB_OCCUPIER_TEL_HOME", VAR_PG3_JOB_OCCUPIER_TEL_HOME);
            dictionary.Add("VAR_PG3_JOB_OCCUPIER_TEL_MOBILE", VAR_PG3_JOB_OCCUPIER_TEL_MOBILE);
            dictionary.Add("FIELD_PG3_VISIT_STATUS", FIELD_PG3_VISIT_STATUS);
            dictionary.Add("FIELD_PG3_PERMIT_TO_WORK_SIGN", FIELD_PG3_PERMIT_TO_WORK_SIGN);
            dictionary.Add("FIELD_PG3_TIME_START", FIELD_PG3_TIME_START);
            dictionary.Add("FIELD_PG3_CONFIRM", FIELD_PG3_CONFIRM);
            dictionary.Add("FIELD_PG3_DO_NOT_PROCEED", FIELD_PG3_DO_NOT_PROCEED);
            dictionary.Add("FIELD_PG3_STOP", FIELD_PG3_STOP);
            dictionary.Add("FIELD_PG3_SAFE", FIELD_PG3_SAFE);
            dictionary.Add("FIELD_PG3_DESCRIPTION_OF_WORKS", FIELD_PG3_DESCRIPTION_OF_WORKS);
            dictionary.Add("CAM_PG3_CAMERA_01", CAM_PG3_CAMERA_01);
            dictionary.Add("FIELD_PG3_FIRST_TIME_FIX", FIELD_PG3_FIRST_TIME_FIX);
            dictionary.Add("FIELD_PG3_VAN_STOCK", FIELD_PG3_VAN_STOCK);
            dictionary.Add("VAR_PG3_TOTAL_MAN_HOURS", VAR_PG3_TOTAL_MAN_HOURS);
            dictionary.Add("FIELD_PG5_FINISH_TIME", FIELD_PG5_FINISH_TIME);
            dictionary.Add("VAR_PG3_JOB_CLIENT_REF", VAR_PG3_JOB_CLIENT_REF);
            dictionary.Add("VAR_PG3_JOB_WORK_CENTRE", VAR_PG3_JOB_WORK_CENTRE);
            dictionary.Add("FIELD_PG4_CAMERA_02", FIELD_PG4_CAMERA_02);
            dictionary.Add("CAM_PG4_CAMERA_02", CAM_PG4_CAMERA_02);
            dictionary.Add("FIELD_PG4_CAMERA_03", FIELD_PG4_CAMERA_03);
            dictionary.Add("CAM_PG4_CAMERA_03", CAM_PG4_CAMERA_03);
            dictionary.Add("FIELD_PG4_ARE_PARTS_REQUIRED", FIELD_PG4_ARE_PARTS_REQUIRED);
            dictionary.Add("FIELD_PG4_MAN_HOURS", FIELD_PG4_MAN_HOURS);
            dictionary.Add("VAR_PG4_MAN_HOURS_TOTAL", VAR_PG4_MAN_HOURS_TOTAL);
            dictionary.Add("FIELD_PG4_DUE_DATE", FIELD_PG4_DUE_DATE);
            dictionary.Add("VAR_PG4_UPLIFT_TOTAL", VAR_PG4_UPLIFT_TOTAL);
            dictionary.Add("VAR_PG4_SUB_TOTAL_EX_VAT", VAR_PG4_SUB_TOTAL_EX_VAT);
            dictionary.Add("FIELD_PG4_PO_NO", FIELD_PG4_PO_NO);
            dictionary.Add("FIELD_PG4_DELIVERY_NOTE_NO", FIELD_PG4_DELIVERY_NOTE_NO);
            dictionary.Add("VAR_PG4_TOTAL_INC_VAT", VAR_PG4_TOTAL_INC_VAT);
            dictionary.Add("FIELD_PG4_FIRST_HOUR", FIELD_PG4_FIRST_HOUR);
            dictionary.Add("FIELD_PG4_SUBSEQUENT_HALF_HOURS", FIELD_PG4_SUBSEQUENT_HALF_HOURS);
            dictionary.Add("VAR_PG4_SUB_TOTAL", VAR_PG4_SUB_TOTAL);
            dictionary.Add("VAR_PG4_VAT", VAR_PG4_VAT);
            dictionary.Add("FIELD_PG4_QUOTE_SUBMIT", FIELD_PG4_QUOTE_SUBMIT);
            dictionary.Add("FIELD_PG4_QUOTE_SUBMIT_DATE", FIELD_PG4_QUOTE_SUBMIT_DATE.ToString());
            dictionary.Add("FIELD_PG4_UPLIFT_APPROVED", FIELD_PG4_UPLIFT_APPROVED);
            dictionary.Add("FIELD_PG4_UPLIFT_DATE", FIELD_PG4_UPLIFT_DATE.ToString());
            dictionary.Add("FIELD_PG4_PARKING", FIELD_PG4_PARKING);
            dictionary.Add("FIELD_PG4_PARKING_TICKET", FIELD_PG4_PARKING_TICKET);
            dictionary.Add("FIELD_PG4_CONGESTION_CHARGE", FIELD_PG4_CONGESTION_CHARGE);
            dictionary.Add("CAM_PG4_CAMERA_05", CAM_PG4_CAMERA_05);
            dictionary.Add("FIELD_PG4_CAMERA_05", FIELD_PG4_CAMERA_05);
            dictionary.Add("CAM_PG4_CAMERA_04", CAM_PG4_CAMERA_04);
            dictionary.Add("FIELD_PG4_CAMERA_04", FIELD_PG4_CAMERA_04);
            dictionary.Add("FIELD_PG4_ROW07_HOURS", FIELD_PG4_ROW07_HOURS);
            dictionary.Add("FIELD_PG4_ROW07_DESC", FIELD_PG4_ROW07_DESC);
            dictionary.Add("FIELD_PG4_ROW06_HOURS", FIELD_PG4_ROW06_HOURS);
            dictionary.Add("FIELD_PG4_ROW06_DESC", FIELD_PG4_ROW06_DESC);
            dictionary.Add("FILED_PG5_CLIENT_NOT_PRESENT", FILED_PG5_CLIENT_NOT_PRESENT);
            dictionary.Add("FIELD_PG5_ENGINEER_SIGNATURE", FIELD_PG5_ENGINEER_SIGNATURE);
            dictionary.Add("FIELD_PG5_ENGINEER_SIGNATURE_DATE", FIELD_PG5_ENGINEER_SIGNATURE_DATE.ToString());
            dictionary.Add("FIELD_PG5_CLIENT_SIGNATURE", FIELD_PG5_CLIENT_SIGNATURE);
            dictionary.Add("FIELD_PG5_CLIENT_SIGNATURE_DATE", FIELD_PG5_CLIENT_SIGNATURE_DATE.ToString());
            dictionary.Add("FIELD_PG5_CLIENT_NAME", FIELD_PG5_CLIENT_NAME);
            dictionary.Add("FIELD_PG5_CLIENT_POSITION", FIELD_PG5_CLIENT_POSITION);
            dictionary.Add("FIELD_PG5_SUBMIT", FIELD_PG5_SUBMIT);
            // Add ancillary items
           
            return dictionary;
        }
    }

    public class TemplateImages
    {
        public long? Sequence { get; set; }
        public bool FixedImage { get; set; }
        public int PageNo { get; set; }
        public string FieldName;
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string FileWWWurl { get; internal set; }
        public bool IsBase64;
        public string Base64File;
        
    }

   
}
