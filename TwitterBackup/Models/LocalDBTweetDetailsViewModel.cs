using System.ComponentModel.DataAnnotations;

namespace TwitterBackup.Models
{
    public class LocalDBTweetDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Text")]
        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; }

        public bool Favourite { get; set; }

        public string BrowsingTweetId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}
