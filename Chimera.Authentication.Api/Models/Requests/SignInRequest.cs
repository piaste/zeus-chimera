using System.ComponentModel.DataAnnotations;

namespace Chimera.Authentication.Api.Models.Requests
{
    public class SignInRequest
    {
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
