using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Model;

namespace TinyBank.Core.Services.Options
{
    public class CreateCardOptions
    {
        public Constants.CardType CardType { get; set; }
        public string CardNumber { get; set; }
    }
}
