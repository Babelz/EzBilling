using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EzBilling.DatabaseObjects;

namespace EzBilling.DatabaseObjects
{
    public sealed class BillInformation : DatabaseObject
    {
        #region Properties

        public int Company
        {
            get;
            set;
        }

        public int Client
        {
            get;
            set;
        }

        public string Reference
        {
            get;
            set;
        }

        public DateTime DueDate
        {
            get;
            set;
        }

        public string Comments
        {
            get;
            set;
        }

        #endregion

        

        public override void Fill(DataRow info)
        {
            ID = info["bill_id"].ToString();
            Company = Convert.ToInt32(info["company"].ToString());
            Client = Convert.ToInt32(info["client"].ToString());
            Reference = info["reference"].ToString();
            DueDate = new DateTime(Convert.ToInt64(info["due_date"].ToString()));
            Comments = info["comments"].ToString();

        }
    }
}
