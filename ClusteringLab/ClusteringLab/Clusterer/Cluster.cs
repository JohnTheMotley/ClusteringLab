using System;
using System.Collections.Generic;
using System.Text;
using ClusteringLab.ArffParser;

namespace ClusteringLab.Clusterer {
    public class Cluster {
        List<ArffRow> _points;
        private Dictionary<ArffRow, double> _distances;

        public ArffRow Position { get; set; }

        public Cluster(ArffRow position) {
            Position = position;
            _points = new List<ArffRow>();
            _distances = new Dictionary<ArffRow, double>();
        }

        public void ClearPoints() {
            _points.Clear();
            _distances.Clear();
        }

        public double GetDistance(ArffRow from) {
            if (!_distances.ContainsKey(from)) {
                _distances.Add(from, Position.GetDistance(from));
            }
            return _distances[from];
        }

        public ArffRow GetAveragePositionOfPoints() {
            ArffRow average = Position.Average(_points);
            return average;
        }

        public void AddPoint(ArffRow toAdd) {
            _points.Add(toAdd);
        }

        public double SumSquaredError() {
            double sumSquaredError = 0;

            foreach (ArffRow row in _points) {
                sumSquaredError += Math.Pow(GetDistance(row), 2);
            }

            return sumSquaredError;
        }

        public StringBuilder Data() {
            var builder = new StringBuilder();
            builder.Append(string.Format(CENTROID_MESSAGE, Position.Data()) + "\n");

            var dataBuilder = new StringBuilder();
            foreach (var row in _points) {
                dataBuilder.Append("\t" + row.Data() + "\n");
            }
            builder.Append("Data Points: {\n");
            builder.Append(dataBuilder);
            builder.Append("}");

            return builder;
        }

        private static string CENTROID_MESSAGE = "Centroid: {0}";
    }
}
