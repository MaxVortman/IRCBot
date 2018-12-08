using System.Text.RegularExpressions;

namespace IrcBot
{
    public class IrcMessage
    {
        private static Regex _regex = new Regex($@":([\W|\w]+?)![\W|\w]* PRIVMSG ([\W|\w]+) :([\W|\w]*)", RegexOptions.Compiled);
        //:brain223!c313f751@gateway/web/freenode/ip.195.19.247.81 PRIVMSG max2018 :hi
        //:brain223!c313f751@gateway/web/freenode/ip.195.19.247.81 PRIVMSG #net2233 :max2018: hi


        public IrcMessage(string ircMessage)
        {
            IrcTextMessage = ircMessage;
            var match = _regex.Match(ircMessage);
            if (match.Success)
            {
                Sender = match.Groups[1].Value;
                Receiver = match.Groups[2].Value;
                Message = match.Groups[3].Value;
            }

            IsSuccess = match.Success;
        }

        public string IrcTextMessage { get; }
        public string Sender { get; }
        public string Message { get; }
        public string Receiver { get; }
        public bool IsSuccess { get; }

        public bool IsChannelMessage() => Receiver.StartsWith('#');
    }
}