using System;

namespace MongoTest
{
    public class TenantMongoNotificationInfo
    {
        public Guid Id { get; set; }

        public virtual int? TenantId { get; set; }

        public virtual string NotificationName { get; set; }

        public virtual string Data { get; set; }

        public virtual string DataTypeName { get; set; }

        public virtual string EntityTypeName { get; set; }

        public virtual string EntityTypeAssemblyQualifiedName { get; set; }

        public virtual string EntityId { get; set; }

        public virtual int Severity { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}