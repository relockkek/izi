using FluentValidation;
using MyMediator.Interfaces;
using MyMediator.Types;
using Spidran.Command;

namespace Spidran.Behaviors
{

    public class ClientInfoBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : Spidran.Command.CommandBase, MyMediator.Interfaces.IRequest<TResponse>
        where TResponse : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientInfoBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                request.CreatedAt = DateTime.UtcNow;
                request.ClientIp = GetClientIpAddress();
                request.UserAgent = httpContext.Request.Headers["User-Agent"].ToString();
            }

            return await next();
        }

        public Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private string GetClientIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return string.Empty;

            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return httpContext.Request.Headers["X-Forwarded-For"].ToString();
            }

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }
    }
}
