using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuntificationAPI
{
    class AuntificationMethods
    {

        public static void Login(String email, String password)
        {
            User user = null;

            using (ApplicationContext db = new ApplicationContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            }
            if (user != null)
            {
               Console.WriteLine("Login confirmed");
            }
            else
            {
                Console.WriteLine("put correct data");
            }
        }

        public static void Registration(String email, String password)
        {

            User user = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == email);
            }

            if (user == null)
            {
                // создаем нового пользователя
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Users.Add(new User { Email = email, Password = password, WinGames = 0 });
                    db.SaveChanges();

                    user = db.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                }
                // если пользователь удачно добавлен в бд
                if (user != null)
                {
                    Console.WriteLine("Добавлен в бд");
                }
            }
            else
            {
                Console.WriteLine( "Пользователь с таким логином уже существует");
            }
        }
    }
}
