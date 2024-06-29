using System;
namespace api_lrpd.Util.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class SingletonAttribute : System.Attribute
    {
        
    }
}