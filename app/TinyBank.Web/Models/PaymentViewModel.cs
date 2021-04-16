using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Web.Models
{
    public class PaymentViewModel
    {
        public PaymentOptions PaymentOptions { get; set; }

        public PaymentViewModel()
        {
            PaymentOptions = new PaymentOptions();
        }
    }
}
