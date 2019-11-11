using System;

namespace ClusteringLab {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            var stream = new System.IO.FileStream(@"C:\Users\motlj\Desktop\test.arff", System.IO.FileMode.Open);
            ArffParser.ArffReader.LoadArff(stream);
        }
    }
}
