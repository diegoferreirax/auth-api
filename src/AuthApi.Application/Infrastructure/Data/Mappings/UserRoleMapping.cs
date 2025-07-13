using AuthApi.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Application.Infrastructure.Data.Mappings;

public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("USER_ROLE");

        builder.Property(u => u.IdUser).IsRequired();
        builder.Property(u => u.IdRole).IsRequired();
    }
}
