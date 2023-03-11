using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="*")]
        public string Title { get; set; }
        [Required(ErrorMessage ="*")]
        public string Image { get; set; }
        [Required(ErrorMessage ="*")]
        public string Describtion { get; set; }
        [Required(ErrorMessage ="*")]
        public string Proccessor { get; set; }
        [Required(ErrorMessage ="*")]
        public string Ram { get; set; }
        [Required(ErrorMessage ="*")]
        public string OS { get; set; }
        [Required(ErrorMessage ="*")]
        public string Storage { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(0,999999999,ErrorMessage ="Please Enter Right Price")]
        public double Price { get; set; }
        [Range(0, 100)]
        public double Discount { get; set; } = 0;
        [Required(ErrorMessage ="*")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
