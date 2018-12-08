using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IrcBot
{
    public class IrcClient : IDisposable
    {

        private readonly Socket _ircSocket;
        private readonly string _server;
        private readonly int _port;

        public IrcClient(string server, int port)
        {
            _server = server;
            _port = port;
            _ircSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(Credentials credentials)
        {
            _ircSocket.Connect(_server, _port);
            
            Send("NICK " + credentials.Nick);
            Send($"USER {credentials.Nick} {credentials.Host} {credentials.Server} :{credentials.RealName}");
        }
        
        public void Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message + "\r\n");

            _ircSocket.Send(data);  
        }

        public void SendMultiLineMessage(string text, string destination)
        {
            var lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                SendMessage(line, destination);
                Thread.Sleep(50);
            }
        }

        public void SendMessage(string message, string destination)
        {
            Send($"PRIVMSG {destination} :{message}");
        }

        public string ReadLine()
        {
            var data = new byte[1];
            var builder = new StringBuilder();
            string symbol;
            do
            {
                var bytes = _ircSocket.Receive(data, data.Length, 0);
                symbol = Encoding.UTF8.GetString(data, 0, bytes);
                builder.Append(symbol);
            } while (symbol != "\n" && _ircSocket.Available > 0);

            var line = builder.ToString();

            if (!line.StartsWith("PING")) return line;
            
            var splits = line.Split();
            var pongReply = splits[1];
            Send("PONG " + pongReply);

            return line;
        }

        public void Join(string channel)
        {
            Send("JOIN " + channel);
        }

        public void Dispose()
        {
            _ircSocket?.Shutdown(SocketShutdown.Both);
            _ircSocket?.Close(10);
        }
    }
}