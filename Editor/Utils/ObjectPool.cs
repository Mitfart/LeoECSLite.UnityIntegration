using System;
using System.Collections.Concurrent;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Utils {
  public class ObjectPool<T> {
    private readonly Func<T>          _objectGenerator;
    private readonly ConcurrentBag<T> _objects;



    public ObjectPool(Func<T> objectGenerator) {
      _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
      _objects         = new ConcurrentBag<T>();
    }



    public T Take()
      => _objects.TryTake(out T item)
        ? item
        : _objectGenerator();

    public void Return(T item) => _objects.Add(item);
  }
}