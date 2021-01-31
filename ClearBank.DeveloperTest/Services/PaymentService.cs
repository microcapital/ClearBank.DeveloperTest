using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory,
            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _accountDataStore = accountDataStoreFactory.Create();
            _paymentProcessorFactory = paymentProcessorFactory;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            var paymentProcessor = _paymentProcessorFactory.Create(request.PaymentScheme);

            var result = paymentProcessor.Process(request, account);

            if (result.Success)
                _accountDataStore.UpdateAccount(account);
            
            return result;
        }
    }
}
