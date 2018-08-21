using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoTest
{
    public class UserNotificationMongoInfo 
    {
        public Guid Id { get; set; }
        public virtual int? TenantId { get; set; }

        public virtual long UserId { get; set; }

        public virtual int State { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public virtual DateTime CreationTime { get; set; }

        public virtual Guid Guid { get; set; }

        public virtual TenantMongoNotificationInfo MongoNotificationInfo { get; set; }
    }
}
