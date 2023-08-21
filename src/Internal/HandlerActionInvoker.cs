using Microsoft.Extensions.DependencyInjection;
using KingHandler.Exceptions;
using KingHandler.Internal.Transformers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KingHandler.Internal
{
    /// <summary>
    /// Provides methods to invoke a handler action.
    /// </summary>
    internal sealed class HandlerActionInvoker : IHandlerInvoker
    {
        private readonly HandlerActionInvokerCache _invokerCache;
        private readonly IParameterTransformer _parameterTransformer;

        /// <summary>
        /// Creates a new <see cref="HandlerActionInvoker"/> instance.
        /// </summary>
        /// <param name="invokerCache">Handler action invoker cache.</param>
        /// <param name="parameterTransformer">Parameter transformer.</param>
        public HandlerActionInvoker(HandlerActionInvokerCache invokerCache, IParameterTransformer parameterTransformer)
        {
            _invokerCache = invokerCache;
            _parameterTransformer = parameterTransformer;
        }

        /// <inheritdoc />
        public object Invoke(IServiceScope scope, object handlerAction, params object[] args)
        {
            HandlerActionInvokerCacheEntry handlerActionInvoker = _invokerCache.GetCachedHandlerAction(handlerAction);

            if (handlerActionInvoker == null)
            {
                throw new HandlerActionNotFoundException(handlerAction);
            }

            var targetHandler = handlerActionInvoker.HandlerFactory(scope, handlerActionInvoker.HandlerType);

            if (targetHandler == null)
            {
                throw new HandlerTargetCreationFailedException(handlerActionInvoker.HandlerType);
            }

            object handlerResult = null;

            try
            {
                object[] handlerActionParameters = PrepareParameters(scope, args, handlerActionInvoker.HandlerExecutor);

                handlerResult = handlerActionInvoker.HandlerExecutor.Execute(targetHandler, handlerActionParameters);
            }
            catch
            {
                throw;
            }
            finally
            {
                handlerActionInvoker.HandlerReleaser(targetHandler);
            }

            return handlerResult;
        }

        /// <inheritdoc />
        public async Task InvokeAsync(IServiceScope scope, object handlerAction, params object[] args)
        {
            HandlerActionInvokerCacheEntry handlerActionInvoker = _invokerCache.GetCachedHandlerAction(handlerAction);

            if (handlerActionInvoker == null)
            {
                throw new HandlerActionNotFoundException(handlerAction);
            }

            var targetHandler = handlerActionInvoker.HandlerFactory(scope, handlerActionInvoker.HandlerType);

            if (targetHandler == null)
            {
                throw new HandlerTargetCreationFailedException(handlerActionInvoker.HandlerType);
            }

            try
            {
                object[] handlerActionParameters = PrepareParameters(scope, args, handlerActionInvoker.HandlerExecutor);

                await handlerActionInvoker.HandlerExecutor.ExecuteAsync(targetHandler, handlerActionParameters);
            }
            catch
            {
                throw;
            }
            finally
            {
                handlerActionInvoker.HandlerReleaser(targetHandler);
            }
        }

        /// <summary>
        /// Prepare the invoke parameters. Adds default values if a parameter is missing.
        /// </summary>
        /// <param name="originalParameters">Original parameters.</param>
        /// <param name="executor">Handler executor.</param>
        /// <returns>Handler parameters.</returns>
        private object[] PrepareParameters(IServiceScope scope, object[] originalParameters, HandlerExecutor executor)
        {
            if (!executor.MethodParameters.Any())
            {
                return null;
            }

            var parameters = new object[executor.MethodParameters.Count()];

            for (var i = 0; i < parameters.Length; i++)
            {
                ParameterInfo methodParameterInfo = executor.MethodParameters.ElementAt(i);

                if (i < originalParameters.Length)
                {
                    Type originalObjectType = originalParameters[i]?.GetType();

                    if (!methodParameterInfo.ParameterType.IsAssignableFrom(originalObjectType))
                    {
                        object transformedParameter = _parameterTransformer.Transform(scope, originalParameters[i], methodParameterInfo.ParameterType.GetTypeInfo());
                        
                        parameters[i] = transformedParameter ?? executor.GetDefaultValueForParameter(i);
                    }
                    else
                    {
                        parameters[i] = originalParameters[i];
                    }
                }
                else
                {
                    parameters[i] = executor.GetDefaultValueForParameter(i);
                }
            }

            return parameters;
        }
    }
}
