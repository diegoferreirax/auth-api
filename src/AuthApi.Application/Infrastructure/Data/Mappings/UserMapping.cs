using AuthApi.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Application.Infrastructure.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USER");
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Name).IsRequired();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.Hash).IsRequired();
        builder.Property(u => u.UpdatedDate).IsRequired();

        builder.HasMany(u => u.Role)
            .WithOne()
            .HasForeignKey("UserId")
            .IsRequired(false);
    }
}
