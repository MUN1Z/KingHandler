using System;

namespace KingHandler.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class HandlerAttribute : Attribute
    {
    }
}
