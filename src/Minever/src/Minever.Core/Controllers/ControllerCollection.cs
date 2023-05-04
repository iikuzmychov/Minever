using Minever.Core.Controllers;
using System.Collections;

namespace Minever.Core;

public class ControllerCollection : IControllerCollection
{
    private readonly Dictionary<Type, IController> _baseDictionary = new();

    public int Count => _baseDictionary.Count;
    bool ICollection<ControllerDescriptor>.IsReadOnly => false;

    public void Add(IController item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _baseDictionary.Add(item.GetType(), item);
    }

    public void Clear() => _baseDictionary.Clear();

    public bool Contains(IController item) => _baseDictionary.ContainsValue(item);

    public void CopyTo(IController[] array, int arrayIndex) => _baseDictionary.Values.CopyTo(array, arrayIndex);

    public bool Remove(IController item) => _baseDictionary.Remove(_baseDictionary.GetType());

    public IEnumerator<IController> GetEnumerator() => _baseDictionary.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _baseDictionary.Values.GetEnumerator();
}
