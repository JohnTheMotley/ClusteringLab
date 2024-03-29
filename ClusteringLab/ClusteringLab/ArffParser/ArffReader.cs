﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ClusteringLab.ArffParser {
    public static class ArffReader {
        public static ArffRelation LoadArff(FileStream fs, List<int> columnsToIgnore) {
            var reader = new StreamReader(fs);
            ArffRelation toReturn = null;
            bool notInDataSection = true;

            int currentColumn = 0;
            while (!reader.EndOfStream && notInDataSection) {
                string line = reader.ReadLine();

                if (line.ToLower().StartsWith("@relation")) {
                    toReturn = LoadRelation(line);
                }
                else if (line.ToLower().StartsWith("@attribute")) {
                    if (!columnsToIgnore.Contains(currentColumn)) {
                        AddRelationAttribute(toReturn, line);
                    }
                    currentColumn++;
                }
                else if (line.ToLower().StartsWith("@data")) {
                    notInDataSection = false;
                }
            }

            // Reading the actual data.
            while (!reader.EndOfStream) {
                string line = reader.ReadLine();

                // % starts a comment line in arff files.
                if (!line.StartsWith("%")) {
                    AddRelationRow(toReturn, line, columnsToIgnore);
                }
            }

            return toReturn;
        }

        private static ArffRelation LoadRelation(string relationLine) {
            string name = relationLine.Split(' ')[1];
            return new ArffRelation(name);
        }

        private static void AddRelationAttribute(ArffRelation relation, string attribute) {
            string[] attributeValues = new string[3];
            attributeValues[0] = attribute.Substring(0, attribute.IndexOfAny(new char[] { ' ', '\t' }));
            attribute = attribute.Remove(0, attributeValues[0].Length).Trim();
            attributeValues[1] = attribute.Substring(0, attribute.IndexOfAny(new char[] { ' ', '\t' }));
            attribute = attribute.Remove(0, attributeValues[1].Length).Trim();
            attributeValues[2] = attribute;

            string name = attributeValues[1];
            bool isReal = true;
            Map<double, string> nominals = null;

            if (attributeValues[2].ToLower() != "real" && attributeValues[2].ToLower() != "continuous") {
                isReal = false;
                nominals = new Map<double, string>();
                string[] nominalValues = attributeValues[2].Trim(new char[] { '{', '}' }).Split(',');
                for (int i = 0; i < nominalValues.Length; i++) {
                    nominals.Add(i, nominalValues[i].Trim());
                }
            }            

            var column = new ArffColumn(name, isReal, nominals);
            relation.AddColumn(column);
        }

        private static void AddRelationRow(ArffRelation relation, string row, List<int> ignoredCols) {
            string[] values = row.Split(',');
            List<double> rowValues = new List<double>();

            int effectiveCol = 0;
            for (int i = 0; i < values.Length; i++) {
                if (!ignoredCols.Contains(i)) {
                    if (values[i] == "?") {
                        rowValues.Add(double.MaxValue);
                    }
                    else if (relation.Columns[effectiveCol].IsReal) {
                        rowValues.Add(double.Parse(values[i]));
                    }
                    else {
                        rowValues.Add(relation.Columns[effectiveCol].NominalValues[values[i]]);
                    }
                    effectiveCol++;
                }
            }

            var toAdd = new ArffRow(relation, rowValues.ToList());
            relation.AddRow(toAdd);
        }
    }
}
