using Microsoft.AspNetCore.Identity;

namespace CyberGamify.Models
{
    //Extend IdentityUser so we can store XP and earned badges
    public class ApplicationUser : IdentityUser
    {
        //Total experience points earned by the user
        public int XP { get; set; } = 0;

        //Comma-separated badges (simple for MVP). Could be refactore
        //laterd
        public string Badges { get; set; } = "";
    }

}
