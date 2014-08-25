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
                .Select(l => l.Trim())
                .Where(l => !l.StartsWith(COMMENT))
                .Where(l => !string.IsNullOrEmpty(l))
                .ToArray();

            char[] splitTokens = new char[] { '.', ' ' };

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(splitTokens);

                if (tokens.Length > 4)
                {
                    throw new ArgumentException("Invalid token count found in cell coordinates. Line is: " + lines[i]);
                }

                string containingObjectname = tokens[0].Trim();
                string name = tokens[1].Trim().Replace(":", "");
                string column = tokens[2].Trim();
                int row = int.Parse(tokens[3].Trim());

                coordinates.Add(new CellCoordinate(containingObjectname, name, column, row));
            }

            return coordinates.OrderBy(o => o.ContainingObjectName).ToList();
        }
    }
}
