using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GymManagement.Data
{
    public static class GymInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new GymContext(
                serviceProvider.GetRequiredService<DbContextOptions<GymContext>>()))
            {
                try
                {
                    if (DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted();
                        if (UseMigrations)
                            context.Database.Migrate();
                        else
                            context.Database.EnsureCreated();
                    }
                    else
                    {
                        if (UseMigrations)
                            context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }

                try
                {
                    if (!context.Clients.Any(c => c.MembershipNumber == 10000))
                    {
                        Client securityClient = new Client()
                        {
                            MembershipNumber = 10000,
                            FirstName = "Sam",
                            LastName = "Client",
                            Phone = "9051234567",
                            Email = "client@outlook.com",
                            DOB = DateTime.Today.AddYears(-30),
                            PostalCode = "L3C 1A1",
                            HealthCondition = "Very healthy",
                            Notes = "Seeded client to match the security login",
                            MembershipStartDate = DateTime.Today,
                            MembershipEndDate = DateTime.Today.AddYears(1),
                            MembershipTypeID = 2,
                            MembershipFee = 199d,
                            FeePaid = true
                        };
                        context.Clients.Add(securityClient);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Client insertion error: " + ex.GetBaseException().Message);
                }
            }
        }
    }
}
