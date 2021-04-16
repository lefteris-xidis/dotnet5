using Microsoft.EntityFrameworkCore;

using System.Linq;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using Xunit;

namespace TinyBank.Core.Tests
{
    public class CardTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly ICardService _cardService;
        private readonly IAccountService _accountService;
        private readonly CustomerServiceTests _customerTests;
        private readonly AccountServiceTests _accountTests;


        public CardTests(TinyBankFixture fixture)
        {
            _cardService = fixture.GetService<ICardService>();
            _accountService = fixture.GetService<IAccountService>();
            _customerTests = new CustomerServiceTests(fixture);
            _accountTests = new AccountServiceTests(fixture);
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public void Checkout()
        {
            PaymentOptions options = new PaymentOptions() {
                CardNumber = "4917910043228546",
                ExpirationMonth = "4",
                ExpirationYear = "2027",
                Amount = 100
            };
            var cardResult = _cardService.Checkout(options);
            Assert.True(cardResult.IsSuccessful());
        }


        [Fact]
        public void GetByCardNumber()
        {
            string cardNumber = "4917910043228546";
            var cardResult = _cardService.GetByCardNumber(cardNumber);
            Assert.True(cardResult.IsSuccessful());
        }

        [Fact]
        public void CreateCardOnAccount()
        {
            string accountId = "GR00000000000688051010";
            var accountResult = _accountService.GetById(accountId);
            var account = accountResult?.Data;
            Assert.NotNull(account);

            var cardOptions = new CreateCardOptions() {
                CardType = Constants.CardType.Debit,
                CardNumber = "4917910043228546"
            };
            var cardResult = _cardService.Create(account.AccountId, cardOptions);
            Assert.True(cardResult.IsSuccessful());

        }


    }
}
