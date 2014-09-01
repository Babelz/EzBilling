using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EzBilling.Database;
using EzBilling.Models;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Security.AccessControl;
using System.Reflection;

namespace EzBilling.Excel
{
    public sealed class BillWriter
    {
        #region Vars
        private readonly FileManager fileManager;
        private readonly List<object> billObjects;
        private readonly string billsDirectory;
        private readonly string billName;
        #endregion

        public BillWriter(Company company, Client client, Bill bill, string billsDirectory)
        {
            fileManager = new FileManager();

            // Get objects associated with the bill and
            // insert them to same list so writing to the excel file is easier.
            billObjects = new List<object>()
            {
                company, client, bill
            };

            for (int i = 0; i < bill.Products.Count; i++)
            {
                billObjects.Add(bill.Products[i]);
            }

            billObjects = billObjects.OrderBy(o => o.GetType().Name).ToList();
            
            billName = bill.Name;

            this.billsDirectory = billsDirectory;
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
                    int row = type == typeof(Product) ? productIndex : coordinates[j].Row;

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

                if (type == typeof(Product))
                {
                    productIndex++;
                }
            }
        }
        public void SaveBillAsPDF(Worksheet worksheet)
        {
            string directory =  string.Format("{0}{1}\\", billsDirectory, billName);
            string filename = string.Format("{0}{1}.pdf", billsDirectory, billName);
            string fullPath = directory + filename;

            fileManager.CreateDirectoryIfDoesNotExist(directory);
            fileManager.CreateFileIfDoesNotExist(fullPath);

            worksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, fullPath);
        }
    }
}
