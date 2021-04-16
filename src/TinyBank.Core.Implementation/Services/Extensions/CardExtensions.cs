using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services.Extensions
{
    public static class CardExtensions
    {
        public static bool IsActive(this Card value)
        {
            if (value == null) return false;
            return value.Active;
        }

        public static bool IsValidRequest(this Card value, PaymentOptions options)
        {
            if (value == null || options == null) return false;
            if (options.IsValid()) {
                int month = int.Parse(options?.ExpirationMonth);
                int year = int.Parse(options?.ExpirationYear);
                return value.Expiration.Month == month &&
                    value.Expiration.Year == year;
            }
            return false;
        }

        public static bool IsValid(this PaymentOptions options)
        {
            try {
                int month = int.Parse(options?.ExpirationMonth);
                int year = int.Parse(options?.ExpirationYear);
                //decimal amount = decimal.Parse(options?.Amount);
                return true;
            }
            catch {
                return false;
            }

        }


    }
}
