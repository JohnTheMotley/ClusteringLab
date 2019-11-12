using System;
using System.Collections;
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

        public bool ContainsKey(T1 key) {
            return _forward.ContainsKey(key);
        }
        public bool ContainsKey(T2 key) {
            return _reverse.ContainsKey(key);
        }

        public void Add(T1 first, T2 second) {
            _forward.Add(first, second);
            _reverse.Add(second, first);
        }

        public T2 this[T1 key] {
            get { return _forward[key]; }
            set { _forward[key] = value; }
        }
        public T1 this[T2 key] {
            get { return _reverse[key]; }
            set { _reverse[key] = value; }
        }
    }
}
