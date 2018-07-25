using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MongoTest
{
    public class UserNotificationMongoInfo 
    {
        public ObjectId Id { get; set; }
        public virtual int? TenantId { get; set; }

        public virtual long UserId { get; set; }

        public virtual int State { get; set; }

        public virtual DateTime CreationTime { get { return DateTime.Now;} }

        public virtual Guid Guid { get; set; }

        public virtual TenantMongoNotificationInfo MongoNotificationInfo { get; set; }
    }
}
