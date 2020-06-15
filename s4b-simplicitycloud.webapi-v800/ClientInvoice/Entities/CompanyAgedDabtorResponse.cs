namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class CompanyAgedDabtorResponse
    {
        public long entityId { get; set; }
        public string companyName { get; set; }
        public string foreSurname { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string telephone { get; set; }
        public string telMobile { get; set; }
        public string email { get; set; }
        public decimal? total { get; set; }
        public decimal? days30Amount { get; set; }
        public decimal? days60Amount { get; set; }
        public decimal? days90Amount { get; set; }
        public decimal? olderAmount { get; set; }
    }
}
