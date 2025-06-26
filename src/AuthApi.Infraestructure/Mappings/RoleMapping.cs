using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuthApi.Infraestructure.Domain;

namespace AuthApi.Infraestructure.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("ROLE");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnType("VARCHAR(36)").IsRequired();
        builder.Property(u => u.Name).HasColumnType("VARCHAR(150)").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnType("DATETIME").IsRequired();
    }
}
