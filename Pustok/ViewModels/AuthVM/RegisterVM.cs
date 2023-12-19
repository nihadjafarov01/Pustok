using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.AuthVM
{
    public class RegisterVM
    {
		[Required(ErrorMessage = "Enter valid Fullname"), MaxLength(36)]
		public string Fullname { get; set; }
		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required(ErrorMessage = "Enter valid Username"), MaxLength(24)]
		public string Username { get; set; }
		[Required, DataType(DataType.Password), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string Password { get; set; }
		[Required, DataType(DataType.Password), Compare(nameof(Password)), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string ConfirmPassword { get; set; }

	}
}
