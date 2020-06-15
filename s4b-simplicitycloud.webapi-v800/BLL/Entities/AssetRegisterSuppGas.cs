using System;

namespace SimplicityOnlineBLL.Entities
{
    public class AssetRegisterSuppGas
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? EntityId { get; set; }
        public string AssetGasType { get; set; }
        public bool FlgGasFixing { get; set; }
        public string GasFixing { get; set; }
        public bool FlgGasType { get; set; }
        public string GasType { get; set; }
        public bool FlgGasFuel { get; set; }
        public string GasFuel { get; set; }
        public bool FlgGasEfficiency { get; set; }
        public string GasEfficiency { get; set; }
        public bool FlgGasFlueType { get; set; }
        public string GasFlueType { get; set; }
        public bool FlgGasFlueing { get; set; }
        public string GasFlueing { get; set; }
        public bool FlgGasOvUvSs { get; set; }
        public string GasOvUvSs { get; set; }
        public bool FlgGasExpansionVessel { get; set; }
        public bool FlgGasExpansion { get; set; }
        public string GasExpansion { get; set; }
        public bool FlgGasImmersion { get; set; }
        public string GasImmersion { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}