using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuthApi.Infraestructure.Domain;

namespace AuthApi.Infraestructure.Mappings;

public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("USER_ROLE");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.IdUser).HasColumnType("VARCHAR(36)").IsRequired();
        builder.Property(u => u.IdRole).HasColumnType("VARCHAR(36)").IsRequired();

        builder.HasIndex(ur => ur.IdUser);
        builder.HasIndex(ur => ur.IdRole);
    }
}
