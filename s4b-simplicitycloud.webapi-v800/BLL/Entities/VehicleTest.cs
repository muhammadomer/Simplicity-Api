using SimplicityOnlineWebApi.BLL.Entities;
using System;

namespace SimplicityOnlineBLL.Entities
{
    public class VehicleTestH
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? AssetSequence { get; set; }
        public long? TypeSequence { get; set; }
        public DateTime? DateTest { get; set; }
        public bool FlgLocked { get; set; }
        public bool FlgComplete { get; set; }
        public long? TestPassOrFail { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class VehicleTestI
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? AssetSequence { get; set; }
        public long? SectionId { get; set; }
        public long? RowId { get; set; }
        public long? InputData { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class VehicleTestI2
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? AssetSequence { get; set; }
        public long? RowIndex { get; set; }
        public string FaultDesc { get; set; }
        public string IMNumber { get; set; }
        public String FixFaultDesc { get; set; }
        public string DoneBy { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
    public class VehicleTestI3
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? AssetSequence { get; set; }
        public long? BrakeTest { get; set; }
        public bool FlgTapleyTest { get; set; }
        public bool FlgRollerBrakeTest { get; set; }
        public int LadenType { get; set; }
        public int RoadCoditionType { get; set; }
        public double BreakTestMain { get; set; }
        public double BreakTestSecondary { get; set; }
        public double BreakTestParking { get; set; }
        public double BreakTestSpeed { get; set; }
        public int BreakTestSpeedType { get; set; }
        public bool FlgRollerBbrakeTest { get; set; }
        public String DrawingRegNo { get; set; }
        public long? TestMileage { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }



}