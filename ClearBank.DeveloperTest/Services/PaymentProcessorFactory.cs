using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public  class PaymentProcessorFactory: IPaymentProcessorFactory
    {

        private static readonly IDictionary<PaymentScheme, Func<IPaymentProcessor>> Creators =
        new Dictionary<PaymentScheme, Func<IPaymentProcessor>>()
        {
            { PaymentScheme.Bacs, () => new BacsPaymentProcessor() },
            { PaymentScheme.FasterPayments, () => new FasterPaymentsPaymentProcessor() },
            { PaymentScheme.Chaps, () => new ChapsPaymentProcessor() }
        };

        public  IPaymentProcessor Create(PaymentScheme scheme)
        {
            return Creators[scheme]();
        }
    }
}
