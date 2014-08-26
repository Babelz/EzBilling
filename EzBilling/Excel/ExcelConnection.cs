using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace EzBilling.Excel
{
    public sealed class ExcelConnection 
    {
        #region Static vars
        private static readonly string tempWorkBookPath;
        #endregion

        #region Vars
        private Application application;
        private Workbook workbook;
        #endregion

        #region Properties
        public bool Connected
        {
            get
            {
                return application != null;
            }
        }
        #endregion

        static ExcelConnection()
        {
            tempWorkBookPath = AppDomain.CurrentDomain.BaseDirectory + @"\Files\TempBill.xlsx";
        }

        public ExcelConnection()
        {
        }

        private bool ExcelInstalled()
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");

            return officeType != null;
        }

        public void Open()
        {
            if (ExcelInstalled())
            {
                application = new Application();
            }
        }
        public Worksheet GetWorksheet()
        {
            // Should not be null if we are connected.
            if (application != null)
            {
                if (workbook == null)
                {
                    workbook = application.Workbooks.Open(tempWorkBookPath);
                }
            }

            return workbook.ActiveSheet;
        }
        public void ResetWorksheet()
        {
            Close();
            Open();
        }
        public void Close()
        {
            if (application == null)
            {
                return;
            }

            Marshal.ReleaseComObject(application);

            application = null;

            if (workbook != null)
            {
                workbook.Close(XlSaveAction.xlDoNotSaveChanges);
                Marshal.ReleaseComObject(workbook);

                workbook = null;
            }
        }
    }
}
