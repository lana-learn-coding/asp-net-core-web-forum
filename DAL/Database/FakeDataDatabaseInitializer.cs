using System;
using System.Linq;
using Bogus;
using DAL.Models;
using DAL.Models.Auth;
using DAL.Models.Forum;

namespace DAL.Database
{
    // Fake additional data (for testing purpose)
    // The generated data is not relevant to this project real domain
    // but it is useful when you need a lot of data to test
    public class FakeDataDatabaseInitializer : DatabaseInitializer
    {
        protected override void Seed(ModelContext context)
        {
            base.Seed(context);

            Console.WriteLine("Start generating fake data...");
            FakeUsers(context);
            context.SaveChanges();

            FakeForums(context);
            context.SaveChanges();

            FakeThreads(context);
            context.SaveChanges();
            Console.WriteLine("Generation completed.");
        }

        private static void FakeUsers(ModelContext context)
        {
            Console.WriteLine("Start generating users:");
            for (var i = 1; i <= 20; i++)
            {
                var faker = new Faker();
                var user = new User
                {
                    Avatar = faker.Person.Avatar,
                    Username = faker.Person.UserName + i,
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    Email = faker.Person.Email + i,
                };
                context.Users.Add(user);
                Console.WriteLine($"User: {user.Username}");

                var cities = context.Cities.Include("Country").ToList();
                var specialities = context.Specialties.ToList();
                var experiences = context.Experiences.ToList();
                var positions = context.Positions.ToList();
                var city = faker.Random.CollectionItem(cities);
                user.UserInfo = new UserInfo
                {
                    User = user,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    DateOfBirth = faker.Person.DateOfBirth,
                    Phone = faker.Person.Phone,
                    WorkCity = city,
                    WorkCountry = city.Country,
                    WorkExperience = faker.Random.ListItem(experiences),
                    WorkPosition = faker.Random.ListItem(positions),
                    WorkSpecialities = faker.Random.ListItems(specialities),
                    WorkPhone = faker.Person.Phone,
                    WorkAddress = $"{faker.Person.Address.Street}, {faker.Person.Address.State}",
                    WorkDescription = faker.Name.JobTitle()
                };
            }
        }

        private static void FakeForums(ModelContext context)
        {
            Console.WriteLine("Start generating forums:");

            var categories = context.Categories.ToList();
            for (var i = 0; i < 20; i++)
            {
                var faker = new Faker();
                var forum = new Forum
                {
                    Title = $"{faker.Company.CatchPhrase()} {i}",
                    SubTitle = $"{faker.Name.JobDescriptor()} {faker.Name.JobTitle()}",
                    Description = faker.Lorem.Sentence(10, 10),
                    CategoryId = faker.Random.ListItem(categories).Id,
                    ForumAccess = AccessMode.Public,
                    ThreadAccess = AccessMode.Public
                };
                context.Forums.Add(forum);
                Console.WriteLine($"Forum: {forum.Title}");
            }
        }

        private static void FakeThreads(ModelContext context)
        {
            Console.WriteLine("Start generating threads:");

            var users = context.Users.ToList();
            var forums = context.Forums.ToList();
            var tags = context.Tags.ToList();
            for (var i = 0; i < 200; i++)
            {
                var id = Guid.NewGuid();
                var faker = new Faker();
                var postCount = faker.Random.Int(10, 50);
                var thread = new Thread
                {
                    Id = id,
                    ForumId = forums[faker.Random.Int(0, forums.Count - 1)].Id,
                    Title =
                        $"{faker.Music.Genre() + faker.Commerce.ProductMaterial() + faker.Name.JobTitle() + faker.Company.CompanyName()} {i}",
                    Tags = tags.Skip(faker.Random.Int(3, 8)).ToList(),
                    ViewsCount = faker.Random.Int(10, 10 * postCount)
                };
                var originPost = new Post
                {
                    Id = id,
                    ThreadId = id,
                    Content = string.Join("<br>",
                        faker.Rant.Reviews(faker.Random.ListItem(tags).Name, faker.Random.Int(2, 10))),
                    UserId = faker.Random.ListItem(users).Id
                };
                context.Posts.Add(originPost);
                context.Threads.Add(thread);
                Console.WriteLine($"Thread: {thread.Title}");

                for (var j = 0; j < postCount; j++)
                {
                    var post = new Post
                    {
                        ThreadId = thread.Id,
                        Content = string.Join("<br>",
                            faker.Rant.Reviews(faker.Random.ListItem(tags).Name, faker.Random.Int(2, 6))),
                        UserId = faker.Random.ListItem(users).Id
                    };
                    context.Posts.Add(post);
                }

                var voteCount = faker.Random.Int(0, users.Count - 1);
                for (var j = 0; j < voteCount; j++)
                {
                    var vote = new Vote
                    {
                        Post = originPost,
                        Value = (short)(new Faker().Random.Bool() ? 1 : -1),
                        UserId = users[j].Id
                    };
                    context.Votes.Add(vote);
                }

                Console.WriteLine($"Post({postCount}): {thread.Title}");
            }
        }
    }
}