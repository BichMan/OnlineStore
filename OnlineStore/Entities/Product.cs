using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        [Column(TypeName = "Money")]
        public decimal ? Price { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public int AttributesId { get; set; }
        [ForeignKey("FK_Cate")]
        public ProductCategory productCategory { set; get; }
        [ForeignKey("FK_Color")]
        public ProductAttribute productAttributes { set; get; }
    }
}
