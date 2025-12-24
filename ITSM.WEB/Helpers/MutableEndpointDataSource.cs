using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace ITSM.WEB.Helpers
{
    // Simple mutable EndpointDataSource used to provide a placeholder EndpointDataSource
    // during DI construction and later populated with actual endpoints after endpoint mapping.
    public class MutableEndpointDataSource : EndpointDataSource
    {
        private volatile IReadOnlyList<Endpoint> _endpoints = Array.Empty<Endpoint>();

        public override IReadOnlyList<Endpoint> Endpoints => _endpoints;

        public override IChangeToken GetChangeToken()
        {
            // Return a change token that never signals (simple implementation)
            return new CancellationChangeToken(new CancellationToken());
        }

        public void SetEndpoints(IEnumerable<Endpoint>? endpoints)
        {
            _endpoints = endpoints?.ToArray() ?? Array.Empty<Endpoint>();
        }
    }
}
