namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class ConfigModel
    {
        public string linkedtable { get; set; }
        public string compname { get; set; }
        public string mailfrom { get; set; }
        public string mailfromname { get; set; }
        public int e_ssl { get; set; }
        public int e_auth { get; set; }
        public string e_username { get; set; }
        public string e_password { get; set; }
        public string e_server { get; set; }
        public string bottomleft { get; set; }
        public int monthfilt { get; set; }
        public string appn { get; set; }
        public string appnpl { get; set; }
        public string appn_abbr { get; set; }
        public string invaddr { get; set; }
        public bool isEstimateTabDisplay { get; set; }
    }
}
