using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public class PropertyMap : IList<PropertyItem>
    {
        private Dictionary<string, PropertyItem> _sourceMap = new Dictionary<string, PropertyItem>();
        private List<PropertyItem> _tokens = new List<PropertyItem>();

        public PropertyItem this[string key]
        {
            get => _sourceMap[key];
            set
            {
                _sourceMap[key] = value;
                if (!_tokens.Contains(value))
                {
                    _tokens.Add(value);
                }
            }
        }

        public PropertyItem this[int index]
        {
            get => _tokens[index];
            set => _tokens[index] = value;
        }

        public int Count => _tokens.Count;

        public bool IsReadOnly => false;

        public void Add(PropertyItem item)
        {
            _sourceMap[item.Source] = item;

            _tokens.Add(item);

        }

        public PropertyItem BestMatch(string source)
        {
            PropertyItem token = null;
            if (_sourceMap.TryGetValue(source, out token))
            {
                return token;
            }

            var t = _tokens.Aggregate((t1, t2) =>
            {
                var smlr1 = t1.FuzzyMatch(source);
                var smlr2 = t2.FuzzyMatch(source);
                return smlr1 > smlr2 ? t1 : t2;
            });
            if (t.FuzzyMatch(source) > 0.5)
            {
                return t;

            }
            return null;
        }

        public void Clear()
        {
            _tokens.Clear();
            _sourceMap.Clear();
        }

        public bool Contains(PropertyItem item)
        {
            return _tokens.Contains(item);
        }

        public void CopyTo(PropertyItem[] array, int arrayIndex)
        {
            _tokens.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PropertyItem> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        public int IndexOf(PropertyItem item)
        {
            return _tokens.IndexOf(item);
        }

        public void Insert(int index, PropertyItem item)
        {
            _sourceMap[item.Source] = item;
            _tokens.Insert(index, item);

        }

        public bool Remove(PropertyItem item)
        {
            PropertyItem ditem = null;
            if (_sourceMap.TryGetValue(item.Source, out ditem))
            {
                if (item == ditem) _sourceMap.Remove(item.Source);
            }
            return _tokens.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (index > _tokens.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Remove(_tokens[index]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
