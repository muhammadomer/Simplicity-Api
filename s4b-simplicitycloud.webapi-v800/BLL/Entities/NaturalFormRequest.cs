namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class NaturalFormRequest
    {
        public NaturalForm Form { get; set; }
        public long? AppSequence { get; set; }
        public long? AppWebAssignSequence { get; set; }
        public bool IsThirdParty { get; set; }
        public long? SupplierId { get; set; }
        public long? AssetSequence { get; set; }
    }
}
