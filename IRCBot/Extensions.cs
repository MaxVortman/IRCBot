using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IrcBot
{
    public static class Extensions
    {
        public static async Task<string> ProceedAsync(this IEnumerable<IIrcBot> bots, IrcMessage message)
        {
            var builder = new StringBuilder();
            
            foreach (var ircBot in bots)
            {
                var text = await ircBot.ProceedAsync(message);
                if (!string.IsNullOrEmpty(text))
                {
                    builder.AppendLine(text);
                }
            }

            return builder.ToString();
        }

        public static SmellWord[] SplitSmellWords(this string text)
        {
            var splits = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var smellSplits = new SmellWord[splits.Length];
            int j = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                var word = splits[i];
                while (word[0] != text[j]) ++j;
                smellSplits[i] = new SmellWord(word, j);
                while (text.Length > j && text[j] != ' ') ++j;
            }

            return smellSplits;
        }

        public static void SetAtPosition(this IEnumerable<SmellWord> smellWords, string word, int position)
        {
            foreach (var smellWord in smellWords)
            {
                if (smellWord.Position == position)
                {
                    smellWord.Word = word;
                    break;
                }
            }
        }
    }
}