using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceProject.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Enter Right Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="*")]
        [RegularExpression("^01[0125][0-9]{8}", ErrorMessage ="Enter Valid Phone Number")]
        public String Phone { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "at least 8 characters")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Not matched")]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "at least 8 characters")]
        public string OldPass { get; set; }

        [Required(ErrorMessage ="*")]
        public string Address { get; set; }
        [Required(ErrorMessage ="*")]
        public int Age { get; set; }
        [NotMapped]
        public string ErrorMsg { get; set; } = "";
        public string? ProfileImage { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
