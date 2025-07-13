using AuthApi.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Application.Persistence.Data.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("ROLE");
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Name).IsRequired();
        builder.Property(u => u.Code).IsRequired();
        builder.Property(u => u.UpdatedDate).IsRequired();
    }
}
