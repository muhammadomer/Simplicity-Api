namespace SimplicityOnlineBLL.Entities
{
    public class ClientRequest
    {
        public int first { get; set; }
        public int rows { get; set; }
        public string sortField { get; set; }
        public int sortOrder { get; set; }
        public string globalFilter { get; set; }
        public object filters { get; set; }
        public string query { get; set; }
    }
}
