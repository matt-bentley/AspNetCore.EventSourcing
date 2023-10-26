using AspNetCore.EventSourcing.Core.Accounts.Entities;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;

namespace AspNetCore.EventSourcing.Core.Tests.Builders
{
    public class AccountBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _accountNumber = "12345678";
        private Guid _customerId = Guid.NewGuid();

        public Account Build()
        {
            return Account.Create(_id, _accountNumber, _customerId);
        }

        public AccountReadModel BuildReadModel()
        {
            return new AccountReadModel()
            {
                Id = _id,
                AccountNumber = _accountNumber,
                CustomerId = _customerId,
                Balance = 0,
                OpenedDate = DateTime.UtcNow,
            };
        }

        public AccountBuilder WithAccountNumber(string accountNumber)
        {
            _accountNumber = accountNumber;
            return this;
        }

        public AccountBuilder WithCustomer(Guid customerId)
        {
            _customerId = customerId;
            return this;
        }
    }
}
