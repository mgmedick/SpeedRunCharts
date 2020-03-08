using System.Collections.Generic;

namespace SpeedrunComSharp.Model
{
    public class CachedEnumerable<T> : IEnumerable<T>
    {
        IEnumerable<T> baseEnumerable;
        IEnumerator<T> baseEnumerator;
        List<T> cachedElements;

        public CachedEnumerable(IEnumerable<T> baseEnumerable)
        {
            this.baseEnumerable = baseEnumerable;
            cachedElements = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var element in cachedElements)
                yield return element;

            if (baseEnumerator == null)
                baseEnumerator = baseEnumerable.GetEnumerator();

            while (baseEnumerator.MoveNext())
            {
                var current = baseEnumerator.Current;
                cachedElements.Add(current);
                yield return current;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
