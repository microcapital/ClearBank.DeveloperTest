using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class ChapsPaymentProcessor : IPaymentProcessor
    {
        public MakePaymentResult Process(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult { Success = false };
            if (account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)&& account.Status == AccountStatus.Live)
            {
                account.Balance -= request.Amount;
                result.Success = true;
            }
            return result;
        }
    }
}