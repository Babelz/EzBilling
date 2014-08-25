using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EzBilling.DatabaseObjects;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Security.AccessControl;
using System.Reflection;

namespace EzBilling.Excel
{
    public sealed class BillWriter
    {
        #region Static vars
        private static readonly string billsPath;
        #endregion

        #region Constants
        private const int MAX_PRODUCTS = 20;
        #endregion

        #region Vars
        private readonly List<DatabaseObject> billObjects;
        #endregion

        static BillWriter()
        {
            billsPath = AppDomain.CurrentDomain.BaseDirectory + @"Bills\";

            if (!Directory.Exists(billsPath))
            {
                Directory.CreateDirectory(billsPath);
            }
        }

        public BillWriter(CompanyInformation company, ClientInformation client, BillInformation bill)
        {
            billObjects = new List<DatabaseObject>()
            {
                company, client, bill
            };

            for (int i = 0; i < bill.Products.Count; i++)
            {
                billObjects.Add(bill.Products[i]);
            }

            billObjects = billObjects.OrderBy(o => o.GetType().Name).ToList();
        }

        private void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir, new DirectorySecurity(dir, AccessControlSections.All));
            }
        }
        private void CreateFile(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }
        }

        private void WriteToCell(Worksheet worksheet, string value, int row, string column)
        {
            worksheet.Cells[row, column] = value;
        }

        public void Write(Worksheet worksheet)
        {
            int productIndex = 17;
            CellCoordinateReader coordinateReader = new CellCoordinateReader();
            List<CellCoordinate> coordinates = coordinateReader.ReadCoordinates();

            for (int i = 0; i < billObjects.Count; i++)
            {
                Type type = billObjects[i].GetType();
                string name = type.Name;

                int start = coordinates.IndexOf(coordinates.First(c => c.ContainingObjectName == name));

                for (int j = start; j < coordinates.Count; j++)
                {
                    int row = type == typeof(ProductInformation) ? productIndex : coordinates[j].Row;

                    // Get the property that matches cell coordinates name property.
                    PropertyInfo property = type.GetProperty(coordinates[j].Name);

                    // Write the propertys value to a cell at given coordinates.
                    WriteToCell(worksheet, property.GetValue(billObjects[i], null).ToString(), row, coordinates[j].Column);

                    int next = j + 1;

                    if (next < coordinates.Count && coordinates[next].ContainingObjectName == name)
                    {
                        continue;
                    }

                    break;
                }

                if (type == typeof(ProductInformation))
                {
                    productIndex++;
                }
            }
        }

        public void SaveAsPDF(Worksheet worksheet, string clientName)
        {
            string dir =  string.Format("{0}{1}\\", billsPath, clientName);
            string file = string.Format("{1} - {2}.pdf", billsPath, clientName, DateTime.Now.Date.ToString("MM/dd/yyyy"));
            string fullPath = dir + file;

            CreateDirectory(dir);
            CreateFile(fullPath);

            worksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, fullPath);
        }
    }
}
