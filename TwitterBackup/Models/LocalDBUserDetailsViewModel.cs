using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TwitterBackup.Models
{
    public class LocalDBUserDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        public IFormFile ImageFile { get; set; }

        public byte[] Avatar { get; set; }
    }
}
