using JetBrains.Annotations;

namespace Radon.Common.Core.DI;

[MeansImplicitUse(ImplicitUseKindFlags.Access)]
[AttributeUsage(AttributeTargets.Class)]
public class ServiceTransientAttribute : Attribute;
