using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public class BacsPaymentProcessor:IPaymentProcessor
    {

        public MakePaymentResult Process(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult { Success = false };
            if (account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                account.Balance -= request.Amount;
                result.Success = true;
            }
            return result;
        }
    }
}
