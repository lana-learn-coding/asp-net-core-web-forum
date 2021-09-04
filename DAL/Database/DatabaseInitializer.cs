using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            SeedTopic(context);
            SeedUsers(context);
            SeedForums(context);
            context.SaveChanges();
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
                    Description =
                        "All major announcements and news can be found in here. Keep up to date with regards to the development of the doctors community!",
                    Priority = (short)Priority.VeryHigh
                },
                // this forum act as default forum mapping when deleted
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
            // this user act as default user mapping when deleted
            context.Users.Add(new User
                {
                    Id = Guid.Empty,
                    Avatar = faker.Person.Avatar,
                    Username = "Anon",
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    Email = "anon@anon.com"
                }
            );
        }

        private static void SeedForums(ModelContext context)
        {
            // this forum act as default forum mapping when deleted
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
        }
    }
}