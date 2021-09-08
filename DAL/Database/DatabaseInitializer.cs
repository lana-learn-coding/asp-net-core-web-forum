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
            SeedRegion(context);
            SeedExperience(context);
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
                    Priority = (short)Priority.Normal
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
                new Tag { Name = "Hematology" },
                new Tag { Name = "Dermatology" },
                new Tag { Name = "Anesthesiology" },
            });

            foreach (var tag in context.Tags.Local)
            {
                context.Specialties.Add(new Specialty { Tag = tag });
            }

            context.Tags.AddRange(new[]
            {
                new Tag { Name = "Infectious Diseases" },
                new Tag { Name = "Gastroenterology & Liver Diseases" },
                new Tag { Name = "Endocrinology & Diabetes" }
            });

            context.Languages.AddRange(new[]
            {
                new Language { Name = "English", Id = Guid.Empty },
                new Language { Name = "Vietnamese" },
                new Language { Name = "German" }
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
                    Title = "Legacy Forum Threads",
                    SubTitle = "Forums contains thread of all deleted forums",
                    Priority = (short)Priority.Low,
                    CategoryId = Guid.Empty,
                    ForumAccess = AccessMode.Private,
                    ThreadAccess = AccessMode.Private
                }
            );
        }


        private static void SeedRegion(ModelContext context)
        {
            var vietnam = new Country { Name = "Vietnam" };
            var germany = new Country { Name = "germany" };
            context.Countries.AddRange(new[] { vietnam, germany });
            context.Cities.AddRange(new[]
            {
                new City { Name = "Da Lat", Country = vietnam },
                new City { Name = "Ha Noi", Country = vietnam },
                new City { Name = "Da Nang", Country = vietnam },
                new City { Name = "Ho Chi Minh", Country = vietnam },
                new City { Name = "Nha Trang", Country = vietnam },
                new City { Name = "Berlin", Country = germany },
                new City { Name = "Hamburg", Country = germany },
                new City { Name = "Munich", Country = germany },
                new City { Name = "Cologne", Country = germany }
            });
        }

        private static void SeedExperience(ModelContext context)
        {
            context.Experiences.AddRange(new[]
            {
                new Experience { Measurement = "<2 Years", Level = 1 },
                new Experience { Measurement = "2+ Years", Level = 2 },
                new Experience { Measurement = "5+ Years", Level = 5 },
                new Experience { Measurement = "10+ Years", Level = 10 },
                new Experience { Measurement = "15+ Years", Level = 15 },
                new Experience { Measurement = "20+ Years", Level = 20 }
            });

            context.Positions.AddRange(new[]
            {
                new Position { Name = "Doctor" },
                new Position { Name = "Nurse" },
                new Position { Name = "Physician" },
                new Position { Name = "Student" },
                new Position { Name = "Attending" }
            });
        }
    }
}