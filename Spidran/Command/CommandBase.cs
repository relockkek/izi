using MyMediator.Interfaces;

namespace Spidran.Command
{
    public abstract class CommandBase : IRequest<Result>
    {
        public DateTime CreatedAt { get; set; }
        public string ClientIp { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }
}
