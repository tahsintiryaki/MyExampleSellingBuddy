using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", OrderDbContext.DEFAULT_SCHEMA);

            builder.HasKey(o => o.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();

            builder.Ignore(i => i.DomainEvents);

            //OwnsOne => addresleri ayrı bir tabloya yazoıp order ile Id üzerinden ilişki kurmak yerine, OwnsOne metodu ile addresing shibi order'dır mantığında addresleri ayrı bir tabloya yazmadan address class'ı ile order tablosu içerisinde tutmayı sağlamaktadır.
            builder
                .OwnsOne(o => o.Address, a =>
                {
                    a.WithOwner();
                });

            // UsePropertyAccessMode ile  backing field özelliği kullanılmış oldu. Order tablosu içerisinde private olarak tanımlanan orderStatusId propertysibir field olarak tanımlandı.
            builder
                .Property<int>("orderStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("OrderStatusId")
                .IsRequired();

            //IReadOnlyCollection olarak tanımlanan OrderItems için kullanıldı.
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);


            builder.HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(i => i.BuyerId);


            builder.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey("orderStatusId");
        }
    }
}
