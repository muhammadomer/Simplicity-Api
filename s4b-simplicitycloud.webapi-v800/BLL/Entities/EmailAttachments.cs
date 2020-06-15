namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class EmailAttachments
    {
        public bool IsBase64 { get; set; }
        public string Base64File { get; set; }
        public string FileName { get; set; }
    }
}