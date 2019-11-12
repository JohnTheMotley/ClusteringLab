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
            bool normalize = false;
            List<int> columnsToIgnore = null;

            try {
                k = int.Parse(arguments.ElementAt(arguments.IndexOf(K_ARGUMENT) + 1));
                arffFile = arguments.ElementAt(arguments.IndexOf(ARFF_ARGUMENT) + 1);
                randomInitials = bool.Parse(arguments.ElementAt(arguments.IndexOf(RANDOM_INITIAL_CLUSTERS_ARGUMENT) + 1));
                normalize = bool.Parse(arguments.ElementAt(arguments.IndexOf(NORMALIZE_ARGUMENT) + 1));

                columnsToIgnore = new List<int>();
                if (arguments.Contains(IGNORED_COLUMNS_ARGUMENT)) {
                    string[] ignoredColumns = arguments.ElementAt(arguments.IndexOf(IGNORED_COLUMNS_ARGUMENT) + 1).Split(",");
                    foreach (string s in ignoredColumns) {
                        columnsToIgnore.Add(int.Parse(s));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(string.Format(ERROR_MESSAGE, ARFF_ARGUMENT, K_ARGUMENT, RANDOM_INITIAL_CLUSTERS_ARGUMENT, NORMALIZE_ARGUMENT, IGNORED_COLUMNS_ARGUMENT, ex.Message));
                Environment.Exit(2);
            }

            var stream = new System.IO.FileStream(arffFile, System.IO.FileMode.Open);
            var relation = ArffParser.ArffReader.LoadArff(stream, columnsToIgnore);

            if (normalize) {
                relation.Normalize();
            }

            RunClusteringAlgorithm(relation, k, randomInitials);
        }

        private static void RunClusteringAlgorithm(ArffParser.ArffRelation relation, int k, bool randomInitials) {
            var clusterer = new Clusterer.KClusterer(k, relation, randomInitials);
            double squaredError = clusterer.TotalSquaredError();

            PrintMessage(clusterer, 1);

            clusterer.ReCluster();
            double nextSquaredError = clusterer.TotalSquaredError();
            int iteration = 2;
            PrintMessage(clusterer, iteration);
            while (nextSquaredError != squaredError) {
                iteration++;
                clusterer.ReCluster();
                squaredError = nextSquaredError;
                nextSquaredError = clusterer.TotalSquaredError();
                PrintMessage(clusterer, iteration);
            }
        }

        private static void PrintMessage(Clusterer.KClusterer clusterer, int iteration) {
            var builder = new System.Text.StringBuilder();

            builder.Append(SEPARATOR + "\n" + SEPARATOR + "\n");
            builder.Append(string.Format(ITERATION_MESSAGE, iteration) + "\n");
            builder.Append(SEPARATOR + "\n");
            builder.Append(clusterer.ClusterData() + "\n");
            builder.Append(string.Format(SSE_MESSAGE, clusterer.TotalSquaredError()) + "\n");
            builder.Append(SEPARATOR + "\n" + SEPARATOR + "\n");

            Console.WriteLine(builder.ToString());
        }

        private static string ERROR_MESSAGE = "There was an error parsing the command line inputs. Please include the following arguments: {0} <Arff File Location> {1} <K for Clustering> {2} <Use random initial cluster locations> {3} <Normalize Data>\nOptional Parameters: {4} <Column indices to ignore>\nInternal Error: {5}";
        private static string ARFF_ARGUMENT = "--arffFile";
        private static string K_ARGUMENT = "--k";
        private static string RANDOM_INITIAL_CLUSTERS_ARGUMENT = "--randomInitials";
        private static string IGNORED_COLUMNS_ARGUMENT = "--ignoredColumns";
        private static string NORMALIZE_ARGUMENT = "--normalize";

        private static string SEPARATOR = "----------";
        private static string ITERATION_MESSAGE = "Iteration: {0}";
        private static string SSE_MESSAGE = "Total SSE: {0}";
    }
}
