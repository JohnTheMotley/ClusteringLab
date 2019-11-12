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
                if (_relation.Columns[col].IsReal) {
                    // Unknown values have a distance of one.
                    if (this.ColumnIsUnkown(col) || from.ColumnIsUnkown(col)) {
                        distance += 1;
                    }
                    else {
                        distance += Math.Pow(from._values[col] - this._values[col], 2);
                    }
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

            for (int col = 0; col < averages.Count; col++) {
                if (_relation.Columns[col].IsReal) {
                    double average = 0;
                    foreach (ArffRow row in toAverage) {
                        if (row.ColumnIsUnkown(col)) {
                            average += 1;
                        }
                        else {
                            average += row._values[col];
                        }
                    }
                    average /= toAverage.Count;
                    averages[col] = average;
                }
                else {
                    // For nominal values, get the mode rather than the average.
                    var nominalOptionCounts = new Dictionary<double, int>();
                    foreach (ArffRow row in toAverage) {
                        double key = row._values[col];
                        if (!nominalOptionCounts.ContainsKey(key)) {
                            nominalOptionCounts.Add(key, 1);
                        }
                        else {
                            nominalOptionCounts[key] += 1;
                        }
                    }
                    double highestCountKey = -1;
                    int highestCount = 0;
                    foreach (double key in nominalOptionCounts.Keys) {
                        if (nominalOptionCounts[key] > highestCount) {
                            highestCount = nominalOptionCounts[key];
                            highestCountKey = key;
                        }
                    }
                    averages[col] = highestCountKey;
                }
            }

            return new ArffRow(_relation, averages);
        }
    }
}
