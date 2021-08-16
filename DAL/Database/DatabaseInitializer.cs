using System.Collections.Generic;
using System.Data.Entity;
using Bogus;
using DAL.Models.Auth;


namespace DAL.Database
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ModelContext>
    {
        protected override void Seed(ModelContext context)
        {
            base.Seed(context);
            SeedUsers(context);
            context.SaveChanges();
        }

        private static void SeedUsers(ModelContext context)
        {
            var admin = new Role { Name = "Admin" };

            context.Roles.AddRange(new[]
            {
                admin,
                new Role { Name = "Moderator" },
                new Role { Name = "User" }
            });

            var faker = new Faker();
            context.Users.Add(new User
                {
                    Avatar = faker.Person.Avatar,
                    Username = "Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    Email = faker.Person.Email,
                    Phone = faker.Person.Phone,
                    Roles = new List<Role> { admin }
                }
            );

            for (var i = 1; i <= 20; i++)
            {
                context.Users.Add(new User
                    {
                        Avatar = faker.Person.Avatar,
                        Username = faker.Person.UserName + i,
                        Password = BCrypt.Net.BCrypt.HashPassword("1"),
                        Email = faker.Person.Email + i,
                        Phone = faker.Person.Phone + i
                    }
                );
            }
        }
    }
}