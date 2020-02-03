using System.Collections.Generic;

namespace ApiLdapIntegration.Models
{
    public class AuthResult
    {
        public AppUser AppUser { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSucceeded => Errors == null || Errors.Count == 0;
    }
}
