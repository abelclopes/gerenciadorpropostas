using System.ComponentModel.DataAnnotations;
using Model;

namespace Model
{
  public class LoginModel
  {
      [Required]
      public string Email { get; set; }
      [Required]
      public string Password { get; set; }
  }
}