using System.Collections;
using UnityEngine;
using Scaffold.Core.Events.Requests;
using System.Threading.Tasks;
using System;

namespace Scaffold.Core.Events
{
    public class Request
    {
        public Request(IRequestBroker requestBroker)
        {
            if (_broker != null)
            {
                Debug.Log("Request is already Initialized");
                return;
            }
            _broker = requestBroker;
        }

        private static IRequestBroker broker
        {
            get
            {
                if (_broker == null)
                {
                    Debug.Log("Request not initialized, pass a valid EventBroker on the Events constructor");
                }
                return _broker;
            }
        }

        private static IRequestBroker _broker;

        public static TResponse Raise<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            return broker.Raise<TRequest, TResponse>(request);
        }
        public static TResponse Raise<TRequest, TResponse>() where TRequest : IRequest, new()
        {
            TRequest request = new TRequest();
            return Raise<TRequest, TResponse>(request);
        }
        public static Task<TResponse> RaiseAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            return broker.RaiseAsync<TRequest, TResponse>(request);
        }
        public static Task<TResponse> RaiseAsync<TRequest, TResponse>() where TRequest : IRequest, new()
        {
            TRequest request = new TRequest();
            return RaiseAsync<TRequest, TResponse>(request);
        }
        public static void Register<TResponse, TRequest>(Func<TRequest, TResponse> listener) where TRequest : IRequest
        {
            if (listener == null)
            {
                return;
            }

            broker.Register(listener);
        }
        public static void RegisterAsync<TResponse, TRequest>(Func<TRequest, Task<TResponse>> listener) where TRequest : IRequest
        {
            if (listener == null)
            {
                return;
            }

            broker.RegisterAsync(listener);
        }
    }
}