using System;
using System.Collections.Generic;
using System.Text;
using System;
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
                User user1 = new User { Name = "Tom", Age = 33 };
                User user2 = new User { Name = "Alice", Age = 26 };

                db.Users.AddRange(user1, user2);
                db.SaveChanges();
            }
            // получение данных
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = db.Users.ToList();
                Console.WriteLine("Список объектов:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                }
            }
        }
    }
}
