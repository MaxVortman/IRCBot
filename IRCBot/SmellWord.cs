namespace IrcBot
{
    public class SmellWord
    {
        public int Position { get; }
        public string Word { get; set; }

        public SmellWord(string word, int position)
        {
            Word = word;
            Position = position;
        }
    }
}