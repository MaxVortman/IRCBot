using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Speller.Api;
using Yandex.Speller.Api.DataContract;

namespace IrcBot
{
    public class SpellCheckerBot : IIrcBot
    {
        private readonly string _nick;
        private readonly string _channel;
        private readonly YandexSpeller _speller;

        public SpellCheckerBot(string nick, string channel)
        {
            _nick = nick;
            _channel = channel;
            _speller = new YandexSpeller();
        }

        public async Task<string> ProceedAsync(IrcMessage ircMessage)
        {
            var message = ircMessage.Message;
            if (ircMessage.IsChannelMessage() && message.StartsWith(_nick + ":"))
            {
                message = message.Substring(_nick.Length + 1).Trim();
            } else if (ircMessage.IsChannelMessage())
            {
                return string.Empty;
            }

            var spellResult =
                await Task.Run(() => _speller.CheckText(message, Lang.En, Options.Default, TextFormat.Plain));

            return MakeResultString(spellResult.Errors, message);
        }

        private static string MakeResultString(IEnumerable<Error> errors, string text)
        {
            var inputTextSplits = text.SplitSmellWords();
    
            foreach (var error in errors)
            {
                var steer = error.Steer.FirstOrDefault();
                if (steer != null)
                    inputTextSplits.SetAtPosition(steer, error.Pos);
            }

            return inputTextSplits.Select(word => word.Word).Aggregate((acc, word) => acc += " " + word);
        }
    }
}