using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using PollyConsole;

namespace MongoTest
{
    class Program
    {
        static SemaphoreSlim _semaphore = new SemaphoreSlim(300);
        private static Stopwatch stopWatch = new Stopwatch();
        private static  int total = 0;
        static void Main(string[] args)
        {

            

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("fcloud_dev");
            var collection = database.GetCollection<UserNotificationMongoInfo>("UserNotifications");
            collection.InsertOne(new UserNotificationMongoInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)
        });
            
          var t =  collection.AsQueryable().OrderBy(s=>s.CreationTime).ToList().First();

            Console.WriteLine("Press a key...");
            Console.ReadKey();
        }

        static void TestClosure(Guid id)
        {
            Task.Run( ()=>
            {
                int y = 5;
                Console.WriteLine(id);
            });
        }

        static bool CollectionHasIndex<T>(IMongoCollection<T> collection  ,string indexName)
        {
            var indexes = collection.Indexes.List();
            while (indexes.MoveNext())
            {
                var currentIndex = indexes.Current;
                foreach (var doc in currentIndex)
                {
                    var docNames = doc.Names;

                    foreach (string name in docNames)
                    {
                        var value = doc.GetValue(name);
                        
                        if (string.Equals(value.ToString(), indexName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static void CreateIndex<T>(IMongoCollection<T> collection, string indexName, string indexJson)
        {
            var indexDefinition =
                new CreateIndexModel<T>((IndexKeysDefinition<T>) indexJson,
                    new CreateIndexOptions {Name = indexName, Background = true});
            collection.Indexes.CreateOne(indexDefinition);
        }


        static void TestCreateIndex()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("fcloud_dev");
            var collection = database.GetCollection<UserNotificationMongoInfo>("UserNotifications");
            Console.WriteLine(CollectionHasIndex(collection, "user_createtime_state"));
            var index = "{\"UserId\" : 1, \"CreationTime\" : -1,\"State\" : 1} ";
            CreateIndex<UserNotificationMongoInfo>(collection, "user_createtime_state", index);

            Console.ReadLine();
        }

        static  void Test()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("fcloud_dev");
            var collection = database.GetCollection<UserNotificationMongoInfo>("UserNot");
            Insert(collection);
            stopWatch.Start();
            Parallel.For(1, 1000000, s => { Insert(collection); });
 
            Console.ReadLine();
        }

        static void Insert(IMongoCollection<UserNotificationMongoInfo> collection)
        {
                var item = CreateUserNotification(1);
            // _semaphore.Wait();
            try
            {
                collection.InsertOne(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+ " " + ex.StackTrace);
            }
               
            if (Interlocked.Increment(ref total) >= (1000000 - 2))
            {
                stopWatch.Stop();
                Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
            // _semaphore.Release();
           
        }
 
        static UserNotificationMongoInfo CreateUserNotification(int x)
        {
            var item = new UserNotificationMongoInfo
            {
                TenantId = new Random().Next(10000),
                Guid = Guid.NewGuid(),
                State = new Random().Next(3),
                UserId = new Random().Next(10000),
                Id = Guid.NewGuid(),
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