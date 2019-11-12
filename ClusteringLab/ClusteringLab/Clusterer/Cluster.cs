using System;
using System.Collections.Generic;
using System.Text;
using ClusteringLab.ArffParser;

namespace ClusteringLab.Clusterer {
    public class Cluster {
        private ArffRow _position;
        List<ArffRow> _points;

        public Cluster(ArffRow position) {
            _position = position;
            _points = new List<ArffRow>();
        }

        public double GetDistance(ArffRow from) {
            return _position.GetDistance(from);
        }

        public ArffRow GetAveragePosition() {
            ArffRow average = _position.Average(_points);
            return average;
        }

        public void AddPoint(ArffRow toAdd) {
            _points.Add(toAdd);
        }
    }
}
