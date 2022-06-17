using System;
using System.Collections.Generic;

#nullable disable

namespace FarmCentralApp.Models
{
    public partial class User
    {
        public User()
        {
            Products = new HashSet<Product>();
        }

        public User(int usersId, string usersFirstname,string usersSurname, string usersPassword, string usersRole)
        {
            UsersId = usersId;
            UsersFirstname = usersFirstname;
            UsersSurname = usersSurname;
            UsersPassword = usersPassword;
            UsersRole = usersRole;
        }

        public int UsersId { get; set; }
        public string UsersFirstname { get; set; }
        public string UsersSurname { get; set; }
        public string UsersPassword { get; set; }
        public string UsersRole { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
