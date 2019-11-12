using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringLab.ArffParser {
    public class ArffRow {
        private List<double> _values;
        private ArffRelation _relation;

        public ArffRow(ArffRelation relation, List<double> values) {
            _relation = relation;
            _values = values;
        }

        public ArffRow Clone() {
            return new ArffRow(_relation, new List<double>(_values));
        }

        public bool ColumnIsUnkown(int col) {
            return _values[col] == double.MaxValue;
        }

        public double GetDistance(ArffRow from) {
            double distance = 0;

            for (int col = 0; col < _values.Count; col++) {
                if (this.ColumnIsUnkown(col) || from.ColumnIsUnkown(col)) {
                    distance += 1;
                }
                else if (_relation.Columns[col].IsReal) {
                    distance += Math.Pow(from._values[col] - this._values[col], 2);
                }
                else {
                    distance += this._values[col] == from._values[col] ? 0 : 1;
                }
            }

            distance = Math.Sqrt(distance);
            return distance;
        }

        public ArffRow Average(List<ArffRow> toAverage) {
            var averages = new List<double>(_values.Count);

            for (int col = 0; col < _values.Count; col++) {
                averages.Add(0);
                if (_relation.Columns[col].IsReal) {
                    averages[col] = GetAverageForRealColumn(col, toAverage);
                }
                else {
                    averages[col] = GetModeForNominalColumn(col, toAverage);
                }
            }

            return new ArffRow(_relation, averages);
        }

        private double GetModeForNominalColumn(int col, List<ArffRow> toAverage) {
            double mode = -1;

            var nominalCounts = new Dictionary<double, int>();
            foreach (ArffRow row in toAverage) {
                // Only count known rows.
                if (!row.ColumnIsUnkown(col)) {
                    double value = row._values[col];
                    if (!nominalCounts.ContainsKey(value)) {
                        nominalCounts.Add(value, 1);
                    }
                    else {
                        int currentCount = nominalCounts[value];
                        nominalCounts[value] = currentCount + 1;
                    }
                }
            }

            int highestCount = -1;
            double highestCountKey = -1;
            foreach (double key in nominalCounts.Keys) {
                if (nominalCounts[key] > highestCount) {
                    highestCount = nominalCounts[key];
                    highestCountKey = key;
                }
                else if (nominalCounts[key] == highestCount) {
                    // In case of a tie, choose the nominal attribute first in the metadata list.
                    // This will be the lower key, because the key is effectively the index of nominal metadata.
                    if (key < highestCountKey) {
                        highestCountKey = key;
                    }
                }
            }

            mode = highestCountKey;

            return mode;
        }

        private double GetAverageForRealColumn(int col, List<ArffRow> toAverage) {
            double average = 0;

            int knownCount = 0;
            foreach (ArffRow row in toAverage) {
                // Only count known rows.
                if (!row.ColumnIsUnkown(col)) {
                    knownCount++;
                    average += row._values[col];
                }
            }
            if (knownCount == 0) {
                average = double.MaxValue;
            }
            else {
                average /= knownCount;
            }
            return average;
        }

        public StringBuilder Data() {
            var builder = new StringBuilder();
            builder.Append("{");
            for (int col = 0; col < _values.Count - 1; col++) {
                builder.Append(" " + ColumnToString(col) + ",");
            }
            builder.Append(" " + ColumnToString(_values.Count - 1) + "}");
            return builder;
        }

        private string ColumnToString(int col) {
            string toRet = "";
            if (ColumnIsUnkown(col)) {
                toRet = "?";
            }
            else if (_relation.Columns[col].IsReal) {
                toRet = _values[col].ToString();
            }
            else {
                toRet = _relation.Columns[col].NominalValues[_values[col]];
            }
            return toRet;
        }
    }
}
