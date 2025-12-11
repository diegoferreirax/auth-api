using AuthApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Application.Persistence.Context.Mappings;

public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("USER_ROLE");

        builder.HasAlternateKey(ur => new { ur.IdUser, ur.IdRole });

        builder.Property(u => u.IdUser).IsRequired();
        builder.Property(u => u.IdRole).IsRequired();
    }
}
