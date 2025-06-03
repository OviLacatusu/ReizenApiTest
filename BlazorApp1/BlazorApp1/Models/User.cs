using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Models
{
    public class User : IdentityUser
    {
        public string Email
        {
            get;set;
        }
    }
}
