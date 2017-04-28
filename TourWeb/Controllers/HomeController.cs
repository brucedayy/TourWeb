using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TourWeb.Controllers
{
    public class HomeController : Controller
    {
        private TourWebDBEntities db = new TourWebDBEntities();
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    return View(db.Scene.ToList());
        //}

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (Session["username"] == null)
                return RedirectToAction("Login");
            int page = 1, count = 3;     
            //var article = db.Article.Include(a => a.User);
            var scene = (from item in db.Scene
                           orderby item.Sid
                           select item
                           ).Skip(count * (page - 1)).Take(count);
            return View(await scene.ToListAsync());
        }

        [HttpPost]
        public ActionResult Index(int? pg, int? ct)
        {
            if (Session["username"] == null)
                return RedirectToAction("Login");
            try
            {
                int page = 1, count = 3;
                if (pg != null && ct != null)
                {                    
                    page = (int)pg;
                    count = (int)ct;
                }
                var db = new TourWebDBEntities();
                var lstScene = new List<Scene>();
                var query = (from item in db.Scene
                             orderby item.Sid
                             select new
                             {
                                 Sid = item.Sid,
                                 Img = item.Img,
                                 Name = item.Name,
                                 Position = item.Position,
                                 Priority = item.Priority,
                                 Credit = item.Credit,
                                 Price = item.Price,
                                 PubTime = item.PubTime
                             }).Skip(count * (page - 1)).Take(count).ToList();
                foreach (var item in query)
                {
                    lstScene.Add(new Scene
                    {
                        Sid = item.Sid,
                        Img = item.Img,
                        Name = item.Name,
                        Position = item.Position,
                        Priority = item.Priority,
                        Credit = item.Credit,
                        Price = item.Price,
                        PubTime = item.PubTime
                    });
                }
                var jsonScene = new JObject(
                    new JProperty("total", lstScene.Count),
                    new JProperty("rows",
                          new JArray(from p in lstScene
                                     select new JObject(new JProperty("Sid", p.Sid), new JProperty("Img", p.Img), new JProperty("Name", p.Name),
                                            new JProperty("Position", p.Position), new JProperty("Priority", p.Priority), new JProperty("Credit", p.Credit),
                                               new JProperty("Price", p.Price), new JProperty("PubTime", p.PubTime)))));
                if (Math.Ceiling(jsonScene.Count / 3.0) > page)
                    return RedirectToAction("Index");
                return Content(jsonScene.ToString());
            }
            catch (Exception ex) { return Content("error"); }
        }

        [HttpGet]
        public ActionResult Admin()
        {
            if (Session["adminuser"] == null)
                return RedirectToAction("AdminLogin");
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                TourWebDBEntities db = new TourWebDBEntities();
                var set = db.Set<User>();
                var result = from u in set.ToList()
                             where u.UserName == username && u.Password == password
                             select u;
                if (result.Count() != 0)
                {
                    Session["username"] = username;
                    return Content("success");
                }
                else
                    return Content("error");
            }
            catch (Exception ex)
            {
                return Content("error");
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            try
            {
                var db = new TourWebDBEntities();
                var user = new User
                {
                    UserName = username,
                    Password = password,
                    PubTime = DateTime.Now
                };
                db.User.Add(user);
                db.SaveChanges();
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content("error");
            }
        }

        [HttpGet]
        public ActionResult Order(string Img,string Name,string Position,string Priority,string Credit,string Price, string Sid)
        {
            //var str = Img + "=>" + Name + "=>" + Position + "=>" + Priority + "=>" + Credit + "=>" + Price;
            ViewBag.Img = Img;
            ViewBag.Name = Name;
            ViewBag.Position = Position;
            ViewBag.Priority = Priority;
            ViewBag.Credit = Credit;
            ViewBag.Price = Price;
            ViewBag.Sid = Sid;
            return View();
        }

        [HttpPost]
        public ActionResult SubmitOrder(int sid,double total)
        {
            try {
                var uname = Session["username"].ToString();
                var order = new Order
                {
                    Sid = sid,
                    UserName = uname,
                    Total = total,
                    PubTime=DateTime.Now
                };
                db.Order.Add(order);
                db.SaveChanges();
                return Content("success");
            } catch (Exception ex) {
                //return Content("error");
                return Content(ex.ToString());
            }            
        }

        [HttpGet]
        public ActionResult Note()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitNote(string details)
        {
            try {
                var note = new Note
                {
                    UserName = Session["username"].ToString(),
                    Details =details,
                    PubTime=DateTime.Now
                };
                db.Note.Add(note);
                db.SaveChanges();
                return Content("success");
            } catch (Exception ex) { return Content(ex.ToString()); }
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {
            try
            {
                TourWebDBEntities db = new TourWebDBEntities();
                var set = db.Set<Admin>();
                var result = from u in set.ToList()
                             where u.UserName == username && u.Password == password
                             select u;
                if (result.Count() != 0)
                {
                    Session["adminuser"] = username;
                    return Content("success");
                }
                else
                    return Content("error");
            }
            catch (Exception ex)
            {
                return Content("error");
            }
        }

        [HttpGet]
        public ActionResult LogoutUser()
        {
            Session["username"] = null;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult LogoutAdminUser()
        {
            Session["adminuser"] = null;
            return RedirectToAction("AdminLogin");
        }

    }
}