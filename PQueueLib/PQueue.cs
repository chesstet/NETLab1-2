using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PQueueLib.EventArgs;
using PQueueLib.Extensions;

namespace PQueueLib
{
    public class PQueue<T> : IEnumerable<T>, ICollection, IReadOnlyCollection<T>
    {
        public event EventHandler<PQueueEventArgs> Cleared; 
        public event EventHandler<PQueueElemEventArgs<T>> Added; 
        public event EventHandler<PQueueEventArgs> Removed; 

        private T[] _array;
        private int _head;
        private int _size;
        private int _tail;
        public PQueue() => _array = Array.Empty<T>();
        public PQueue(int capacity) => _array = capacity >= 0 ? new T[capacity] : throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be equal or more than 0");

        private void CallEvent<TEventArgs>(TEventArgs eventArgs, EventHandler<TEventArgs> eventHandler) where TEventArgs : System.EventArgs
        {
            if (eventArgs != null)
                eventArgs.Raise(this, ref eventHandler);
        }

        protected virtual void OnCleared(PQueueEventArgs eventArgs)
        {
            CallEvent(eventArgs, Cleared);
        }

        protected virtual void OnAdded(PQueueElemEventArgs<T> eventArgs)
        {
            CallEvent(eventArgs, Added);
        }
        protected virtual void OnRemoved(PQueueEventArgs eventArgs)
        {
            CallEvent(eventArgs, Removed);
        }

        public int Count => _size;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>)new PQueue<T>.Enumerator(this);
        public IEnumerator GetEnumerator() => new PQueue<T>.Enumerator(this);

        public T Peek()
        {
            if (_size == 0)
                throw new InvalidOperationException("Array is empty");
            return _array[_head];
        }

        public T Dequeue()
        {
            if (_size == 0)
                throw new InvalidOperationException("Array is empty");
            T obj = _array[_head];
            MoveNext(ref _head);
            --_size;
            OnRemoved(new PQueueEventArgs("The first element was deleted from the queue"));
            return obj;
        }

        public void Enqueue(T item)
        {
            if (_size == _array.Length)
                SetCapacity(_array.Length * 2 < _array.Length + 4 ? _array.Length + 4 : _array.Length * 2);

            _array[_tail] = item;
            MoveNext(ref _tail);
            ++_size;
            OnAdded(new PQueueElemEventArgs<T>("The element was added into the queue", item));
        }

        public bool Contains(T item)
        {
            if (_size == 0)
                return false;
            if (_head < _tail)
                return Array.IndexOf(_array, item, _head, _size) >= 0;
            return Array.IndexOf(_array, item, _head, _array.Length - _head) >= 0 || Array.IndexOf(_array, item, 0, _tail) >= 0;
        }

        public void Clear()
        {
            if (_size != 0)
            {
                if (_head < _tail)
                    Array.Clear(_array, _head, _size);
                else
                {
                    Array.Clear(_array, _head, _array.Length - _head);
                    Array.Clear(_array, 0, _tail);
                }
                _size = 0;
            }
            _head = 0;
            _tail = 0;
            OnCleared(new PQueueEventArgs("The queue is cleared"));
        }

        private void SetCapacity(int capacity)
        {
            T[] objArray = new T[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, objArray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, objArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, objArray, _array.Length - _head, _tail);
                }
            }
            _array = objArray;
            _head = 0;
            _tail = _size == capacity ? 0 : _size;
        }

        private void MoveNext(ref int index) => index = index + 1 == _array.Length ? 0 : index + 1;

        void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;

        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {

            private readonly PQueue<T> _q;
            private int _index;
            private T _currentElement;

            internal Enumerator(PQueue<T> q)
            {
                _q = q;
                _index = -1;
                _currentElement = default;
            }

            public void Dispose()
            {
                _index = -2;
                _currentElement = default;
            }

            public bool MoveNext()
            {
                if (_index == -2)
                    return false;
                ++_index;
                if (_index == _q._size)
                {
                    _index = -2;
                    _currentElement = default;
                    return false;
                }
                int length = _q._array.Length;
                int index = _q._head + _index;
                if (index >= length)
                    index -= length;
                _currentElement = _q._array[index];
                return true;
            }

            public T Current
            {
                get
                {
                    if (_index < 0)
                        ThrowEnumerationNotStartedOrEnded();
                    return _currentElement;
                }
            }

            private void ThrowEnumerationNotStartedOrEnded() => throw new InvalidOperationException();

            object IEnumerator.Current => Current;

            void IEnumerator.Reset()
            {
                _index = -1;
                _currentElement = default;
            }
        }
    }

    
}
