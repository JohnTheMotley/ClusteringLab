using System;
using System.Linq;
using System.Collections.Generic;

namespace ClusteringLab {
    class Program {
        static void Main(string[] args) {
            var arguments = args.ToList();

            int k = -1;
            string arffFile = null;
            bool randomInitials = false;
            List<int> columnsToIgnore = null;

            try {
                k = int.Parse(arguments.ElementAt(arguments.IndexOf(K_ARGUMENT) + 1));
                arffFile = arguments.ElementAt(arguments.IndexOf(ARFF_ARGUMENT) + 1);
                randomInitials = bool.Parse(arguments.ElementAt(arguments.IndexOf(RANDOM_INITIAL_CLUSTERS_ARGUMENT) + 1));

                columnsToIgnore = new List<int>();
                if (arguments.Contains(IGNORED_COLUMNS_ARGUMENT)) {
                    string[] ignoredColumns = arguments.ElementAt(arguments.IndexOf(IGNORED_COLUMNS_ARGUMENT) + 1).Split(",");
                    foreach (string s in ignoredColumns) {
                        columnsToIgnore.Add(int.Parse(s));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(string.Format(ERROR_MESSAGE, ARFF_ARGUMENT, K_ARGUMENT, RANDOM_INITIAL_CLUSTERS_ARGUMENT, IGNORED_COLUMNS_ARGUMENT, ex.Message));
                Environment.Exit(2);
            }

            var stream = new System.IO.FileStream(arffFile, System.IO.FileMode.Open);
            var relation = ArffParser.ArffReader.LoadArff(stream, columnsToIgnore);

            RunClusteringAlgorithm(relation, k, randomInitials);
        }

        private static void RunClusteringAlgorithm(ArffParser.ArffRelation relation, int k, bool randomInitials) {
            var clusterer = new Clusterer.KClusterer(k, relation, randomInitials);
            double squaredError = clusterer.TotalSquaredError();

            clusterer.ReCluster();
            double nextSquaredError = clusterer.TotalSquaredError();
            while (nextSquaredError != squaredError) {
                clusterer.ReCluster();
                squaredError = nextSquaredError;
                nextSquaredError = clusterer.TotalSquaredError();
            }
        }

        private static string ERROR_MESSAGE = "There was an error parsing the command line inputs. Please include the following arguments: {0} <Arff File Location> {1} <K for Clustering> {2} <Use random initial cluster locations>\nOptional Parameters: {3} <Column indices to ignore>\nInternal Error: {4}";
        private static string ARFF_ARGUMENT = "--arffFile";
        private static string K_ARGUMENT = "--k";
        private static string RANDOM_INITIAL_CLUSTERS_ARGUMENT = "--randomInitials";
        private static string IGNORED_COLUMNS_ARGUMENT = "--ignoredColumns";
    }
}
