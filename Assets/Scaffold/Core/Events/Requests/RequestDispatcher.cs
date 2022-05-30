using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Scaffold.Core.Events.Requests
{
    public class RequestDispatcher : IRequestBroker
    {
        public TResponse Raise<TRequest, TResponse>(Type responseType, TRequest request) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> RaiseAsync<TRequest, TResponse>(Type responseType, Type requestType, TRequest request) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }

        public void Register<TRequest, TResponse>(Type type, Func<TRequest, TResponse> listener) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }
    }
}