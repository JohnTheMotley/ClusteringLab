using System;
using System.Collections.Generic;
using System.Text;
using ClusteringLab.ArffParser;

namespace ClusteringLab.Clusterer {
    public class KClusterer {
        private int _k;
        private ArffRelation _relation;
        private List<Cluster> _clusters;

        public KClusterer(int k, ArffRelation relation) {
            _k = k;
            _relation = relation;
            _clusters = GetInitialClusters();
        }

        private List<Cluster> GetInitialClusters() {
            var rand = new Random();
            var clusters = new List<Cluster>();

            // Building the initial clusters.
            var chosenRows = new List<int>();
            for (int i = 0; i < _k; i++) {
                int row = rand.Next(_relation.Rows.Count);
                while (chosenRows.Contains(row)) {
                    row = rand.Next(_relation.Rows.Count);
                }
                chosenRows.Add(row);
                clusters.Add(new Cluster(_relation.Rows[row].Clone()));
            }

            // Assigns rows to their closest clusters for initial placement.
            AssignRows(clusters);

            return clusters;
        }

        public void ReCluster() {
            foreach (Cluster c in _clusters) {
                c.Position = c.GetAveragePositionOfPoints();
                c.ClearPoints();
            }

            AssignRows(_clusters);
        }

        private void AssignRows(List<Cluster> clusters) {
            foreach (ArffRow row in _relation.Rows) {
                double closestDistance = double.MaxValue;
                Cluster closestCluster = null;
                foreach (Cluster c in clusters) {
                    double distance = c.GetDistance(row);
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestCluster = c;
                    }
                }
                closestCluster.AddPoint(row);
            }
        }

        public double TotalSquaredError() {
            double error = 0;
            foreach (Cluster c in _clusters) {
                error += c.SumSquaredError();
            }
            return error;
        }

    }
}
