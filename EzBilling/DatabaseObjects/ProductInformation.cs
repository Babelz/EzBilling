using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EzBilling.DatabaseObjects
{
    public sealed class ProductInformation : DatabaseObject
    {

        #region Properties

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Quantity (float)
        /// </summary>
        public string Quantity
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        /// <summary>
        /// Vat percent (float)
        /// </summary>
        public string VatPercent
        {
            get;
            set;
        }

        /// <summary>
        /// Unit price described by cents (eg. 150)
        /// </summary>
        public string UnitPrice
        {
            get;
            set;
        }

        #endregion

        public override void Fill(DataRow info)
        {
            ID = info["product_id"].ToString();
            Quantity = info["quantity"].ToString();
            Unit = info["unit"].ToString();
            UnitPrice = info["unit_price"].ToString();
            VatPercent = info["vat"].ToString();
            Name = info["name"].ToString();
        }
    }
}
