using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.Common
{
    public class HashMap<K, V> : Dictionary<K, V> //where V : Object
    {

        public HashMap()
            : base()
        {
        }

        public HashMap(int cap)
            : base(cap)
        {
        }

        public new virtual V this[K k]
        {
            get
            {
                V v = default(V);
                TryGetValue(k,out v);
                return v;

            }
            set
            {
                base[k] = value;
            }
        }

        public virtual bool Get(K k, out V v)
        {
            return TryGetValue(k, out v);
        }

        public virtual V Get(K k)
        {
            return base[k];
        }
    }
}
