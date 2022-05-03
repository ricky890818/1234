using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.IO;
using 畢業專題.Models;

namespace 畢業專題.Controllers
{
    public class Mail_Controller : Controller
    {
        // GET: Mail_
        public ActionResult Index()
        {
            return View();
        }

        public static void Send(string subject, string username, string userEmail, string bodyName, Controller controller, string guid)
        {            
            StreamReader sr = null;
            string strBody = "";
            
            //讀取信件範本
            try
            {
                sr = new StreamReader(controller.Server.MapPath("~/App_Data/"+bodyName), Encoding.Default);
                strBody = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr.Close();
            }
            //將範本內的特定變數作取代
            strBody = strBody.Replace("$MemberName$", username);
            strBody = strBody.Replace("$ActiveNo$", guid);
            MailMessage em = new MailMessage();
            em.From = new System.Net.Mail.MailAddress("luvdog000@gmail.com", "Love浪福", System.Text.Encoding.UTF8);
            em.To.Add(userEmail);
            em.SubjectEncoding = System.Text.Encoding.UTF8;
            em.BodyEncoding = Encoding.UTF8;
            //信件主題 
            em.Subject = subject;
            //內容 
            em.Body = strBody;
            em.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            //登入帳號認證  
            client.Credentials = new System.Net.NetworkCredential("luvdog000@gmail.com", "wtxDL6Jp");
            //使用587 Port 
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            //啟動SSL 
            client.EnableSsl = true;
            //寄出 
            client.Send(em);
        }

        [HttpGet]
        public ActionResult RegisterSave(string id)//註冊後，信箱驗證
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            var cus = db.tRegister.Where(m => m.tempGuid == id).FirstOrDefault();
            if (cus != null)//轉成字串進行比對
            {
                cus.tempGuid = Guid.NewGuid().ToString();//guid被當作id 不能空白，安全性考量，驗證後就給一個新值
                cus.emailCheck = "是";                
                db.SaveChanges();
                TempData["show"] = "驗證成功" + TempData["guid"];
                return RedirectToAction("Create", "Home");
            }
            else
            {
                TempData["show"] = "Access Denied";//新增show訊息
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult RegisterSave()
        {
            TempData["show"] = "Access Denied";//新增show訊息
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Forget(string email)//忘記密碼
        {
            if (email != null)
            {
                dbtMemberEntities1 db = new dbtMemberEntities1();
                var result = db.tRegister.Where(m => m.userEmail == email).FirstOrDefault();
                if (result != null)
                {
                    string guid = Guid.NewGuid().ToString();
                    result.tempGuid = guid;
                    db.SaveChanges();
                    Mail_Controller.Send("Love浪福 - 忘記密碼", result.username, result.userEmail, "忘記密碼.html", this, guid);
                    TempData["show"] = "驗證信已寄出";
                    return View();
                }
                TempData["show"] = "找不到您的帳號!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ResetVerify(string id)//忘記密碼驗證
        {
            dbtMemberEntities1 db = new dbtMemberEntities1();
            var result = db.tRegister.Where(m => m.tempGuid == id).FirstOrDefault();
            if (result != null)
            {
                result.tempGuid = Guid.NewGuid().ToString();//guid被當作id 不能空白，安全性考量，驗證後就給一個新值
                db.SaveChanges();
                TempData["Account"] = result.userEmail;
                TempData.Keep("Account");
                return View("ResetPassword");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult ResetPassword(string userPassword)//重設密碼
        {
            string tempAccount = (string)TempData["Account"];
            if (tempAccount != null)
            {
                dbtMemberEntities1 db = new dbtMemberEntities1();
                var cus = db.tRegister.Where(m => m.userEmail == tempAccount).FirstOrDefault();
                cus.userPassword = userPassword;
                cus.userAccount = tempAccount;                
                db.SaveChanges();
                TempData["show"] = "密碼重設成功";
                TempData["Account"] = null;
                return RedirectToAction("Create", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
        }

    }
}