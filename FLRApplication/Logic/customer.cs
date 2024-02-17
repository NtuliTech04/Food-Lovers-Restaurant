using System;
using FLRApplication.Models;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FLRApplication.Logic
{
    public class customer
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<CustomerProfile> AllCustomers()
        {
            return db.Customers.ToList();
        }

        public CustomerProfile GetCustomer(string email)
        {
            return db.Customers
                     .Where(x => x.Email == email)
                     .FirstOrDefault();


        }

        public IEnumerable<CustomerAddress> UserAddresses(string user)
        {
            return db.Addresses
                     .Where(x => x.Email == user)
                     .OrderByDescending(x => x.ID)
                     .ToList();
        }

        public CustomerAddress GetAddress(int id)
        {
            return db.Addresses
                     .Where(x => x.ID == id)
                     .FirstOrDefault();
        }


        public bool AddCustomer(CustomerProfile model)
        {
            try
            {
                db.Customers.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }


        public bool AddAddress(CustomerAddress model)
        {
            try
            {
                db.Addresses.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }


        public bool RemoveAddress(int id)
        {
            try
            {
                var address = db.Addresses.Find(id);
                db.Addresses.Remove(address);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }


        //RSAID Processing Methods
        string prefix, DOB;
        public string birthDate(string Id)
        {
            int BirthYear = int.Parse(Id.Substring(0, 2));

            string ThisYear = Convert.ToString(DateTime.Today.Year).Substring(2, 2);
            int Currentyear = int.Parse(ThisYear);

            if (BirthYear >= 00 && BirthYear < Currentyear)
            {
                prefix = "20";
            }
            else
            {
                prefix = "19";
            }

            DOB = prefix + Id.Substring(0, 2) + "/" + Id.Substring(2, 2) + "/" + Id.Substring(4, 2);

            return DOB;
        }

        public string CalcAge()
        {
            DateTime BirthDate = DateTime.ParseExact(DOB, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime thisYear = DateTime.Today;
            int age = thisYear.Year - BirthDate.Year;
            if (BirthDate > thisYear.AddYears(-age)) age--;
            string Age = age.ToString();

            return Age;
        }

        public string GetGender(string Id)
        {
            var genderCode = (Id.Substring(6, 4));
            var sex = int.Parse(genderCode) < 5000 ? "Female" : "Male";
            string gender = sex.ToString();

            return gender;
        }

        #region constants
        public const string AddressSaved = "A new address has been saved. Please check the list below.";
        public const string AddressUsed = "This address cannot be removed. It could be associated with previous order(s).";
        public const string DeletedAddress = "You have deleted one of you saved addresses.";
        #endregion
    }
}