using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringLab.ArffParser {
    public class ArffRow {
        private ArffRelation _relation;
        private List<double> _values;

        public ArffRow(ArffRelation relation, List<double> values) {
            _relation = relation;
            _values = values;
        }

        public string ColumnName(int column) {
            return _relation.Columns[column].Name;
        }
        public double ColumnValue(int column) {
            return _values[column];
        }

        public string NominalColumnValue(int column, double value) {
            string toReturn = null;
            if (!_relation.Columns[column].IsReal) {
                toReturn = _relation.Columns[column].NominalValues[value];
            }
            return toReturn;
        }
    }
}
