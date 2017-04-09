using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Models.Secure
{
    public class BearerToken
    {
        public string Access_token { get; set; }
        public string User_email { get; set; }
    }
}
