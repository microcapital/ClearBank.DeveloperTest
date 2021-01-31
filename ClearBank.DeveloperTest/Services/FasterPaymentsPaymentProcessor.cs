using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public class FasterPaymentsPaymentProcessor : IPaymentProcessor
    {
        public MakePaymentResult Process(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult{Success=false};
            if (account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)&& (account.Balance >= request.Amount))
            {
                account.Balance -= request.Amount;
                result.Success = true;
            }
            return result;
        }
    }
}
