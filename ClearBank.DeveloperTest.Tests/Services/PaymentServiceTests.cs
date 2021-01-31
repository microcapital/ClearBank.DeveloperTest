using AutoFixture;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NSubstitute;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private readonly IAccountDataStoreFactory _mockAccountDataStoreFactory;
        private readonly IPaymentProcessorFactory _mockPaymentProcessorFactory;
        public PaymentServiceTests()
        {
            _mockAccountDataStoreFactory = Substitute.For<IAccountDataStoreFactory>();
            _mockPaymentProcessorFactory = Substitute.For<IPaymentProcessorFactory>();
        }

        [Test]
        public void MakePayment_Should_ReturnsMakePaymentResult_Success()
        {
            // Setup 
            var fixture = new Fixture();
            var request = fixture.Create<MakePaymentRequest>();
            var account = fixture.Create<Account>();
            var mockAccountDataStore = Substitute.For<IAccountDataStore>();
            mockAccountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            _mockAccountDataStoreFactory.Create().Returns(mockAccountDataStore);

            var mockPaymentProcessor= Substitute.For<IPaymentProcessor>();
            mockPaymentProcessor.Process(Arg.Any<MakePaymentRequest>(), Arg.Any<Account>()).Returns(new MakePaymentResult { Success = true });
            _mockPaymentProcessorFactory.Create(Arg.Any<PaymentScheme>()).Returns(mockPaymentProcessor);

            var paymentService = new PaymentService(_mockAccountDataStoreFactory, _mockPaymentProcessorFactory);

            // Action
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            mockAccountDataStore.Received(1).UpdateAccount(account); //should call UpdateAccount() once when success!!
        }

        [Test]
        public void MakePayment_Should_ReturnsMakePaymentResult_Failed()
        {
            // Setup 
            var fixture = new Fixture();
            var request = fixture.Create<MakePaymentRequest>();
            var account = fixture.Create<Account>();
            var mockAccountDataStore = Substitute.For<IAccountDataStore>();
            mockAccountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            _mockAccountDataStoreFactory.Create().Returns(mockAccountDataStore);

            var mockPaymentProcessor = Substitute.For<IPaymentProcessor>();
            mockPaymentProcessor.Process(Arg.Any<MakePaymentRequest>(), Arg.Any<Account>()).Returns(new MakePaymentResult { Success = false });
            _mockPaymentProcessorFactory.Create(Arg.Any<PaymentScheme>()).Returns(mockPaymentProcessor);

            var paymentService = new PaymentService(_mockAccountDataStoreFactory, _mockPaymentProcessorFactory);

            // Action
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            mockAccountDataStore.Received(0).UpdateAccount(account); //should not call UpdateAccount() when fails!!

        }

        [Test]
        public void FasterPayments_WithAmountUnderBalance_Should_ReturnsMakePaymentResult_Success()
        {
            // Setup 
            var fixture = new Fixture();
            fixture.Customize<MakePaymentRequest>(o => o
            .With(p => p.Amount, 99M)
            .With(p => p.PaymentScheme, PaymentScheme.FasterPayments));
            var request = fixture.Create<MakePaymentRequest>();

            fixture.Customize<Account>(o => o
            .With(p => p.Balance, 100M)
            .With(p => p.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments));
            var account = fixture.Create<Account>();
            var mockAccountDataStore = Substitute.For<IAccountDataStore>();
            mockAccountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            _mockAccountDataStoreFactory.Create().Returns(mockAccountDataStore);

            _mockPaymentProcessorFactory.Create(PaymentScheme.FasterPayments).Returns(new FasterPaymentsPaymentProcessor());

            var paymentService = new PaymentService(_mockAccountDataStoreFactory, _mockPaymentProcessorFactory);

            // Action
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            mockAccountDataStore.Received(1).UpdateAccount(account); //should call UpdateAccount() once when success!!

        }

        [Test]
        public void FasterPayments_WithAmountOverBalance_Should_ReturnsMakePaymentResult_Failed()
        {
            // Setup 
            var fixture = new Fixture();
            fixture.Customize<MakePaymentRequest>(o => o
            .With(p => p.Amount ,200M)
            .With(p => p.PaymentScheme , PaymentScheme.FasterPayments));
          var request = fixture.Create<MakePaymentRequest>();

            fixture.Customize<Account>(o => o
            .With(p => p.Balance, 100M)
            .With(p => p.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments));
            var account = fixture.Create<Account>();
            var mockAccountDataStore = Substitute.For<IAccountDataStore>();
            mockAccountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            _mockAccountDataStoreFactory.Create().Returns(mockAccountDataStore);

            _mockPaymentProcessorFactory.Create(PaymentScheme.FasterPayments).Returns(new FasterPaymentsPaymentProcessor());

            var paymentService = new PaymentService(_mockAccountDataStoreFactory, _mockPaymentProcessorFactory);

            // Action
            var result = paymentService.MakePayment(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            mockAccountDataStore.Received(0).UpdateAccount(account); //should not call UpdateAccount() when fails!!

        }
    }
}
