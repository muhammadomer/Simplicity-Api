using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class RossumDocumentType
    {
        public int DocType { get; set; }
        public string DocTypeKey { get; set; }
        public string DocTypeDesc { get; set; }
        public int DocTypeQueueId { get; set; }
        public string DocTypeFolderName { get; set; }
        public string DocTypeFolderCabId { get; set; } 
        public string ReceivedFolderName { get; set; }
        public string ReceivedFolderCabId { get; set; }
        public string InReviewFolderName { get; set; }
        public string InReviewFolderCabId { get; set; }
        public string SuccessFolderName { get; set; }
        public string SuccessFolderCabId { get; set; }
        public string FailedFolderName { get; set; }
        public string FailedFolderCabId { get; set; }
    }
}

