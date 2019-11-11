using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringLab.ArffParser {
    public class Map<T1, T2> {
        private Dictionary<T1, T2> _forward;
        private Dictionary<T2, T1> _reverse;

        public Map() {
            _forward = new Dictionary<T1, T2>();
            _reverse = new Dictionary<T2, T1>();
        }

        public void Add(T1 first, T2 second) {
            _forward.Add(first, second);
            _reverse.Add(second, first);
        }

        public T2 this[T1 key] {
            get { return _forward[key]; }
        }
        public T1 this[T2 key] {
            get { return _reverse[key]; }
        }
    }
}
