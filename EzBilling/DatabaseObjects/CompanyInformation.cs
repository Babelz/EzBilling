using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzBilling.DatabaseObjects
{
    public sealed class CompanyInformation : UserInformation
    {
        #region Vars
        private string accountNumber;
        private string bankName;
        private string bankBIC;
        private string billerName;
        private string phone;
        private string email;
        #endregion

        #region Properties
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
            set
            {
                accountNumber = value;
            }
        }
        public string BankName
        {
            get
            {
                return bankName;
            }
            set
            {
                bankName = value;
            }
        }
        public string BankBIC
        {
            get
            {
                return bankBIC;
            }
            set
            {
                bankBIC = value;
            }
        }
        public string BillerName
        {
            get
            {
                return billerName;
            }
            set
            {
                billerName = value;
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        #endregion

        public CompanyInformation()
            : base()
        {
        }
    }
}
