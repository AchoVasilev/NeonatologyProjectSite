namespace Neonatology.Common.Attributes;

using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |
                AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false)]
public class NotInheritedAttribute : Attribute
{ }