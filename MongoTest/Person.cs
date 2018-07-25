using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PollyConsole
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Server Server { get; set; }
    }

    public enum Server
    {
        Normal,
        High
    }
}
