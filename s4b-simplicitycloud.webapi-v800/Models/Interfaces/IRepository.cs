namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRepository
    {
        string Message { get; set; }
        bool IsSecondaryDatabase { get; set; }
        string SecondaryDatabaseId { get; set; }
    }
}
