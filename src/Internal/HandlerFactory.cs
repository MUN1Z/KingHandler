using Microsoft.Extensions.DependencyInjection;
using System;

namespace KingHandler.Internal
{
    /// <summary>
    /// Provides methods to create and release handlers.
    /// </summary>
    internal sealed class HandlerFactory : IHandlerFactory
    {
        private readonly ITypeActivatorCache _typeActivatorCache;

        /// <summary>
        /// Creates a new <see cref="HandlerFactory"/> instance.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="typeActivatorCache"></param>
        public HandlerFactory(ITypeActivatorCache typeActivatorCache)
        {
            _typeActivatorCache = typeActivatorCache;
        }

        /// <inheritdoc />
        public object CreateHandler(IServiceScope scope, Type handlerType)
        {
            if (handlerType == null)
                throw new ArgumentNullException(nameof(handlerType));

            return _typeActivatorCache.Create<object>(scope, handlerType);
        }

        /// <inheritdoc />
        public void ReleaseHandler(object handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (handler is IDisposable disposableHandler)
                disposableHandler.Dispose();
        }
    }
}
