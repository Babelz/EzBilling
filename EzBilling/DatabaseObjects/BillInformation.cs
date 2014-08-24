using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

namespace EzBilling.DatabaseObjects
{
    public sealed class BillInformation : DatabaseObject
    {
        #region Vars
        private string reference;
        private string dueDate;
        private string additionalInformation;
        private List<ProductInformation> products;
        #endregion

        #region Properties
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
            }
        }
        public string DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
            }
        }
        public string AdditionalInformation
        {
            get
            {
                return additionalInformation;
            }
            set
            {
                additionalInformation = value;
            }
        }
        public List<ProductInformation> Products
        {
            get
            {
                return products;
            }
            set
            {
                products = value;
            }
        }
        public string VATAmount
        {
            get
            {
                decimal totalVATAmount = 0.0m;

                for (int i = 0; i < products.Count; i++)
                {
                    totalVATAmount += decimal.Parse(products[i].VATAmount);
                }

                return totalVATAmount.ToString("0.00");
            }
        }
        public string Total
        {
            get
            {
                decimal total = 0.0m;

                for (int i = 0; i < products.Count; i++)
                {
                    total += decimal.Parse(products[i].Total);
                }

                return total.ToString("0.00");
            }
        }
        #endregion

        public BillInformation()
        {
            products = new List<ProductInformation>();
        }

        public override void Fill(DataRow info)
        {
            throw new NotImplementedException();
        }
    }
}
