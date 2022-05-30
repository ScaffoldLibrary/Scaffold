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
            if(_broker != null)
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

        public static TResponse Raise<TRequest, TResponse>(TRequest request) where TRequest: IRequest
        {
            return broker.Raise<TRequest, TResponse>(typeof(TResponse), request);
        }

        public static Task<TResponse> RaiseAsync<TResponse, TRequest>(TRequest request) where TRequest: IRequest
        {
            return broker.RaiseAsync<TRequest, TResponse>(typeof(TResponse), typeof(TRequest), request);
        }

        public static void Register<TResponse, TRequest>(TRequest request, Func<TRequest, TResponse> listener) where TRequest: IRequest
        {
            broker.Register(typeof(TResponse), listener);
        } 

    }
}