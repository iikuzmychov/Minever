using System.Diagnostics;

namespace Minever.LowLevel.Java.Core.Extensions;

// todo: make it part of BidirectionalDictionary package
internal static class EnumerableExtensions
{
    public static IReadOnlyBidirectionalDictionary<TKey, TValue> ToReadOnlyBidirectionalDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
        where TKey : notnull
        where TValue : notnull
    {
        Debug.Assert(source is not null);
        Debug.Assert(keySelector is not null);
        Debug.Assert(elementSelector is not null);

        var bidirectionalDictionary = source.TryGetNonEnumeratedCount(out var count)
            ? new BidirectionalDictionary<TKey, TValue>(count)
            : new BidirectionalDictionary<TKey, TValue>();

        foreach (var item in source)
        {
            bidirectionalDictionary.Add(keySelector(item), elementSelector(item));
        }

        return bidirectionalDictionary.AsReadOnly();
    }
}
