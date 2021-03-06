using System;
using System.Collections.Generic;
using System.Linq;
using Silent.Tetris.Contracts;

namespace Silent.Tetris
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IDictionary<Tuple<Type, string>, Func<IDependencyResolver, object>> _registeredFactories = new Dictionary<Tuple<Type, string>, Func<IDependencyResolver, object>>();

        public ICollection<Tuple<Type, string>> Registrations => _registeredFactories.Keys.ToList();

        public TService Resolve<TService>() where TService : class
        {
            return Resolve<TService>(string.Empty);
        }

        public TService Resolve<TService>(string serviceName) where TService : class
        {
            Tuple<Type, string> key = new Tuple<Type, string>(typeof(TService), serviceName);
            if (_registeredFactories.ContainsKey(key) == false)
            {
                string exceptionMessage = string.IsNullOrWhiteSpace(serviceName)
                    ? $"Service of type '{typeof(TService)}' is not registered"
                    : $"Service of type '{typeof(TService)}' with name '{serviceName}' is is not registered";

                throw new ArgumentException(exceptionMessage);
            }

            return _registeredFactories[key].Invoke(this) as TService;
        }

        public void Register<TService>(object serviceInstance) where TService : class
        {
            Register<TService>(string.Empty, serviceInstance);
        }

        public void Register<TService>(string serviceName, object serviceInstance) where TService : class
        {
            Register<TService>(serviceName, container => serviceInstance);
        }

        public void Register<TService>(Func<IDependencyResolver, object> factoryMethod) where TService : class
        {
            Register<TService>(string.Empty, factoryMethod);
        }

        public void Register<TService>(string serviceName, Func<IDependencyResolver, object> factoryMethod) where TService : class
        {
            Tuple<Type, string> key = new Tuple<Type, string>(typeof(TService), serviceName);
            if (_registeredFactories.ContainsKey(key))
            {
                string exceptionMessage = string.IsNullOrWhiteSpace(serviceName)
                    ? $"Service of type '{typeof(TService)}' is already registered"
                    : $"Service of type '{typeof(TService)}' with name '{serviceName}' is already registered";

                throw new ArgumentException(exceptionMessage);
            }

            _registeredFactories.Add(key, factoryMethod);
        }
    }
}