using AuthApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Application.Persistence.Context.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("ROLE");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnType("VARCHAR(36)").IsRequired();
        builder.Property(u => u.Name).HasColumnType("VARCHAR(150)").IsRequired();
        builder.Property(u => u.Code).HasColumnType("VARCHAR(10)").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnType("DATETIME").IsRequired();
    }
}
