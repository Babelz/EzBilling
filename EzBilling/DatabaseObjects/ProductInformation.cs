using System;
using System.Collections.Generic;
using System.Data;


namespace EzBilling.DatabaseObjects
{
    public sealed class ProductInformation : DatabaseObject
    {
        #region Vars
        private string name;
        private string quantity;
        private string unit;
        private string unitPrice;
        private string vatPercent;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
            }
        }
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
            }
        }
        public string UnitPrice
        {
            get
            {
                return unitPrice;
            }
            set
            {
                unitPrice = decimal.Parse(value).ToString("0.00");
            }
        }
        public string VATPercent
        {
            get
            {
                return vatPercent;
            }
            set
            {
                vatPercent = value;
            }
        }
        public string VATAmount
        {
            get
            {
                decimal total = decimal.Parse(unitPrice) * decimal.Parse(quantity);
                decimal onePercent =  total / 100.0m;

                return (decimal.Parse(vatPercent) * onePercent).ToString("0.00");
            }
        }
        public string Total
        {
            get
            {
                return (decimal.Parse(unitPrice) * decimal.Parse(quantity) + decimal.Parse(VATAmount)).ToString("0.00");
            }
        }
        #endregion

        public ProductInformation()
        {
        }

        public override void Fill(DataRow info)
        {
            ID = info["product_id"].ToString();
            Quantity = info["quantity"].ToString();
            Unit = info["unit"].ToString();
            UnitPrice = info["unit_price"].ToString();
            VATPercent = info["vat"].ToString();
            Name = info["name"].ToString();
        }
    }
}
