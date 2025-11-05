using System;

namespace Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InitializeSystemAttribute : Attribute
    {
        public string Name;

        public InitializeSystemAttribute(string name)
        {
            Name = name;
        }
    }
}