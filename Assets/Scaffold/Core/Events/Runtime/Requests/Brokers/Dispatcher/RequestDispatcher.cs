using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Scaffold.Core.Events.Requests
{
    public class RequestDispatcher : IRequestBroker
    {
        public TResponse Raise<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            return RequestHandler<TRequest, TResponse>.Resolve(request);
        }

        public Task<TResponse> RaiseAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            return AsyncRequestHandler<TRequest, TResponse>.Resolve(request);
        }

        public void Register<TRequest, TResponse>(Func<TRequest, TResponse> listener) where TRequest : IRequest
        {
            RequestHandler<TRequest, TResponse>.RegisterHandler(listener);
        }

        public void RegisterAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> listener) where TRequest : IRequest
        {
            AsyncRequestHandler<TRequest, TResponse>.RegisterHandler(listener);
        }

        private class RequestHandler<TRequest, TResponse>
        {
            private static Func<TRequest, TResponse> _callback;

            public static void RegisterHandler(Func<TRequest, TResponse> callback)
            {
                if (callback == null)
                {
                    return;
                }

                if (_callback != null)
                {
                    Debug.Log($"Resolver for {typeof(TRequest)}|{typeof(TResponse)} was already defined");
                    return;
                }

                _callback = callback;
            }

            public static TResponse Resolve(TRequest request)
            {
                if(_callback == null)
                {
                    Debug.Log("No resolver defined for this Request|Response pair");
                    return default(TResponse);
                }
                return _callback(request);
            }
        }

        private class AsyncRequestHandler<TRequest, TResponse>
        {
            private static Func<TRequest, Task<TResponse>> _callback;

            public static void RegisterHandler(Func<TRequest, Task<TResponse>> callback)
            {
                if (callback == null)
                {
                    return;
                }

                if(_callback != null)
                {
                    Debug.Log($"Resolver for {typeof(TRequest)}|{typeof(TResponse)} was already defined");
                    return;
                }

                _callback = callback;
            }

            public static Task<TResponse> Resolve(TRequest request)
            {
                if (_callback == null)
                {
                    Debug.Log($"No async resolver defined for {typeof(TRequest)}|{typeof(TResponse)} pair");
                    return Task.FromResult(default(TResponse));
                }

                return _callback.Invoke(request);
            }
        }
    }
}