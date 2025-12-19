using System;

namespace ChatBot.Application.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class AutoMapAttribute : Attribute
    {
        public AutoMapAttribute(Type targetType, AutoMapDirection direction = AutoMapDirection.Both)
        {
            TargetType = targetType;
            Direction = direction;
        }

        public Type TargetType { get; }
        public AutoMapDirection Direction { get; }
    }

    public enum AutoMapDirection
    {
        To = 0,
        From = 1,
        Both = 2
    }
}
