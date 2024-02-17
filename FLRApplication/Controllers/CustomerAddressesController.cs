using FLRApplication.Logic;
using FLRApplication.Models;
using FLRApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FLRApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerAddressesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public customer ad = new customer();
        public AddressViewModel vm = new AddressViewModel();


        [HttpGet]
        public ActionResult AddressVM(string saved, string error, string removed)
        {
            #region Messages
            ViewBag.NewAddress = saved;
            ViewBag.AddressUsed = error;
            ViewBag.DeletedAddress = removed;
            #endregion
            AddressViewModel VModel = new AddressViewModel();

            string email = HttpContext.User.Identity.Name;
            VModel.Address = new CustomerAddress() { Email = email };
            VModel.AddressList = ad.UserAddresses(email);
            return View(VModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddressVM(AddressViewModel VModel)
        {
            bool Save = ad.AddAddress(VModel.Address);

            if (Save == true) 
            {
                return RedirectToAction("AddressVM", new { saved = customer.AddressSaved });
            }
            return RedirectToAction("AddressVM");
        }

        public ActionResult RemoveAddress(int id)
        {
            bool remove = ad.RemoveAddress(id);

            if(remove == true)
            {
                return RedirectToAction("AddressVM", "CustomerAddresses", new { removed = customer.DeletedAddress });
            }
            else
            {
                return RedirectToAction("AddressVM", new { error = customer.AddressUsed });
            }
        }


        //[Authorize]
        //public ActionResult Addresses()
        //{
        //    return View(ad.UserAddresses(User.Identity.Name));
        //}

        public ActionResult MyAddress(int id)
        {
            return PartialView(ad.GetAddress(id));
        }

        //[HttpGet]
        //public ActionResult NewAddress()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult NewAddress(CustomerAddress model)
        //{
        //    bool Saved = ad.AddAddress(model);

        //    if(Saved == true)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}
    }
}