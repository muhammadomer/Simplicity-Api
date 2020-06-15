namespace SimplicityOnlineBLL.Entities
{
    public class RefDiaryAppTypes
    {

        public long? AppTypeCode { get; set; }
        public long? AppTypeSequence { get; set; }
        public string AppTypeDesc { get; set; }
        public string AppTypeIconPath { get; set; }
        public bool FlgAppTypeAccounts { get; set; }
        public bool FlgAppTypeCertificate { get; set; }
        public long? CertificateSequence { get; set; }
        public bool FlgAppTypeSim { get; set; }
        public long? SimIndex { get; set; }
        public bool FlgAppTypeSimDueVisit { get; set; }
        public bool FlgAppTypeSimRevisit { get; set; }
        public bool FlgAppTypeVeh { get; set; }
        public long? VehIndex { get; set; }
        public bool FlgAppTypeVehDueVisit { get; set; }
        public bool FlgAppTypeVehDueRevisit { get; set; }
        public bool FlgAppTypeVehDueSer { get; set; }
        public bool FlgAppTypeVehDuePmi { get; set; }
        public bool FlgAppTypeVehDueMot { get; set; }
        public bool FlgAppTypeVehDueIns { get; set; }
        public bool FlgAppTypeMnt { get; set; }
        public bool FlgAppTypeHoliday { get; set; }
        public bool FlgAppTypeTool { get; set; }
        public long? ToolIndex { get; set; }
        public bool FlgAppTypeToolDueVisit { get; set; }
        public bool FlgAppTypePlant { get; set; }
        public long? PlantIndex { get; set; }
        public bool FlgAppTypePlantDueVisit { get; set; }
        public bool FlgAppTypePlantDueRevisit { get; set; }
        public bool FlgAppTypePlantDueSer { get; set; }

    }
}