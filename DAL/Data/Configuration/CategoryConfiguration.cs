using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DAL
{
    public class CategoryConfiguration : AuditableEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(500)
                   .IsRequired(false);
        }
    }
}
