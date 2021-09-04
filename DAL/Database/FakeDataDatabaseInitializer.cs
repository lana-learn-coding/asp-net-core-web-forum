using System;
using System.Linq;
using Bogus;
using DAL.Models;
using DAL.Models.Auth;
using DAL.Models.Forum;

namespace DAL.Database
{
    // Fake additional data (for testing purpose)
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
                    Priority = faker.Random.Short((short)Priority.Normal, (short)Priority.VeryHigh),
                    CategoryId = categories[faker.Random.Int(0, 3)].Id,
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
                var thread = new Thread
                {
                    Id = id,
                    ForumId = forums[faker.Random.Int(0, forums.Count - 1)].Id,
                    Title =
                        $"{faker.Music.Genre() + faker.Commerce.ProductMaterial() + faker.Name.JobTitle() + faker.Company.CompanyName()} {i}",
                    Tags = tags.Skip(faker.Random.Int(3, 8)).ToList()
                };
                var originPost = new Post
                {
                    Id = id,
                    ThreadId = id,
                    IsOrigin = true,
                    Content = faker.Rant.Review(),
                    UserId = users[faker.Random.Int(0, users.Count - 1)].Id
                };
                context.Posts.Add(originPost);
                context.Threads.Add(thread);
                Console.WriteLine($"Thread: {thread.Title}");

                var postCount = faker.Random.Int(10, 50);
                for (var j = 0; j < postCount; j++)
                {
                    var post = new Post
                    {
                        ThreadId = thread.Id,
                        Content = faker.Rant.Review(),
                        UserId = users[faker.Random.Int(0, users.Count - 1)].Id
                    };
                    context.Posts.Add(post);
                }

                Console.WriteLine($"Post({postCount}): {thread.Title}");
            }
        }
    }
}