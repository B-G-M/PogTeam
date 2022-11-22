using System;
using System.Collections.Generic;
using System.Text;

namespace AuntificationAPI
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int WinGames { get; set; }

    }
}
