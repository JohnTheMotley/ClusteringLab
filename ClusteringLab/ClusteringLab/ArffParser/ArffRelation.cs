using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringLab.ArffParser {
    public class ArffRelation {
        private string _name;
        private List<ArffColumn> _columns;
        private List<ArffRow> _rows;

        public ArffRelation(string name) {
            _name = name;
            _columns = new List<ArffColumn>();
            _rows = new List<ArffRow>();
        }

        internal List<ArffColumn> Columns {
            get { return _columns; }
        }

        internal void AddColumn(ArffColumn col) {
            _columns.Add(col);
        }
        
        internal void AddRow(ArffRow row) {
            _rows.Add(row);
        }
    }
}
