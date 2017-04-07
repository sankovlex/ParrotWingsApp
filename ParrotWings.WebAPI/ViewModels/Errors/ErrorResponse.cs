using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.ViewModels.Errors
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
