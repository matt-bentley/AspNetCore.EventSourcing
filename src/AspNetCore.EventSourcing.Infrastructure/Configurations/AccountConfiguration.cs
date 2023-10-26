using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.EventSourcing.Infrastructure.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<AccountReadModel>
    {
        public void Configure(EntityTypeBuilder<AccountReadModel> builder)
        {
            builder.Property(e => e.AccountNumber)
                .HasColumnType("varchar(8)")
                .IsRequired();

            builder.Property(e => e.Balance)
                .HasColumnType("money");

            builder.HasIndex(e => e.AccountNumber)
                .IsUnique();

            builder.HasIndex(e => e.CustomerId);
        }
    }
}
