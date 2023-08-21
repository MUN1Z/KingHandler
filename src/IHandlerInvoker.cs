using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace KingHandler
{
    /// <summary>
    /// Provides methods to invoke a handler action.
    /// </summary>
    public interface IHandlerInvoker
    {
        /// <summary>
        /// Invokes a handler action.
        /// </summary>
        /// <param name="scope">Handler scope.</param>
        /// <param name="handlerAction">Handler action.</param>
        /// <param name="args">Handler action parameters.</param>
        /// <returns>Handler action result.</returns>
        object Invoke(IServiceScope scope, object handlerAction, params object[] args);

        /// <summary>
        ///  Invokes a handler action async.
        /// </summary>
        /// <param name="scope">Handler scope.</param>
        /// <param name="handlerAction">Handler action.</param>
        /// <param name="args">Handler action parameters.</param>
        Task InvokeAsync(IServiceScope scope, object handlerAction, params object[] args);
    }
}
