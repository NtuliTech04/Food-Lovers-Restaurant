using FLRApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FLRApplication.ViewModels
{
    public class AddressViewModel
    {
        public CustomerAddress Address { get; set; }

        public IEnumerable<CustomerAddress> AddressList { get; set; }
    }
}