using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Model;

namespace TinyBank.Core.Services
{
    public interface ICardService
    {
        public ApiResult<Card> Create(string accountId, Options.CreateCardOptions options);

        public ApiResult<Card> GetById(Guid? cardId);

        public ApiResult<Card> GetByCardNumber(string cardNumber);

        public ApiResult<Card> Checkout(Options.PaymentOptions options);

    }
}
