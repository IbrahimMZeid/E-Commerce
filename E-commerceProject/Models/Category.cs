using System.ComponentModel.DataAnnotations;

namespace E_commerceProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
        [Required(ErrorMessage ="*")]
        public string Describtion { get; set; }
        public List<Product> Products { get; set; }
    }
}
