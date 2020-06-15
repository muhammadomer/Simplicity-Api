namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{    
    public class ClientPageListRequest
    { 
        public int page { get; set; }
        
        public int size { get; set; }
        
        public SortOrders? sortOrder { get; set; }
        
        public string sortField { get; set; }
        
        public string search { get; set; }
    }
}