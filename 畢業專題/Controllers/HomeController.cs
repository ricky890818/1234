using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 畢業專題.Models;

namespace 畢業專題.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            /*dbtMemberEntities1 db = new dbtMemberEntities1();
            var result = db.tLogin.ToList();*/
            //TempData["show"] = "test";
            if (Session["Member"] != null) TempData["btn1"] = "登出";
            return View(/*result*/);

        }
        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult Create2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(tRegister cus)
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            if (ModelState.IsValid)
            {                
                var result = db.tRegister.Where(mbox => mbox.userEmail == cus.userEmail).FirstOrDefault();
                string show = "";
                if (result != null)
                {
                    if(result.emailCheck=="是")
                        TempData["show"] = result.username + " 您已註冊，且已驗證信箱，登入即可";
                    else
                        TempData["show"] = cus.username + " 您已註冊，請驗證信箱，再登入";
                    return RedirectToAction("Create");
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();
                    cus.tempGuid = guid;
                    cus.emailCheck = "否";
                    db.tRegister.Add(cus);
                    db.SaveChanges();
                    Mail_Controller.Send("Love浪福，信箱驗證", cus.username, cus.userEmail, "信件範本.html", this, guid);
                    TempData["cus"] = cus;
                    show = "驗證信已寄到"+cus.userEmail+"\n請前往信箱驗證!";
                }
                TempData["show"] = show;
            }
            else
            {
                TempData["show"] = "There are some problems";
            }
            return RedirectToAction("Create");
        }
        public ActionResult Delete(string email)
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            var churn = db.tLogin.Where(m => m.email == email).FirstOrDefault();
            db.tLogin.Remove(churn);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string userEmail)
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            var result = db.tRegister.Where(m => m.userEmail == userEmail).FirstOrDefault();
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(string username, int userPhone, string userAccount, string userPassword, string userEmail)
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            var result = db.tRegister.Where(m => m.userAccount == userAccount).FirstOrDefault();
            result.username = username;
            result.userPhone = userPhone;
            result.userAccount = userAccount;
            result.userPassword = userPassword;
            result.userEmail = userEmail;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //登入
        [HttpPost]
        public ActionResult Create(tLogin cus)
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            DateTime date = DateTime.Now;
            string next = "";
            if (ModelState.IsValid)
            {
                var result = db.tRegister.Where(mbox => mbox.userEmail == cus.email && mbox.userPassword == cus.pass).FirstOrDefault();
                
                if (result != null)
                {
                    if (result.emailCheck == "是")
                    {
                        cus.time = date.ToString();
                        db.tLogin.Add(cus);
                        db.SaveChanges();
                        TempData["show"] = result.username + " 您已登入";
                        next = "Index";
                        Session["Member"] = cus;
                        Session["Welcome"] = result.username + "歡迎光臨";
                    }
                    else
                    {
                        TempData["show"] = " 您已註冊，請驗證信箱，再登入";
                        return RedirectToAction("Create2","Home");
                    }
                }
                else
                {
                    TempData["show"] = "帳號不存在或帳號密碼錯誤!";
                    next = "Create";
                }
                return RedirectToAction(next);
            }
            return View(cus);
        }
        
        public ActionResult Create3()
        {
            return View();
        }

        //實體領養
        public ActionResult Adopt()
        {
            return View();
        }
        //登出
        public ActionResult Logout()
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            tLogout cus = new tLogout();
            cus.time = DateTime.Now.ToString();
            cus.email = (Session["Member"] as tLogin).email;
            cus.pass = (Session["Member"] as tLogin).pass;
            db.tLogout.Add(cus);
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
