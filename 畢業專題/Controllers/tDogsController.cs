using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using 畢業專題.Models;

namespace 畢業專題.Controllers
{
    public class tDogsController : Controller
    {
        private dbtMemberEntities1 db = new dbtMemberEntities1();

        // GET: tDogs
        public async Task<ActionResult> Index()
        {
            var dogs = from m in db.tDogs
                       where m.dogSex == "公" || m.dogSex == "母"
                       select m;
            return View(await dogs.ToListAsync());
        }

        // GET: tDogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tDogs tDogs = await db.tDogs.FindAsync(id);
            if (tDogs == null)
            {
                return HttpNotFound();
            }
            return View(tDogs);
        }

        // GET: tDogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tDogs/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "dogId,dogName,dogBreed,dogSize,dogAge,dogSex,dogNeuter,dogStory,dogImage")] tDogs tDogs)
        {
            if (ModelState.IsValid)
            {
                db.tDogs.Add(tDogs);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tDogs);
        }

        // GET: tDogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tDogs tDogs = await db.tDogs.FindAsync(id);
            if (tDogs == null)
            {
                return HttpNotFound();
            }
            return View(tDogs);
        }

        // POST: tDogs/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "dogId,dogName,dogBreed,dogSize,dogAge,dogSex,dogNeuter,dogStory,dogImage")] tDogs tDogs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tDogs).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tDogs);
        }

        // GET: tDogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tDogs tDogs = await db.tDogs.FindAsync(id);
            if (tDogs == null)
            {
                return HttpNotFound();
            }
            return View(tDogs);
        }

        // POST: tDogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tDogs tDogs = await db.tDogs.FindAsync(id);
            db.tDogs.Remove(tDogs);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
