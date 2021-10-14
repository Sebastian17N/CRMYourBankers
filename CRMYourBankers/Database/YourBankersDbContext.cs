using CRMYourBankers.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CRMYourBankers.Database
{
    public class YourBankersDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=YourBankersConnection.sqlite");
        }

        public void DataSeeds()
        {
            this.Database
            if (!Clients.Any())
            {
                Clients.AddRange(
                    new List<Client>
                    {
                        new Client
                        {
                            Id = 1,
                            FirstName = "Piotr",
                            LastName ="Zieliński",
                            PhoneNumber = 888777999,
                            Email = "zielinski@wp.pl",
                            PersonalId = 12121212345
                        },
                        new Client
                        {
                            Id = 2,
                            FirstName = "Jan",
                            LastName ="Kowalski",
                            PhoneNumber = 555444666,
                            Email = "kowalski@onet.pl",
                            PersonalId = 55443312345
                        }
                    }
                );
            }
        }
    }
}
