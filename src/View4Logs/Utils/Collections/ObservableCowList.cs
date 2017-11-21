using System;
using System.Collections;
using System.Collections.Generic;

namespace View4Logs.Utils.Collections
{
    /// <summary>
    /// List implementation specialized for efficient providing of <see cref="INotifyListChanged{T}"/> interface.
    /// </summary>
    /// <remarks>
    /// Provides similar interface to <see cref="List{T}"/> but creates a new copy of internal array on write operations.
    /// Adding item(s) to end is optimized so there is no copy of internal array.
    /// Any list modification will raise appropriate <see cref="INotifyListChanged{T}.ListChanged"/> event.
    /// </remarks>
    public sealed class ObservableCowList<T> : IList<T>, INotifyListChanged<T>
    {
        private const int DefaultCapacity = 4;
        private const int MaxArrayLength = 0X7FEFFFFF;

        private static readonly T[] EmptyArray = new T[0];

        private T[] _items;
        private int _size;

        public ObservableCowList()
            : this(0)
        {
        }

        public ObservableCowList(int capacity)
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

        public event EventHandler<NotifyListChangedEventArgs<T>> ListChanged;

        public int Count => _size;

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _size)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _size)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                var newItems = new T[_items.Length];
                newItems[index] = value;

                _items = newItems;
                NotifyListReset();
            }
        }

        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be smaller than current _size of items");
                }

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
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
            if (_size == _items.Length)
            {
                EnsureCapacity(_size + 1);
            }

            _items[_size++] = item;

            var newItems = new ArraySegment<T>(_items, _size - 1, 1);
            NotifyListAdd(newItems);
        }

        public void Add(IList<T> collection)
        {
            if (_size < _items.Length + collection.Count)
            {
                EnsureCapacity(_size + collection.Count);
            }

            var oldSize = _size;

            collection.CopyTo(_items, _size);
            _size += collection.Count;

            var newItems = new ArraySegment<T>(_items, oldSize, _size - oldSize);
            NotifyListAdd(newItems);
        }

        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == _size)
            {
                Add(item);
                return;
            }

            var newItems = new T[_size + 1];
            Array.Copy(_items, 0, newItems, 0, index);
            newItems[index] = item;
            Array.Copy(_items, index, newItems, index + 1, _size - index);

            _items = newItems;
            _size = _items.Length;
            NotifyListReset();
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var newSize = _size - 1;
            var newItems = new T[newSize];
            Array.Copy(_items, 0, newItems, 0, index);
            Array.Copy(_items, index + 1, newItems, index, newSize - index);

            _items = newItems;
            _size = newSize;
            NotifyListReset();
        }

        public void Reset(IList<T> items)
        {
            var newItems = new T[items.Count];
            items.CopyTo(newItems, 0);

            _items = newItems;
            _size = _items.Length;

            NotifyListReset();
        }

        public void Clear()
        {
            if (_size > 0)
            {
                _items = EmptyArray;
                _size = 0;

                NotifyListReset();
            }
        }

        public bool Contains(T item)
        {
            return Array.IndexOf(_items, item) != -1;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IList<T> GetSnapshot()
        {
            return new ArraySegment<T>(_items, 0, _size);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetSnapshot().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > MaxArrayLength)
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

        private void NotifyListAdd(IList<T> newItems)
        {
            ListChanged?.Invoke(this, new NotifyListChangedEventArgs<T>(NotifyListChangedAction.Add, GetSnapshot(), newItems));
        }

        private void NotifyListReset()
        {
            var items = GetSnapshot();
            ListChanged?.Invoke(this, new NotifyListChangedEventArgs<T>(NotifyListChangedAction.Reset, items, items));
        }
    }
}
