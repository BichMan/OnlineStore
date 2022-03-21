using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
