using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using EzBilling.Database;
using EzBilling.Excel;
using EzBilling.Models;
using Microsoft.Office.Interop.Excel;

namespace EzBilling
{
    public sealed class BillManager
    {
        #region Static vars
        private static readonly string billsDirectory;
        #endregion

        #region Vars
        private readonly FileManager fileManager;
        private readonly List<string> knowBillNames;
        #endregion

        static BillManager()
        {
            billsDirectory = AppDomain.CurrentDomain.BaseDirectory + "Bills";

            if (!Directory.Exists(billsDirectory))
            {
                Directory.CreateDirectory(billsDirectory);
            }
        }

        public BillManager()
        {
            fileManager = new FileManager();
            knowBillNames = new List<string>();
        }

        public string CreateBillName(string clientName)
        {
            // Name for the bill.
            string billName = string.Format("{0} - {1}", clientName, DateTime.Now.Date.ToString("MM/dd/yyyy"));
            // Filename.
            string fileName = billName + ".pdf";
            // Clients bills dir.
            string clientBillsDirectory = string.Format(@"{0}\{1}", billsDirectory, clientName);
            // Full dir path + filename.
            string fullPath = clientBillsDirectory + fileName;

            string newBillName = string.Empty;

            if (!File.Exists(fullPath) && !knowBillNames.Contains(billName))
            {
                knowBillNames.Add(billName);

                return billName;
            }
            else
            {
                IEnumerable<string> fileNames = Directory.GetFiles(clientBillsDirectory)
                    .Concat(knowBillNames);

                int nextEnding = fileNames.Distinct()
                    .Count(s => s.Contains(billName));

                newBillName = billName + string.Format(" ({0})", nextEnding);

                knowBillNames.Add(newBillName);
            }

            return newBillName;
        }
        public void SaveBillAsPDF(Worksheet worksheet, Company company, Client client, Bill bill)
        {
            BillWriter writer = new BillWriter(company, client, bill, billsDirectory);
            writer.Write(worksheet);
            writer.SaveBillAsPDF(worksheet);
        }
    }
}
