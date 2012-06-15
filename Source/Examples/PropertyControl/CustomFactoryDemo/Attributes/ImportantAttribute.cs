namespace CustomFactoryDemo
{
    using System;

    /// <summary>
    /// Specifies that the decorated property is important.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ImportantAttribute : Attribute
    {
    }
}