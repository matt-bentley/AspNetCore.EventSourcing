using AspNetCore.EventSourcing.Core.Customers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.EventSourcing.Infrastructure.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.OwnsOne(e => e.Name, childBuilder =>
            {
                childBuilder.Property(e => e.FirstName)
                    .HasColumnName("FirstName")
                    .HasColumnType("varchar(64)")
                    .IsRequired();

                childBuilder.Property(e => e.LastName)
                    .HasColumnName("LastName")
                    .HasColumnType("varchar(64)")
                    .IsRequired();
            });

            builder.OwnsOne(e => e.Email, childBuilder =>
            {
                childBuilder.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasColumnType("varchar(64)")
                    .IsRequired();

                childBuilder.HasIndex(e => e.Value)
                    .IsUnique();
            });
        }
    }
}
