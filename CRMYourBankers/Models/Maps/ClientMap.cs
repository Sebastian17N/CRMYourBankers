using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMYourBankers.Models.Maps
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            ToTable("Client");

            Property(p => p.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(200);

            Property(p => p.PhoneNumber)
                .IsOptional();

            Property(p => p.Email)
                .IsOptional()
                .HasMaxLength(200);

            Property(p => p.PersonalId)
                .IsRequired();
        }
    }
}
