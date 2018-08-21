using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new Dictionary<string, object>
            {
                { "group.id", "test-consumer-group" },
                { "bootstrap.servers", "localhost:9092" },
                { "auto.commit.interval.ms", 5000 },
                { "auto.offset.reset", "earliest" }
            };

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Assign(new List<TopicPartition>()
                {
                    new TopicPartition("x",2)
                    
                });
                consumer.OnMessage += (_, msg)
                    =>
                {
                    Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");
                };

                consumer.OnError += (_, error)
                    => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                    => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe("my-topic");




                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }
        }
    }
}
