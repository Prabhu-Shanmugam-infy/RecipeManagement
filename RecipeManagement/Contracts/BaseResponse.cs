namespace RecipeManagement.Contracts
{
    public enum Status
    {
        Success,
        Error
    }
    public class BaseResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
    }
}
