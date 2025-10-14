using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuthApi.Infraestructure.Domain;

namespace AuthApi.Infraestructure.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USER");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Id).HasColumnType("VARCHAR(36)").IsRequired();
        builder.Property(u => u.Name).HasColumnType("VARCHAR(150)").IsRequired();
        builder.Property(u => u.Email).HasColumnType("VARCHAR(150)").IsRequired();
        builder.Property(u => u.Hash).HasColumnType("VARCHAR(100)").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnType("DATETIME").IsRequired();

        builder.HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey("IdUser")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
