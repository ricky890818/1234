using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using 畢業專題.Models;


namespace 畢業專題.Controllers
{
    public class CrudController : Controller
    {
        dbtMemberEntities1 db = new dbtMemberEntities1();
        // GET: Crud
        public ActionResult Read()
        {
            var cusList = db.tRegister.OrderByDescending(m => m.userEmail).ToList();
            return View(cusList);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(string id)
        {
            var cus = db.tRegister.Where
                (m => m.tempGuid == id).FirstOrDefault();
            return View(cus);
        }
        [HttpPost]
        public ActionResult Edit
            (string userName,int userPone,string userAccount,string userPassword,string tempGuid)
        {
            var cus = db.tRegister.Where
                (m => m.tempGuid == tempGuid).FirstOrDefault();
            cus.username = userName;
            cus.userPhone = userPone;
            cus.userAccount = userAccount;
            cus.userPassword = userPassword;
            db.SaveChanges();
            return RedirectToAction("Read");
        }
   
        public ActionResult Delete(string id)
        {
            var cus = db.tRegister.Where
                (m => m.tempGuid == id).FirstOrDefault();
            db.tRegister.Remove(cus);
            db.SaveChanges();
            return RedirectToAction("Read");
        }
        
    }
}