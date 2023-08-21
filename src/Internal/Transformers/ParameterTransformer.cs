using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KingHandler.Internal.Transformers
{
    internal class ParameterTransformer : IParameterTransformer
    {
        private readonly ParameterTransformerCache _transformerCache;

        /// <summary>
        /// Creates a new <see cref="ParameterTransformer"/> instance.
        /// </summary>
        /// <param name="transformerCache">Parameter transformer cache.</param>
        public ParameterTransformer(ParameterTransformerCache transformerCache)
        {
            _transformerCache = transformerCache;
        }

        /// <inheritdoc />
        public object Transform(IServiceScope scope, object originalParameter, TypeInfo destinationParameterType)
        {
            ParameterTransformerCacheEntry transformer = _transformerCache.GetTransformer(destinationParameterType);

            if (transformer == null)
            {
                return null;
            }

            object destinationParameter = transformer.ParameterFactory(scope, destinationParameterType);

            return transformer.Transformer(originalParameter, destinationParameter);
        }
    }
}
