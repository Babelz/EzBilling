using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzBilling.Excel
{
    public sealed class CellCoordinate
    {
        #region Vars
        private readonly string containingObjectName;
        private readonly string name;
        private readonly string column;
        private readonly int row;
        #endregion

        #region Properties
        public string ContainingObjectName
        {
            get
            {
                return containingObjectName;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Column
        {
            get
            {
                return column;
            }
        }
        public int Row
        {
            get
            {
                return row;
            }
        }
        #endregion

        public CellCoordinate(string containingObjectName, string name, string column, int row)
        {
            this.containingObjectName = containingObjectName;
            this.name = name;
            this.column = column;
            this.row = row;
        }
    }
}
