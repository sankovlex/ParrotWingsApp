using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Models.Secure
{
    public class BearerToken
    {
        public string Access_Token { get; set; }
        public string User_Email { get; set; }
    }
}
