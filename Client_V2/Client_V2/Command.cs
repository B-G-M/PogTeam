using System;
using System.Net.Sockets;

namespace Client_V2
{
    public class Command
    {
        private ConnToServer _connToServer;

        public Command(ConnToServer connToServer)
        {
            _connToServer = connToServer;
        }


        private void ExitGame(Socket socket)
        {
            if (socket.Connected)
            {
                socket.Close();
            }
        }

        public void GetAuthentification(String login, String password)
        {
            String message = $"Authentification {login} {password}";
            _connToServer.SendMessage(message);
        }
    }
}