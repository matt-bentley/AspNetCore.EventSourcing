using AspNetCore.EventSourcing.Core.Accounts.ReadModels;

namespace AspNetCore.EventSourcing.Core.Tests.Builders
{
    public class TransactionBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _accountId = Guid.NewGuid();
        private decimal _amount = 50;
        private decimal _balance = 160;

        public TransactionReadModel Build()
        {
            return new TransactionReadModel()
            {
                AccountId = _accountId,
                Id = _id,
                Amount = _amount,
                Balance = _balance,
                Description = "Test",
                TransationDate = DateTime.UtcNow
            };
        }

        public TransactionBuilder WithAccount(Guid accountId)
        {
            _accountId = accountId;
            return this;
        }
    }
}
