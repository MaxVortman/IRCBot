using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IrcBot
{
    public class ConsoleChat
    {
        private readonly string _server;
        private readonly int _port;
        private readonly Credentials _credentials;

        public ConsoleChat(string server, int port, Credentials credentials)
        {
            _server = server;
            _port = port;
            _credentials = credentials;
        }
        
        public void Start(IEnumerable<IIrcBot> bot)
        {
            using (var irc = new IrcClient(_server, _port))
            {
                irc.Connect(_credentials);

                var cancelWriteSource = new CancellationTokenSource();
                
                Task.Run(async () =>
                {
                    while (!cancelWriteSource.IsCancellationRequested)
                    {
                        var chatMessage = irc.ReadLine();
                        
                        if(chatMessage.Contains("001")) irc.Join(_credentials.Channel);
                        
                        Console.Write(">> ");
                        Console.WriteLine(chatMessage);

                        var ircMessage = new IrcMessage(chatMessage);
                        
                        if(!ircMessage.IsSuccess) continue;

                        var botMessage = await bot.ProceedAsync(ircMessage);
                        if (!string.IsNullOrEmpty(botMessage))
                        {
                            irc.SendMultiLineMessage(botMessage,
                                ircMessage.IsChannelMessage() ? ircMessage.Receiver : ircMessage.Sender);
                        }
                    }
                }, cancelWriteSource.Token);

                while (true)
                {
                    var message = Console.ReadLine();
                    
                    if(message == "-q") break;
                    
                    irc.Send(message);    
                }
                
                cancelWriteSource.Cancel();
                
                Thread.Sleep(10);
            }
        }
    }
}