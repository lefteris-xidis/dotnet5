﻿namespace TinyBank.Core.Services.Options
{
    public class CreateAccountOptions
    {
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
    }
}
