using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Bogus;
using DAL.Models;
using DAL.Models.Auth;
using DAL.Models.Forum;
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

            SeedForums(context);
            context.SaveChanges();
        }

        private static void SeedUsers(ModelContext context)
        {
            var admin = new Role { Name = "Admin" };

            context.Roles.AddRange(new[]
            {
                admin
            });

            var faker = new Faker();
            context.Users.Add(new User
                {
                    Avatar = faker.Person.Avatar,
                    Username = "Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    Email = "admin@admin.com",
                    Roles = new List<Role> { admin }
                }
            );
            context.Users.Add(new User
                {
                    Avatar = faker.Person.Avatar,
                    Username = "Test",
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    Email = "test@test.com"
                }
            );

            for (var i = 1; i <= 20; i++)
            {
                faker = new Faker();
                context.Users.Add(new User
                    {
                        Avatar = faker.Person.Avatar,
                        Username = faker.Person.UserName + i,
                        Password = BCrypt.Net.BCrypt.HashPassword("1"),
                        Email = faker.Person.Email + i,
                    }
                );
            }
        }

        private static void SeedTopic(ModelContext context)
        {
            context.Categories.AddRange(new[]
            {
                new Category { Name = "General Discussion", Priority = (short)Priority.High },
                new Category { Name = "Question & Answer", Priority = (short)Priority.High },
                new Category
                {
                    Name = "Announcements & FeedBack",
                    Description = "All major announcements and news can be found in here. Keep up to date with regards to the development of the doctors community!",
                    Priority = (short)Priority.VeryHigh
                },
                new Category
                {
                    Name = "Uncategorized",
                    Description = "Uncategorized forums",
                    Id = Guid.Empty, Priority = (short)Priority.Low
                }
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

        private static void SeedForums(ModelContext context)
        {
            var categories = context.Categories.ToList();
            context.Forums.Add(new Forum
                {
                    Id = Guid.Empty,
                    Title = "Deleted",
                    SubTitle = "Forums contains thread of all deleted forums",
                    Priority = (short)Priority.Low,
                    CategoryId = Guid.Empty,
                    ForumAccess = AccessMode.Private,
                    ThreadAccess = AccessMode.Private
                }
            );
            for (var i = 0; i < 30; i++)
            {
                var faker = new Faker();
                context.Forums.Add(new Forum
                    {
                        Title = $"{faker.Company.CatchPhrase()} {i}",
                        SubTitle = $"{faker.Name.JobDescriptor()} {faker.Name.JobTitle()}",
                        Description = faker.Lorem.Sentence(10, 10),
                        Priority = faker.Random.Short((short)Priority.Normal, (short)Priority.VeryHigh),
                        CategoryId = categories[faker.Random.Int(0, 3)].Id,
                        ForumAccess = AccessMode.Public,
                        ThreadAccess = AccessMode.Public
                    }
                );
            }
        }
    }
}