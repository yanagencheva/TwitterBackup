using System.ComponentModel.DataAnnotations;

namespace TwitterBackup.Models
{
    public class TwitterUserViewModel
    {
        public TwitterUserViewModel()
        {
            IsIncluded = false;
        }
        public string Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Include in backup database")]
        public bool IsIncluded { get; set; }
    }
}
