namespace Survivor.Models
{
    public class CompetitorEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CategoryId { get; set; }

        // relationships
        public CategoryEntity Category { get; set; }
    }
}
