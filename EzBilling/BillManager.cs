using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using EzBilling.Database;
using EzBilling.Excel;
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
            string billName = string.Format("{0} - {1}", clientName, DateTime.Now.Date.ToString("MM/dd/yyyy"));
            string fileName = billName + ".pdf";
            string clientBillsDirectory = string.Format(@"{0}\{1}", billsDirectory, clientName);
            string fullPath = clientBillsDirectory + fileName;

            string newBillName = string.Empty;


            fileManager.CreateDirectoryIfDoesNotExist(clientBillsDirectory);

            if (!File.Exists(fullPath) && !knowBillNames.Contains(billName))
            {
                knowBillNames.Add(billName);

                return fileName.Replace(".pdf", "");
            }
            else
            {
                int index = 1;

                while (true)
                {
                    newBillName = string.Format("{0} ({1})", billName, index);
                    string newFullPath = string.Format(@"{0}\{1}{2}.pdf", clientBillsDirectory, clientName, string.Format(" ({0})", index.ToString()));

                    fullPath = fullPath.Replace(fileName, newFullPath);
                    fileName = newFullPath;

                    if(!(File.Exists(fullPath) || knowBillNames.Contains(newBillName)))
                    {
                        knowBillNames.Add(newBillName);
                        break;
                    }

                    index++;
                }
            }

            return newBillName;
        }
        public void SaveBillAsPDF(Worksheet worksheet, CompanyInformation company, ClientInformation client, BillInformation bill)
        {
            BillWriter writer = new BillWriter(company, client, bill, billsDirectory);
            writer.Write(worksheet);
            writer.SaveBillAsPDF(worksheet);
        }
    }
}
