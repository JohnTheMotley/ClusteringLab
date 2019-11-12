using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        public List<ArffColumn> Columns {
            get { return _columns; }
        }

        public List<ArffRow> Rows {
            get { return _rows; }
        }

        public void AddColumn(ArffColumn col) {
            _columns.Add(col);
        }
        
        public void AddRow(ArffRow row) {
            _rows.Add(row);
        }

        public void Normalize() {
            Rows[0].Normalize(Rows);
        }
    }
}
