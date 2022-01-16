using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Topelab.Core.Resolver.Interfaces;

namespace Topelab.Core.Resolver.Microsoft
{
    internal class ResolverStorage<T> : IResolverStorage<T>
    {
        private readonly Dictionary<T, IResolver> resolvers = new();

        public event Action<IResolverStorage<T>, T> ValueAdded;
        public event Action<IResolverStorage<T>, T> ValueRemoved;
        public event Action<IResolverStorage<T>, T> ValueChanged;
        public event Action<IResolverStorage<T>> AllValuesRemoved;

        public IResolver this[T key]
        {
            get => resolvers[key];
            set
            {
                if (resolvers.ContainsKey(key))
                {
                    resolvers[key] = value;
                    OnValueChanged(this, key);
                }
                else
                {
                    resolvers.Add(key, value);
                }
            }
        }

        public ICollection<T> Keys { get; }
        public ICollection<IResolver> Values { get; }
        public int Count { get; }
        public bool IsReadOnly { get; }

        public void Add(T key, IResolver value)
        {
            resolvers.Add(key, value);
            OnValueAdded(this, key);
        }

        public void Add(KeyValuePair<T, IResolver> item)
        {
            resolvers.Add(item.Key, item.Value);
            OnValueAdded(this, item.Key);
        }

        public void Clear()
        {
            resolvers.Clear();
            OnAllValuesRemoved(this);
        }

        public bool Contains(KeyValuePair<T, IResolver> item)
        {
            return resolvers.Contains(item);
        }

        public bool ContainsKey(T key)
        {
            return resolvers.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<T, IResolver>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T, IResolver>>)resolvers).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<T, IResolver>> GetEnumerator()
        {
            return resolvers.GetEnumerator();
        }

        public bool Remove(T key)
        {
            var result = Remove(key);
            OnValueRemoved(this, key);
            return result;
        }

        public bool Remove(KeyValuePair<T, IResolver> item)
        {
            var result = Remove(item);
            OnValueRemoved(this, item.Key);
            return result;
        }

        public bool TryGetValue(T key, [MaybeNullWhen(false)] out IResolver value)
        {
            return resolvers.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return resolvers.GetEnumerator();
        }

        private void OnValueChanged(IResolverStorage<T> sender, T key)
        {
            ValueChanged?.Invoke(sender, key);
        }

        private void OnValueAdded(IResolverStorage<T> sender, T key)
        {
            ValueAdded?.Invoke(sender, key);
        }

        private void OnValueRemoved(IResolverStorage<T> sender, T key)
        {
            ValueRemoved?.Invoke(sender, key);
        }

        private void OnAllValuesRemoved(IResolverStorage<T> sender)
        {
            AllValuesRemoved?.Invoke(sender);
        }

    }
}
