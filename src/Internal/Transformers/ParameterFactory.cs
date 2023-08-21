using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace KingHandler.Internal.Transformers
{
    internal class ParameterFactory : IParameterFactory
    {
        private readonly ITypeActivatorCache _typeActivatorCache;

        /// <summary>
        /// Creates a new <see cref="ParameterFactory"/> instance.
        /// </summary>
        /// <param name="typeActivatorCache">Type Activator cache.</param>
        public ParameterFactory(ITypeActivatorCache typeActivatorCache)
        {
            _typeActivatorCache = typeActivatorCache;
        }

        /// <inheritdoc />
        public object Create(IServiceScope scope, TypeInfo type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return _typeActivatorCache.Create<object>(scope, type.AsType());
        }
    }
}
