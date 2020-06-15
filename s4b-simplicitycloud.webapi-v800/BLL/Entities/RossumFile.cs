using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class RossumFile 
    {
        public long Sequence { get; set; }
        public string FileName { get; set; }
        public string FileNameCabId { get; set; } // Google drive file id of the original file
        
        public int DocType { get; set; }  // Document type like Invoice, DN & Receipts  0-Not Set, 1 = Invoice, 2 = Delivery Note & 3 Receipts
        public long? RossumQueueId { get; set; } //Rossum Queue Id / Workspace Id to identify the customer and the document type.
        public DateTime? DateDocUploaded { get; set; } //The date and time when the file was uploaded
        public int DocUploadSource { get; set; } //The date and time when the file was uploaded

        // The document and annotation id received just after the uploading of a file
        public int RossumDocumentId { get; set; }
        public int RossumAnnotationId { get; set; }

        //When document status changed to processed
        public DateTime? DateDocProcessed { get; set; }

        //If processed failed due to any reason. and Reason why processing was failed.
        public bool FlgFailed { get; set; }
        public string FileRemarks { get; set; }

        //When document validation completed.
        public DateTime? DateDocValidated { get; set; }

        //When document data imported into our system
        public DateTime? DateDocImported { get; set; }

        //The file name and cab_id stored in G Drive containing the returned JSON text.

        public string RossumFileName { get; set; }  // File name of the extracted json file
        public string RossumCabId { get; set; }  // Google drive file id of the Json file extracted from rossum
        public int CreatedBy { get; set; }  // user id of the creator
        public DateTime DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string SupplierName { get; set; }
        public bool IsLearningMode { get; set; }
        public string DebugData { get; set; }

        // extra columns not available in DB-Table
        public string DocUploadSourceName { get; set; } 
        public string DocTypeDesc { get; set; }
        public string CreatedByName { get; set; }
        public int DocStatusCode { get; set; }
        public string DocStatus { get; set; }
    }

    #region Webhook Classes
    public class RossWebHook
    {
        public string @action { get; set; }
        public string @event { get; set; }
        public RossAnnotation annotation { get; set; }
        public RossDocument document { get; set; }
    }

    public class RossDocument
    { 
        public int id { get; set; }
        public string url { get; set; }
        public string s3_name { get; set; }
        public string mime_type { get; set; }
        public string arrived_at { get; set; }
        public string original_file_name { get; set; }
        public string content { get; set; }
        public ExportedData_Meta metadata { get; set; }
    }

    public class RossAnnotation
    {
        public string document { get; set; }
        public int id{ get; set; }
        public string queue { get; set; }
        public string schema { get; set; }
        public List<string> pages { get; set; }
        public string modifier { get; set; }
        public DateTime? modified_at { get; set; }
        public DateTime? confirmed_at { get; set; }
        public DateTime? exported_at { get; set; }
        public DateTime? assigned_at { get; set; }
        public RossumDocStatus status { get; set; }
        public RossumDocStatus previous_status { get; set; }
        public string rir_poll_id { get; set; }
        public List<string> messages { get; set; }
        public string url { get; set; }
        public string content { get; set; }
        public float time_spent { get; set; }
        public ExportedData_Meta metadata { get; set; }  
    }

    public class RossumAnnotationsPaged
    {
        public RossumPagination pagination { get; set; }
        public List<RossAnnotation> results { get; set; }
    }

    public class RossumPagination
    {
        public int total { get; set; }
        public int total_pages { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
    }
    #endregion

    #region Rossum Schema_invoice
    public class RossSchemaInvoice
    {
        public List<RossSchemaContentSection> content { get; set; }
        public List<RossSchemaContentSection> results { get; set; }
    }
    public class RossSchemaContentSection
    {
        public long id { get; set; }
        public string url { get; set; }
        public string category { get; set; }
        public string schema_id { get; set; }
        //public List<string> validation_sources { get; set; }
        public List<RossSchemaInvoice_ChildLevel_1> children { get; set; }
    }

    public class ExportedData_Meta
    {
        public string document_url { get; set; }
        public string arrived_at { get; set; }
        public string original_file { get; set; }
        public string original_filename { get; set; }
        public string queue_name { get; set; }
        public string workspace_name { get; set; }
        public string organization_name { get; set; }
        public string annotation { get; set; }
        public string queue { get; set; }
        public string workspace { get; set; }
        public string organization { get; set; }
        public string modifier { get; set; }
        public string updated_datapoint_ids { get; set; }
        public string modifier_metadata { get; set; }
        public string queue_metadata { get; set; }
        public string annotation_metadata { get; set; }
        public string rir_poll_id { get; set; }
        public string projectId { get; set; }
        public int UserId { get; set; }

    }


    public class RossSchemaInvoice_ChildLevel_1 : RossSchemaInvoice_Leaf
    {
        public List<RossSchemaInvoice_ChildLevel_2> children { get; set; }
    }

    public class RossSchemaInvoice_ChildLevel_2 : RossSchemaInvoice_Leaf
    {
        public List<RossSchemaInvoice_Leaf> children { get; set; }
    }

    public class RossSchemaInvoice_Leaf
    {
        public int id { get; set; }
        public string category { get; set; }
        public string schema_id { get; set; }
        public double rir_confidence { get; set; }
        public string value { get; set; }
//      public List<string> validation_sources { get; set; }
        public string type { get; set; }
        public leafContent content { get; set; }
    }

    public class leafContent
    { 
        public string value { get; set; }
        public float? rir_confidence { get; set; }
    }
    #endregion

    #region Connector Class
    public class RossumConValidResponse  // Rossum Connector Validate Response
    {
        public List<RossumConValidResMessage> messages {get;set;}
        public List<RossumConValidResOp> operations { get; set; }
        public List<RossumConValidResUpdatedDataPoint> UpdatedDataPoints { get; set; }
    }

    public class RossumConValidResMessage // Rossum Connector Validate Response Message 
    {
        public string content { get; set; }
        public string id { get; set; }
        public string error { get; set; }
    }

    public class RossumConValidResOp  // Rossum Connector Validate Response Operation 
    { 
        public string op { get; set;}
        public string id { get; set; }
        public bool hidden { get; set; }
        public List<string>? options { get; set; }
        public List<string>? validation_sources { get; set; }
        public List<string>? values { get; set; }
    }
    public class RossumConValidResOpValue  // Rossum Connector Validate Response Operation Value
    { 
        public string schema_id { get; set; }
        public RossumConValidResOpValueContent content { get; set; } // Rossum Connector Validate Response Operation Value Content

    }

    public class RossumConValidResOpValueContent  // Rossum Connector Validate Response Operation Value
    {
        public int Page { get; set; }
        public string value { get; set; }   
    }

    public class RossumConValidResUpdatedDataPoint  // Rossum Connector Validate Response Operation Value
    {
        public string id { get; set; }
        public string value { get; set; }
    }
    #endregion

    #region Rossum Enumurations
  
    public enum FileUploadSource
    {
        NOT_SET = 0,
        BROWSER = 1,
        SCANNER = 2
    }

    public enum RossumDocStatus
    {
        NOT_SET = 0,
        IMPORTING = 1, //Document is being processed by the AI Core Engine for data extraction; initial state of the document.
        FAILED_IMPORT = 2,  //Import failed e.g.due to a malformed document file.
        TO_REVIEW = 3, //Initial extraction step is done and the document is waiting for user validation.
        REVIEWING = 4, //Document is undergoing validation in the user interface.
        EXPORTING = 5, //Document is validated and is now awaiting the completion of connector save call.See connector extension for more information on this status.
        EXPORTED = 6, //Document is validated and successfully passed all hooks; this is the typical terminal state of a document.
        FAILED_EXPORT = 7, // When the connector returned an error.
        POSTPONED = 8, // Operator has chosen to postpone the document instead of exporting it.
        DELETED = 9, //When the document was deleted by the user.
        PURGED = 10 //Only metadata was preserved after a deletion.
        /*
         * When uploaded to Rossum => update Date_uploaded
         * When new status = To_Review -> update date_doc_processed
         * When new status = Exported -> update date_doc_validated
         * When create invoice -> update date_doc_imported
         * */
    }
    public enum RossumFileRemarksTypes
    {
        INFO = 0,
        WARNING = 1,
        CRITICAL = 2
    }
    #endregion
}

