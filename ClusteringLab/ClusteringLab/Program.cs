using System;
using System.Linq;

namespace ClusteringLab {
    class Program {
        static void Main(string[] args) {
            var arguments = args.ToList();

            int k = -1;
            string arffFile = null;
            bool randomInitials = false;

            try {
                k = int.Parse(arguments.ElementAt(arguments.IndexOf("--k") + 1));
                arffFile = arguments.ElementAt(arguments.IndexOf("--arffFile") + 1);
                randomInitials = bool.Parse(arguments.ElementAt(arguments.IndexOf("--randomInitials") + 1));
            }
            catch (Exception ex) {
                Console.WriteLine(string.Format(ERROR_MESSAGE, ARFF_ARGUMENT, K_ARGUMENT, RANDOM_INITIAL_CLUSTERS_ARGUMENT, ex.Message));
                Environment.Exit(2);
            }

            var stream = new System.IO.FileStream(arffFile, System.IO.FileMode.Open);
            var relation = ArffParser.ArffReader.LoadArff(stream);

            var clusterer = new Clusterer.KClusterer(k, relation, randomInitials);
            double squaredError = clusterer.TotalSquaredError();

            clusterer.ReCluster();
            double nextSquaredError = clusterer.TotalSquaredError();
            while (nextSquaredError < squaredError) {
                clusterer.ReCluster();
                squaredError = nextSquaredError;
                nextSquaredError = clusterer.TotalSquaredError();
            }
        }

        private static string ERROR_MESSAGE = "There was an error parsing the command line inputs. Please include the following arguments: {0} <Arff File Location> {1} <K for Clustering> {2} <Use random initial cluster locations>\nInternal Error: {3}";
        private static string ARFF_ARGUMENT = "--arffFile";
        private static string K_ARGUMENT = "--k";
        private static string RANDOM_INITIAL_CLUSTERS_ARGUMENT = "--randomInitial";
    }
}
