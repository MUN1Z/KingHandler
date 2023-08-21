using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KingHandler.Internal.Transformers
{
    /// <summary>
    /// Provides methods to create a parameter dynamicaly.
    /// </summary>
    internal interface IParameterFactory
    {
        /// <summary>
        /// Creates a new parameter instance based on the given type information.
        /// </summary>
        /// <param name="scope">calling scope</param>
        /// <param name="type">Type information.</param>
        /// <returns>New instance as <see cref="object"/>.</returns>
        object Create(IServiceScope scope, TypeInfo type);
    }
}
