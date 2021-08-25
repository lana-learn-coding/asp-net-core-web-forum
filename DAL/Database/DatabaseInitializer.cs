using System;
using System.Collections.Generic;
using System.Data.Entity;
using Bogus;
using DAL.Models.Auth;
using DAL.Models.Topic;

namespace DAL.Database
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ModelContext>
    {
        protected override void Seed(ModelContext context)
        {
            base.Seed(context);
            SeedUsers(context);

            SeedTopic(context);
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
                    FullName = "The Real Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    Email = faker.Person.Email,
                    Phone = faker.Person.Phone,
                    Roles = new List<Role> { admin }
                }
            );

            for (var i = 1; i <= 20; i++)
                context.Users.Add(new User
                    {
                        Avatar = faker.Person.Avatar,
                        Username = faker.Person.UserName + i,
                        FullName = faker.Person.FullName,
                        Password = BCrypt.Net.BCrypt.HashPassword("1"),
                        Email = faker.Person.Email + i,
                        Phone = faker.Person.Phone + i
                    }
                );
        }

        private static void SeedTopic(ModelContext context)
        {
            context.Categories.AddRange(new[]
            {
                new Category { Name = "General Discussion" },
                new Category { Name = "Question & Answer" },
                new Category { Name = "Feedback" },
                new Category { Name = "Uncategorized", Id = Guid.Empty }
            });

            context.Tags.AddRange(new[]
            {
                new Tag { Name = "Neurology" },
                new Tag { Name = "Surgery" },
                new Tag { Name = "Urology" },
                new Tag { Name = "Infectious Diseases" },
                new Tag { Name = "Hematology" },
                new Tag { Name = "Dermatology" },
                new Tag { Name = "Endocrinology & Diabetes" },
                new Tag { Name = "Gastroenterology & Liver Diseases" }
            });
        }
    }
}