using MediatR;
using Microsoft.Extensions.Logging;
using Northwind.Application.Common.Interfaces;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.Application.Common.Behaviours;

public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;

    public RequestPerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        if (_timer.ElapsedMilliseconds > 500)
        {
            string name = typeof(TRequest).Name;

            _logger.LogWarning("Northwind Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}", 
                name, _timer.ElapsedMilliseconds, _currentUserService.UserId, request);
        }

        return response;
    }
}