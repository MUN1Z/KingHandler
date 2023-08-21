using KingHandler.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KingHandler.Internal
{
    internal class HandlerActionCache : IHandlerActionCache
    {
        private readonly IDictionary<object, HandlerActionModel> _handlerCache;

        /// <summary>
        /// Creates a new <see cref="HandlerActionCache"/> instance.
        /// </summary>
        /// <param name="cacheEntries">Cached entries.</param>
        public HandlerActionCache(IDictionary<object, HandlerActionModel> cacheEntries)
        {
            _handlerCache = new ConcurrentDictionary<object, HandlerActionModel>(cacheEntries);
        }

        /// <inheritdoc />
        public HandlerActionModel GetHandlerAction(object handlerAction)
        {
            return _handlerCache.TryGetValue(handlerAction, out HandlerActionModel value) ? value : null;
        }
    }
}
