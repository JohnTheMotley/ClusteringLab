using System;

namespace ClusteringLab {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            var stream = new System.IO.FileStream(@"C:\Users\motlj\Desktop\test.arff", System.IO.FileMode.Open);
            var relation = ArffParser.ArffReader.LoadArff(stream);

            var clusterer = new Clusterer.KClusterer(3, relation);
        }
    }
}
