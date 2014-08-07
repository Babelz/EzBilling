using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EzBilling.Database.Serialization;

namespace EzBilling.Database.Objects
{
    [DatabaseObjectAttribute("Companies", "Company")]
    public sealed class CompanyInformation
    {
        #region Properties
        [DatabaseObjectField]
        public string Name
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string Street
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string City
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string PostalCode
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string CompanyID
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string BankAccountNumber
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string BankName
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string BankBIC
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string BillerName
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string PhoneNumber
        {
            get;
            set;
        }
        [DatabaseObjectField]
        public string EmailAddress
        {
            get;
            set;
        }
        #endregion

        public CompanyInformation()
        {
            Name = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
            CompanyID = string.Empty;
            BankAccountNumber = string.Empty;
            BankName = string.Empty;
            BankBIC = string.Empty;
            BillerName = string.Empty;
            PhoneNumber = string.Empty;
            EmailAddress = string.Empty;
        }
    }
}
