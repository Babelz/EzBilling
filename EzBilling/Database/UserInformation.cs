using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzBilling.Database
{
    public abstract class UserInformation : DatabaseObject
    {
        #region Vars
        private string name;
        private string street;
        private string city;
        private string postalCode;
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
        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
            }
        }
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        public string PostalCode
        {
            get
            {
                return postalCode;
            }
            set
            {
                postalCode = value;
            }
        }
        #endregion

        public UserInformation()
            : base()
        {
        }
    }
}
