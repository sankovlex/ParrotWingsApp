using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Models.OptionalParametres
{
    public class Paging
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }
}
