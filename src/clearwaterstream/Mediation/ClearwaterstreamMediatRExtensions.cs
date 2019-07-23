using clearwaterstream.IoC;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks
{
    public static class ClearwaterstreamMediatRExtensions
    {
        public static Task<TResponse> Execute<TResponse>(this IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var mediator = ServiceRegistrar.Current.GetMediator();

            var response = mediator.Send(request, cancellationToken);

            return response;
        }
    }
}