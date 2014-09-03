using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EzBilling.Excel
{
    public sealed class CellCoordinateReader
    {
        #region Constants
        private const string COMMENT = "--";
        #endregion

        #region Static vars
        private static readonly string cellCoordsPath;
        #endregion

        static CellCoordinateReader()
        {
            cellCoordsPath = AppDomain.CurrentDomain.BaseDirectory + @"Files\cellcoords.txt";
        }

        public CellCoordinateReader()
        {
        }

        public List<CellCoordinate> ReadCoordinates()
        {
            List<CellCoordinate> coordinates = new List<CellCoordinate>();

            // Remove comments and trim lines.
            string[] lines = File.ReadLines(cellCoordsPath)
                .Where(l => !l.StartsWith(COMMENT))
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Trim())
                .ToArray();

            char[] splitTokens = new char[] { ' ' };

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);

                string containingObjectName = tokens[0].Replace(":", "").Split('.').First().Trim();
                string name = tokens[0].Replace(containingObjectName + ".", "").Replace(":", "").Trim();
                string column = tokens[1].Trim();
                int row = int.Parse(tokens[2].Trim());

                coordinates.Add(new CellCoordinate(containingObjectName, name, column, row));
            }

            return coordinates.OrderBy(o => o.ContainingObjectName).ToList();
        }
    }
}
