namespace TortenkreationSHSBlog.Models {

    using System.ComponentModel.DataAnnotations;

    public class User {

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public User() {
            //nothing to do
        }

        public User(string username, string password) {
            this.Username = username;
            this.Password = password;
        }

    }

}