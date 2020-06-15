namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class NamedModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public NamedModel()
        {
        }

        public NamedModel(string _name, int _id)
        {
            name = _name;
            id = _id;
        }
    }
}
