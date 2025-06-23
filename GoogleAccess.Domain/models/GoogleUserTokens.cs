using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAccess.Domain.Models
{
    public class GoogleUserTokens
    {
        public User? User { get; set; }
        public string? ClientId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
