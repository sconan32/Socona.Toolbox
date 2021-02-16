using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Collections
{
    public class FixedMinHeap<TE> : IEnumerable<TE>
    {

        private static readonly int DefaultCapacity = 11;
        private object _lockObject = new object();
        private readonly OrderedBag<TE> _innerStore;
        private readonly int _maxCount;


        public FixedMinHeap(int size = 0, IComparer<TE> comparer = null)
        {
            _innerStore = new OrderedBag<TE>(comparer);
            _maxCount = size > 0 ? size : DefaultCapacity;
        }


        public int Count { get { return _innerStore.Count; } }

        public IComparer<TE> Comparer { get { return _innerStore.Comparer; } }


        public IEnumerator<TE> GetEnumerator()
        {
            return _innerStore.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _innerStore.Clear();
            }
        }

        public bool Contains(TE e)
        {
            return _innerStore.Contains(e);
        }

        public virtual bool Add(TE e)
        {
            lock (_lockObject)
            {
                if (Count < _maxCount)
                {
                    _innerStore.Add(e);
                    return true;
                }
                var min = _innerStore.GetFirst();
                if (_innerStore.Comparer.Compare(e, min) > 0)
                {
                    _innerStore.Remove(min);
                    _innerStore.Add(e);
                    return true;
                }
                return false;
            }
        }




        public TE Min => _innerStore.GetFirst();



        public virtual TE RemoveMin()
        {
            lock (_lockObject)
            {
                TE tmp = _innerStore.RemoveFirst();

                return tmp;
            }
        }


        /// <summary>
        /// Remove the element at the given position.<br/>
        /// WARNING: this method  takes O(N) time
        /// </summary>
        /// <param name="pos">the index of the element</param>
        /// <returns></returns>
        protected virtual TE RemoveAt(int pos)
        {
            lock (_lockObject)
            {
                TE tmp = default(TE);
                tmp = _innerStore.ElementAt(pos);
                _innerStore.Remove(tmp);

                return tmp;
            }
        }

        public void AddRange(IEnumerable<TE> c)
        {
            lock (_lockObject)
            {
                foreach (TE elem in c)
                {
                    Add(elem);
                }
            }
        }
    }
}
