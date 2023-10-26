using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using AspNetCore.EventSourcing.Core.Tests.Builders;

namespace AspNetCore.EventSourcing.Core.Tests.Accounts.Entities
{
    public class AccountTests
    {
        [Fact]
        public void GivenAccount_WhenOpen_ThenIsNotClosed()
        {
            var account = new AccountBuilder().Build();
            account.Closed.Should().BeFalse();
            account.Balance.Should().Be(0);
        }

        [Fact]
        public void GivenAccount_WhenInvalidAccountNumber_ThenError()
        {
            Action action = () => new AccountBuilder().WithAccountNumber("").Build();
            action.Should().Throw<DomainException>().WithMessage("Required input 'AccountNumber' is missing.");
        }

        [Fact]
        public void GivenAccount_WhenDeposit_ThenAddedToBalance()
        {
            var amount = 100.50m;
            var account = new AccountBuilder().Build();
            account.Deposit(amount, "Deposited");
            account.Balance.Should().Be(amount);
        }

        [Fact]
        public void GivenAccount_WhenWithdraw_ThenRemovedFromBalance()
        {
            var amount = 100.50m;
            var account = new AccountBuilder().Build();
            account.Deposit(amount, "Deposited");
            account.Withdraw(amount, "Withdrawn");
            account.Balance.Should().Be(0);
        }

        [Fact]
        public void GivenAccount_WhenWithdrawWithoutFunds_ThenError()
        {
            var amount = 100.50m;
            var account = new AccountBuilder().Build();
            Action action = () => account.Withdraw(amount, "Deposited");
            action.Should().Throw<DomainException>().WithMessage("Insufficient funds for withdraw");
        }

        [Fact]
        public void GivenAccount_WhenClose_ThenClosed()
        {
            var account = new AccountBuilder().Build();
            account.Close();
            account.Closed.Should().BeTrue();
        }

        [Fact]
        public void GivenAccount_WhenDepositWhenClosed_ThenError()
        {
            var account = new AccountBuilder().Build();
            account.Close();
            Action action = () => account.Deposit(100m, "Deposited");
            action.Should().Throw<DomainException>().WithMessage("Account is closed");
        }
    }
}
