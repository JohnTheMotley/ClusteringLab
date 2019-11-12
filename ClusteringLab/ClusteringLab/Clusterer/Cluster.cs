using System;
using System.Collections.Generic;
using System.Text;
using ClusteringLab.ArffParser;

namespace ClusteringLab.Clusterer {
    public class Cluster {
        List<ArffRow> _points;

        public ArffRow Position { get; set; }

        public Cluster(ArffRow position) {
            Position = position;
            _points = new List<ArffRow>();
        }

        public void ClearPoints() {
            _points.Clear();
        }

        public double GetDistance(ArffRow from) {
            return Position.GetDistance(from);
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
                sumSquaredError += Position.SquaredError(row);
            }

            return sumSquaredError;
        }
    }
}
