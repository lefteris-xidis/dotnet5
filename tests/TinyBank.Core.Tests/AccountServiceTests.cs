using System;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class AccountServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly IAccountService _accounts;
        private readonly ICustomerService _customers;
        private readonly CustomerServiceTests _customerTests;

        public AccountServiceTests(TinyBankFixture fixture)
        {
            _accounts = fixture.GetService<IAccountService>();
            _customers = fixture.GetService<ICustomerService>();
            _customerTests = new CustomerServiceTests(fixture);
        }

        [Fact]
        public void CreateAccount_Success()
        {
            var customer = _customerTests.RegisterCustomer_Success(
                Constants.Country.GreekCountryCode);

            Assert.NotNull(customer);

            var accountOptions = new CreateAccountOptions() {
                CurrencyCode = Constants.CurrencyCode.Euro,
                Description = "My test account"
            };

            var accountResult = _accounts.Create(
                customer.CustomerId, accountOptions);
            Assert.True(accountResult.IsSuccessful());

            var account = accountResult.Data;
            Assert.StartsWith(customer.CountryCode, account.AccountId);
            Assert.Equal(customer.CustomerId, account.CustomerId);
            Assert.Equal(0M, account.Balance);
            Assert.Equal(Constants.AccountState.Active, account.State);
        }

        [Fact]
        public Account CreateNewAccount()
        {
            var customer = _customerTests.RegisterCustomer_Success(
                Constants.Country.GreekCountryCode);

            Assert.NotNull(customer);

            var accountOptions = new CreateAccountOptions() {
                CurrencyCode = Constants.CurrencyCode.Euro,
                Description = "LefAccount",
            };

            var accountResult = _accounts.Create(
                customer.CustomerId, accountOptions);
            Assert.True(accountResult.IsSuccessful());

            var account = accountResult.Data;
            Assert.StartsWith(customer.CountryCode, account.AccountId);
            Assert.Equal(customer.CustomerId, account.CustomerId);
            Assert.Equal(0M, account.Balance);
            Assert.Equal(Constants.AccountState.Active, account.State);
            return account;
        }

        [Fact]
        public Account CreateNewAccountOnCustomer()
        {
            //my customer
            Guid customerId = Guid.Parse("0c4e8659-8573-4df2-b804-811376553a86");
            var customerResult = _customers.GetById(customerId);
            var customer = customerResult?.Data;
            Assert.NotNull(customer);

            var accountOptions = new CreateAccountOptions() {
                CurrencyCode = Constants.CurrencyCode.Euro,
                Description = "SavingAccount",
                Balance = 10000
            };

            var accountResult = _accounts.Create(
                customer.CustomerId, accountOptions);
            Assert.True(accountResult.IsSuccessful());

            var account = accountResult.Data;
            Assert.StartsWith(customer.CountryCode, account.AccountId);
            Assert.Equal(customer.CustomerId, account.CustomerId);
            //Assert.Equal(0M, account.Balance);
            Assert.Equal(Constants.AccountState.Active, account.State);
            return account;
        }

        [Fact]
        public void ChargeAccount()
        {
            string accountId = "GR00000000001492052809";
            decimal amount = 100;
            var chargeResult = _accounts.Charge(accountId, amount);
            Assert.True(chargeResult.IsSuccessful());
        }

    }
}
