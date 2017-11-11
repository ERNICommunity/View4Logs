using System;
using System.Collections.Generic;
using System.Runtime;

namespace View4Logs.Utils.Collections
{
    /// <summary>
    /// Basically it's a List which only allows appending element and creating "snapshots".  
    /// Snapshot is ArraySegment which points to internal array. This means there is no copying of items. 
    /// </summary>
    public sealed class AppendBuffer<T>
    {
        private const int DefaultCapacity = 4;
        private const int MaxArrayLength = 0X7FEFFFFF;

        static readonly T[] EmptyArray = new T[0];

        private T[] _items;

        public AppendBuffer()
        {
        }

        public AppendBuffer(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Capacity must be positive number.", nameof(capacity));
            }

            if (capacity == 0)
            {
                _items = EmptyArray;
            }
            else
            {
                _items = new T[capacity];
            }
        }

        public int Count { get; private set; }


        public int Capacity
        {
#if !FEATURE_CORECLR
            [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
#endif
            get
            {
                return _items.Length;
            }
            private set
            {
                if (value < Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be smaller than current count of items");
                }

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (Count > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, Count);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = EmptyArray;
                    }
                }
            }
        }

        public void Add(T item)
        {
            if (Count == _items.Length)
            {
                EnsureCapacity(Count + 1);
            }

            _items[Count++] = item;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }
        
        public IList<T> Snapshot()
        {
            return new ArraySegment<T>(_items, 0, Count);
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
                if ((uint) newCapacity > MaxArrayLength)
                {
                    newCapacity = MaxArrayLength;
                }

                if (newCapacity < min)
                {
                    newCapacity = min;
                }

                Capacity = newCapacity;
            }
        }
    }
}
