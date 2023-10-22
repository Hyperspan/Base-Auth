using Hyperspan.Auth.Domain.Context;
using Hyperspan.Settings.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hyperspan.Base.Database
{
    public class Contexts : AuthContext<Guid>
    {
        public Contexts(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SettingsMaster>(entity =>
            {
                entity.ToTable("Masters", schema: "Settings");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.SettingLabel);
                entity.HasOne(x => x.Parent)
                    .WithMany()
                    .HasForeignKey(x => x.ParentId);
            });
        }
    }

}
