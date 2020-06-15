namespace SimplicityOnlineBLL.Entities
{
    public class DashboardViewOrderByClientAndYear
    {
			public string Client { get; set; }
			public int NoOfOrders { get; set; }
	}

    public class DashboardViewInvoiceTotalByClient
    {
        public string Client { get; set; }
        public double InvoiceTotal { get; set; }
    }

    public class DashboardViewOrdersCountByQuarterAndType
    {
        public string RecordType { get; set; }
        public string Quarter { get; set; }
        public int RecordCount { get; set; }
    }
}