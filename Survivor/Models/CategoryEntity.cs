namespace Survivor.Models
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }


        //relationships
        public ICollection<CompetitorEntity> Competitors { get; set; }
    }
}
