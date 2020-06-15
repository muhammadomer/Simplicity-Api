namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class ListResponse<T>
    {
        public ListResponse()
        {
        }

        public ListResponse(T[] _data, int _totalRecords, int _currenPage, int _pageSize, int _currenPageRecords)
        {
            data = _data;
            totalRecords = _totalRecords;
            currenPage = _currenPage;
            pageSize = _pageSize;
            currenPageRecords = (_currenPageRecords < 0) ? 0 : _currenPageRecords;
        }
        
        public T[] data { get; set; }
        public int totalRecords { get; set; }
        public int currenPage { get; set; }
        public int pageSize { get; set; }
        public int currenPageRecords { get; set; }
    }
}
