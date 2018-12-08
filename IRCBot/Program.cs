using System;
using System.Collections.Generic;

namespace IrcBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = new Credentials();
            
            Console.WriteLine("Enter your nickname: ");
            credentials.Nick = Console.ReadLine();
            
            Console.WriteLine("Enter your real name: ");
            credentials.RealName = Console.ReadLine();

            Console.WriteLine("Enter channel: ");            
            credentials.Channel = Console.ReadLine();
            
            credentials.Host = "0";
            credentials.Server = "*";
            
            var chat = new ConsoleChat("chat.freenode.net", 6667, credentials);

            chat.Start(new List<IIrcBot> {new SpellCheckerBot(credentials.Nick, credentials.Channel)});
        }
    }
}