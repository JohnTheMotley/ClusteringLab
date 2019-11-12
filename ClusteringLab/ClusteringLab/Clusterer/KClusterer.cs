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

            // Assigning points to initial clusters.
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

            return clusters;
        }


    }
}
