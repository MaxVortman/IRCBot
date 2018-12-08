using System.Threading.Tasks;

namespace IrcBot
{
    public interface IIrcBot
    {
        Task<string> ProceedAsync(IrcMessage message);
    }
}