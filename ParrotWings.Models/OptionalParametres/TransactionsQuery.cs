﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Models.OptionalParametres
{
    public class TransactionsQuery
    {
        public string OrderBy { get; set; }
        public Mode? Mode { get; set; }
        public string Party { get; set; }
        public string Search { get; set; }
    }

    public enum Mode
    {
        All,
        Sent,
        Received
    }
}
