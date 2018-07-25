using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using PollyConsole;

namespace MongoTest
{
    class Program
    {
        private static Stopwatch stopWatch = new Stopwatch();
        private static SemaphoreSlim semaphore = new SemaphoreSlim( 300);
        private static volatile int total = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Mapper.Initialize(cfg => cfg.CreateMap<TypeA, TypeB>());
            var productDTO = Mapper.Map<TypeA>(new TypeB{Enum = EnumB.Second});

        }

        static  void Test()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("fcloud_dev");
            var collection = database.GetCollection<UserNotificationMongoInfo>("UserNotificationMongoInfo");
            stopWatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                var t = new Thread((() => Create(collection)));
                t.Start();
            }
       //     var ranges = Enumerable.Range(1, 100001).ToList();
        //    var tasks = ranges.Select(r => Task.Run(() => Create(collection))).ToList();
            //Parallel.For(1, 100001,  (index, state) =>  Create(collection));
            Console.ReadLine();
        }


        static void Create(IMongoCollection<UserNotificationMongoInfo> collection)
        {
            var item = CreateUserNotification(0);
            semaphore.Wait();
           collection.InsertOne(item);
            semaphore.Release();
            total++;
            if (total >= 99999)
            {
                stopWatch.Stop();
                Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
        }

        static UserNotificationMongoInfo CreateUserNotification(int x)
        {
            var item = new UserNotificationMongoInfo
            {
                TenantId = new Random().Next(10000),
                Guid = Guid.NewGuid(),
                State = new Random().Next(3),
                UserId = new Random().Next(10000),
                Id = ObjectId.GenerateNewId(),
                MongoNotificationInfo = new TenantMongoNotificationInfo
                {
                    CreationTime = DateTime.Now,
                    CreatorUserId = 1,
                    Data = "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhha",
                    DataTypeName = "xx",
                    Id = Guid.NewGuid(),
                    Severity = 2,
                    NotificationName = "XX",
                    EntityId = "",
                    EntityTypeAssemblyQualifiedName = "",
                    EntityTypeName = ""
                }
            };
            return item;
        }
    }
}