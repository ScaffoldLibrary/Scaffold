using System.Collections;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace Scaffold.Core.Events.Requests
{
    public interface IRequestBroker
    {
        //public void Register<TResponse, TRequest>(Type type, Action<TRequest> callback) where TRequest : IRequest;
        //public void Register<TResponse, TRequest>(Type type, Action callback) where TRequest : IRequest;
        //public void Unregister<TResponse, TRequest>(Type type, Action<TRequest> callback) where TRequest : IRequest;
        //public void Unregister<TResponse, TRequest>(Type type, Action callback) where TRequest : IRequest;
        //public void UnregisterAll(Type type);

        public void Register<TRequest, TResponse>(Type type, Func<TRequest, TResponse> listener) where TRequest : IRequest;
        public TResponse Raise<TRequest, TResponse>(Type responseType, TRequest request) where TRequest : IRequest;
        public Task<TResponse> RaiseAsync<TRequest, TResponse>(Type responseType, Type requestType, TRequest request) where TRequest : IRequest;
    }
}