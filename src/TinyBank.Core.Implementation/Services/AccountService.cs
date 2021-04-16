using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICustomerService _customers;
        private readonly Data.TinyBankDbContext _dbContext;

        public AccountService(
            ICustomerService customers,
            Data.TinyBankDbContext dbContext)
        {
            _customers = customers;
            _dbContext = dbContext;
        }

        public ApiResult<Account> Create(Guid customerId,
            CreateAccountOptions options)
        {
            if (options == null) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(options)}");
            }

            if (string.IsNullOrWhiteSpace(options.CurrencyCode) ||
              !options.CurrencyCode.Equals(
                  Constants.CurrencyCode.Euro, StringComparison.OrdinalIgnoreCase)) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.BadRequest,
                    $"Invalid or unsupported currency {options.CurrencyCode}");
            }

            var customerResult = _customers.GetById(customerId);

            if (!customerResult.IsSuccessful()) {
                return customerResult.ToResult<Account>();
            }

            var customer = customerResult.Data;

            var account = new Account() {
                AccountId = CreateAccountId(customer.CountryCode),
                Balance = options.Balance,
                CurrencyCode = options.CurrencyCode,
                Customer = customer,
                State = Constants.AccountState.Active,
                Description = options.Description
            };

            _dbContext.Add(account);

            try {
                _dbContext.SaveChanges();
            }
            catch (Exception) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.InternalServerError, "Could not save account");
            }

            return ApiResult<Account>.CreateSuccessful(account);
        }

        public ApiResult<Account> GetById(string accountId)
        {
            if (accountId == null) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(accountId)}");
            }

            // SELECT FROM Account
            IQueryable<Account> accountResultSearch = _dbContext.Set<Account>().AsQueryable().Where(a => a.AccountId == accountId);
            var account = accountResultSearch.Include(c => c.Cards).SingleOrDefault();
            if (account == null) {
                return new ApiResult<Account>() {
                    Code = Constants.ApiResultCode.NotFound,
                    ErrorText = $"Customer {accountId} was not found"
                };
            }
            else {
                return new ApiResult<Account>() {
                    Data = account
                };

            }
        }

        public ApiResult<Account> Charge(string accountId, decimal amount)
        {
            if (accountId == null) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Null {nameof(accountId)}");
            }

            IQueryable<Account> accountResultSearch = _dbContext.Set<Account>().AsQueryable().Where(a => a.AccountId == accountId);
            var account = accountResultSearch.Include(c => c.Cards).SingleOrDefault();
            if (account == null) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.NotFound, $"Account {accountId} was not found");
            }

            if (account.State != Constants.AccountState.Active) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.Success, $"Account State {account.State}");
            }

            if (account.Balance < amount) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.Success, "Ιnsufficient Βalance");
            }

            account.Balance -= amount;
            //_dbContext.Add(account);
            try {
                _dbContext.SaveChanges();
            }
            catch (Exception) {
                return ApiResult<Account>.CreateFailed(
                    Constants.ApiResultCode.InternalServerError, "Could not save account");
            }
            return ApiResult<Account>.CreateSuccessful(account);
        }


        private string CreateAccountId(string countryCode)
        {
            var random = new Random();
            var accountId = random.Next(1000, int.MaxValue).ToString().PadLeft(20, '0');
            var res = countryCode + accountId;
            return res;
        }
    }
}
