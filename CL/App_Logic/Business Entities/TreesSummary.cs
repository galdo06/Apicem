using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using Eisk.BusinessLogicLayer;

namespace Eisk.BusinessEntities
{
    public class TreesSummary
    {
        private global::System.String _ScientificName;
        public global::System.String ScientificName
        {
            get { return _ScientificName; }
            set { _ScientificName = value; }
        }

        private global::System.String _CommonName;
        public global::System.String CommonName
        {
            get { return _CommonName; }
            set { _CommonName = value; }
        }

        private global::System.Int32 _Count;
        public global::System.Int32 Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        public TreesSummary(string ScientificName, string CommonName, int Count)
        {
            _ScientificName = ScientificName;
            _CommonName = CommonName;
            _Count = Count;
        }
    }
}
