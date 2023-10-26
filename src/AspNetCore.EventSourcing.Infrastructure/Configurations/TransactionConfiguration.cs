using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.EventSourcing.Infrastructure.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<TransactionReadModel>
    {
        public void Configure(EntityTypeBuilder<TransactionReadModel> builder)
        {
            builder.Property(e => e.Description)
                .HasColumnType("varchar(256)");

            builder.Property(e => e.Balance)
                .HasColumnType("money");

            builder.Property(e => e.Amount)
                .HasColumnType("money");

            builder.HasIndex(e => e.AccountId);
        }
    }
}
