using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace Scaffold.Core.Events.Requests
{
    public interface IRequestBroker
    {
        public void Register<TRequest, TResponse>(Func<TRequest, TResponse> listener) where TRequest : IRequest;
        public void RegisterAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> listener) where TRequest : IRequest;
        public TResponse Raise<TRequest, TResponse>(TRequest request) where TRequest : IRequest;
        public Task<TResponse> RaiseAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest;
    }
}