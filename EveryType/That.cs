using Extensions;

namespace EveryType;

/// <summary>
/// The <c>That</c> class provides facilities for obtaining the <see cref="Type"/>s in the current <see cref="AppDomain"/>
/// that satisfy an arbitrary predicate.
/// </summary>
public class That
{
    private static readonly ISet<Type> ConcreteTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .WhereNot(type => type.IsInterface || type.IsAbstract)
        .ToHashSet();
    
    /// <summary>
    /// Provides the <see cref="Type"/>s in the current <see cref="AppDomain"/> that are annotated by <typeparamref
    /// name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">The <see cref="Type"/> of <see cref="Attribute"/> that annotates the queried types.</typeparam>
    /// <returns>A collection of <see cref="Type"/>s.</returns>
    public static IEnumerable<Type> HasAttribute<TAttribute>() where TAttribute : Attribute
    {
        return ConcreteTypes.Where(type => type.HasCustomAttribute<TAttribute>());
    }

    /// <summary>
    /// Provides the <see cref="Type"/>s in the current <see cref="AppDomain"/> that implement or extend <typeparamref
    /// name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">The <see cref="Type"/> that the queried types implement.</typeparam>
    /// <returns>A collection of <see cref="Type"/>s.</returns>
    public static IEnumerable<Type> IsAssignableTo<TInterface>()
    {
        return IsAssignableTo(typeof(TInterface));
    }

    /// <summary>
    /// Provides the <see cref="Type"/>s in the current <see cref="AppDomain"/> that implement or extend the specified
    /// <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> that the queried types implement.</param>
    /// <returns>A collection of <see cref="Type"/>s.</returns>
    public static IEnumerable<Type> IsAssignableTo(Type type)
    {
        return ConcreteTypes.Where(candidate => candidate.IsAssignableTo(type));
    }

    /// <summary>
    /// Provides the <see cref="Type"/>s in the current <see cref="AppDomain"/> that satisfy the specified <paramref
    /// name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A function from <see cref="Type"/> to <see cref="bool"/>.</param>
    /// <returns>A collection of <see cref="Type"/>s.</returns>
    public static IEnumerable<Type> Satisfies(Func<Type, bool> predicate)
    {
        return ConcreteTypes.Where(predicate);
    }
}