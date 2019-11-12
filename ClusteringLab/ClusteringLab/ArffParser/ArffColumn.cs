using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringLab.ArffParser {
    public class ArffColumn {
        private string _name;
        private bool _isReal;
        private Map<double, string> _nominalValues;

        public ArffColumn(string name, bool isReal, Map<double, string> nominalValues) {
            _name = name;
            _isReal = isReal;
            _nominalValues = nominalValues;
        }

        public string Name { get { return _name; } }
        public bool IsReal { get { return _isReal; } }
        public Map<double, string> NominalValues { get { return _nominalValues; } }
    }
}
