using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Database.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .HasMaxLength(255);

        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seed default roles
        builder.HasData(
            new Role { Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"), Name = "Admin", Description = "Administrator" },
            new Role { Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"), Name = "User", Description = "Regular User" }
        );
    }
}
