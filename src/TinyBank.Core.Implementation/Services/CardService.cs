using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Implementation.Services.Extensions;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class CardService : ICardService
    {
        private readonly IAccountService _accountService;
        private readonly Data.TinyBankDbContext _dbContext;

        public CardService(IAccountService accountService, Data.TinyBankDbContext dbContext)
        {
            _accountService = accountService;
            _dbContext = dbContext;
        }

        public ApiResult<Card> Create(string accountId, CreateCardOptions options)
        {
            if (options == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(options)}");
            }

            var accountResult = _accountService.GetById(accountId);
            if (!accountResult.IsSuccessful()) {
                return accountResult.ToResult<Card>();
            }

            var account = accountResult.Data;

            var card = new Card() {
                CardNumber = options.CardNumber,
                CardType = options.CardType,
                Active = true
            };
            card.Accounts.Add(account);

            _dbContext.Add(card);
            try {
                _dbContext.SaveChanges();
            }
            catch (Exception) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.InternalServerError, "Could not save account");
            }
            return ApiResult<Card>.CreateSuccessful(card);
        }

        //private string CardNumberGenerator()
        //{
        //    Random random = new Random();
        //    string cardNum = string.Empty;
        //    for (int j = 0; j < random.Next(3) + 13; j++) {
        //        cardNum += random.Next(0, 10).ToString();
        //    }
        //    return cardNum;
        //}

        public ApiResult<Card> GetById(Guid? cardId)
        {
            if (cardId == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(cardId)}");
            }

            IQueryable<Card> cardsSearchRes = _dbContext.Set<Card>().AsQueryable().Where(a => a.CardId == cardId);
            var card = cardsSearchRes.SingleOrDefault();
            if (card == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.NotFound, $"CardId {cardId} was not found");
            }
            return new ApiResult<Card>() {
                Data = card
            };
        }

        public ApiResult<Card> GetByCardNumber(string cardNumber)
        {
            if (cardNumber == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(cardNumber)}");
            }

            IQueryable<Card> cardsSearchRes = _dbContext.Set<Card>().AsQueryable().Where(a => a.CardNumber == cardNumber);
            var card = cardsSearchRes.SingleOrDefault();
            if (card == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.NotFound, $"CardNumber {cardNumber} was not found");
            }
            return new ApiResult<Card>() {
                Data = card
            };
        }

        public ApiResult<Card> Checkout(PaymentOptions options)
        {
            if (options == null || !options.IsValid()) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(options)}");
            }

            var card = Get(options.CardNumber);
            var cardValidationsApiResult = card.Validations(options);
            if (cardValidationsApiResult != null) {
                return cardValidationsApiResult;
            }

            var account = card.Accounts.FirstOrDefault();
            var accountValidationsApiResult = account.Validations(options);
            if (accountValidationsApiResult != null) {
                return accountValidationsApiResult;
            }

            decimal amount = options.Amount;
            account.Balance -= amount;
            try {
                _dbContext.SaveChanges();
            }
            catch (Exception) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.InternalServerError, "Could Not Save");
            }
            return ApiResult<Card>.CreateSuccessful(card);
        }

        private Card Get(string cardNumber)
        {
            IQueryable<Card> cardsSearchRes = _dbContext.Set<Card>().AsQueryable().Where(a => a.CardNumber == cardNumber);
            var card = cardsSearchRes
                .Include(c => c.Accounts)
                .SingleOrDefault();
            return card;
        }


    }
}
