namespace TortenkreationSHSBlog.Models {

    using System;

    public class PersistentEntity {

        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public PersistentEntity() {
            this.CreatedAt = new DateTimeOffset(DateTime.Now);
        }

    }

}