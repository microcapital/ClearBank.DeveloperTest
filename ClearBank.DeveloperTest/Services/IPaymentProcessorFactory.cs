using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor Create(PaymentScheme scheme);
    }
}
