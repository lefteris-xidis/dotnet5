using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services.Extensions
{
    public static class Extensions
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
                return true;
            }
            catch {
                return false;
            }
        }

        public static ApiResult<Card> Validations(this Card card, PaymentOptions options)
        {
            if (card == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.NotFound, $"Card {options.CardNumber} was not found");
            }

            if (!card.IsValidRequest(options)) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Bad Request");
            }

            if (!card.IsActive()) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"InActive Card {options.CardNumber}");
            }
            return null;

        }

        public static ApiResult<Card> Validations(this Account account, PaymentOptions options)
        {
            if (account == null) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, "No Connected Account");
            }

            if (account.State != Constants.AccountState.Active) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, $"Account State {account.State}");
            }

            decimal amount = options.Amount;
            if (account.Balance < amount) {
                return ApiResult<Card>.CreateFailed(
                    Constants.ApiResultCode.BadRequest, "Ιnsufficient Βalance");
            }
            return null;

        }

    }
}
