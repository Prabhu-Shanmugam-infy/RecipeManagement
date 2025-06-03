namespace RecipeManagement.Contracts
{
    public class FilterRequest
    {
        public string? Ingredients { get; set; }     
        public int? CookingTimeInMins { get; set; }
        public int? CategoryId { get; set; }
    }
}
