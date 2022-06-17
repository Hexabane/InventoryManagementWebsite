using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmCentralApp.OOP
{
    public class Users : IComparable
    {
        private int userID;
        private string userFN;
        private string userSN;
        private string userPassword;
        private string userRole;

        public Users(int userID, string userFN, string userSN, string userPassword, string userRole)
        {
            UserID = userID;
            UserFN = userFN;
            UserSN = userSN;
            UserPassword = userPassword;
            UserRole = userRole;
        }

        public int UserID { get => userID; set => userID = value; }
        public string UserFN { get => userFN; set => userFN = value; }
        public string UserSN { get => userSN; set => userSN = value; }
        public string UserPassword { get => userPassword; set => userPassword = value; }
        public string UserRole { get => userRole; set => userRole = value; }

        public int CompareTo(object obj)
        {
            return UserID.CompareTo(obj.ToString());


        }
        public override string ToString()
        {
            return UserID.ToString();
        }

    }
}
