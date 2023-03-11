using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceProject.Models
{
    public class Cart
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public DateTime Time { get; set; }
        public string Stats { get; set; }
        public double Price { get; set; }
        public int Number { get; set; } = 1;
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
