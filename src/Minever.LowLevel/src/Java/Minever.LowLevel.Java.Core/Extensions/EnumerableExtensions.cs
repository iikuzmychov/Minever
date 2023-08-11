using System.Collections.ObjectModel;

namespace Minever.LowLevel.Java.Core.Extensions;

// todo: make it part of BidirectionalDictionary package
internal static class EnumerableExtensions
{
    public static IReadOnlyBidirectionalDictionary<TKey, TValue> ToReadOnlyBidirectionalDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
        where TKey : notnull
        where TValue : notnull
    {
        var bidirectionalDictionary = new BidirectionalDictionary<TKey, TValue>();

        foreach (var item in source)
        {
            bidirectionalDictionary.Add(keySelector(item), elementSelector(item));
        }

        return bidirectionalDictionary.AsReadOnly();
    }
}
