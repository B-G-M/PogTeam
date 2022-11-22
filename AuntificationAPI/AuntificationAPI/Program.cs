using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AuntificationAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // добавление данных
            using (ApplicationContext db = new ApplicationContext())
            {

                User user1 = new User { Email = "Tom@mail.com", Password = "33",WinGames= 0 };
                User user2 = new User { Email = "Jaff@mail.com", Password = "44", WinGames = 5 };

                db.Users.AddRange(user1, user2);
                db.SaveChanges();

             
            }
            // получение данных
            using (ApplicationContext db = new ApplicationContext())
            {

                AuntificationMethods.Login("Jaff@mail.com", "44");

                AuntificationMethods.Registration("Jaff@mail.com", "44");
                AuntificationMethods.Registration("Garry@mail.com", "46");
            }
        }
    }
}
