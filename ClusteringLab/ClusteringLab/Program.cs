using System;

namespace ClusteringLab {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            var stream = new System.IO.FileStream(@"C:\Users\motlj\Desktop\test.arff", System.IO.FileMode.Open);
            var relation = ArffParser.ArffReader.LoadArff(stream);

            var clusterer = new Clusterer.KClusterer(3, relation);
            double squaredError = clusterer.TotalSquaredError();

            clusterer.ReCluster();
            double nextSquaredError = clusterer.TotalSquaredError();
            while (nextSquaredError < squaredError) {
                clusterer.ReCluster();
                squaredError = nextSquaredError;
                nextSquaredError = clusterer.TotalSquaredError();
            }
            int test = 0;
        }
    }
}
