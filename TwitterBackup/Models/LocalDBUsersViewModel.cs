using System.Collections.Generic;

namespace TwitterBackup.Models
{
    public class LocalDBUsersViewModel
    {
        public IEnumerable<LocalDBUserDetailsViewModel> Users { get; set; }
    }
}
