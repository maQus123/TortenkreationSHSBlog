namespace TortenkreationSHSBlog.Models {

    using System;

    public class PersistentEntity {

        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public PersistentEntity() {
            this.CreatedAt = new DateTimeOffset(DateTime.UtcNow);
        }

    }

}