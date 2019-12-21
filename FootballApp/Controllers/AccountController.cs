using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FootballApp.Models;

namespace FootballApp.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (Context db = new Context())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }
        public ActionResult RemoveMoney()
        {
            decimal CurrSum;
            using (Context db = new Context())
            {
                CurrSum = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name).ballance;
            }
            return View(Convert.ToInt32(CurrSum));
        }
        [HttpPost]
        public ActionResult RemoveMoney(int sum)
        {
            using (Context db = new Context())
            {
                User u = db.Users.FirstOrDefault(us => us.Email == User.Identity.Name);
                u.ballance -= sum;
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult AddMoney()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMoney(int sum)
        {
            using(Context db = new Context())
            {
                User u = db.Users.FirstOrDefault(us => us.Email == User.Identity.Name);
                u.ballance += sum;
                db.SaveChanges();
            }
            return View();

        }
        public ActionResult Account()
        {
                List<Bet> bets = new List<Bet>();
                User user = new User();
                using (Context db = new Context())
                {
                    user = db.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
                    bets = db.Bets.Where(b => b.User.Id == user.Id).ToList();
                    foreach (Bet bet in bets)
                    {
                        Team team = db.Bets.Where(b => b.Id == bet.Id).Select(b => b.Team).Single();
                        Match match = db.Bets.Where(b => b.Id == bet.Id).Select(b => b.Match).Single();
                        bet.Match = match;
                        bet.Team = team;
                    }
                }
                return View(bets);
        }
        public ActionResult EditAccount()
        {
            ViewBag.isChanged = "";
            RegisterModel rm = new RegisterModel();
            User user = new User();
            using(Context db = new Context())
            {
                user = db.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            }
            rm.Name = user.Email;
            rm.Password = user.Password;
            rm.UserId = user.Id;
            return View(rm);
        }
        public ActionResult EditUserName()
        {
            return PartialView("EditUserNamePartial");
        }
        [HttpPost]
        public ActionResult EditAccount(RegisterModel rm)
        {
            ViewBag.isChanged = "Изменения сохранены";
            User user = new User();
            using(Context db = new Context())
            {
                User u = db.Users.Where(us => us.Id == rm.UserId).SingleOrDefault();
                if(rm.Password != null)
                {
                    u.Password = rm.Password;
                }
                u.Email = rm.Name;
                db.SaveChanges();
                user = db.Users.Where(us => us.Id == rm.UserId).SingleOrDefault();
            }
            return View("EditAccount", rm);
        }
        public ActionResult EditUser(User user)
        {
            ViewBag["isChanged"] = "";
            return PartialView("EditPartial");
        }
            public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (Context db = new Context())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Name);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (Context db = new Context())
                    {
                        db.Users.Add(new User { Email = model.Name, Password = model.Password, RoleId = 2 });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Console()
        {
            List<User> users = new List<User>();
            using (Context db = new Context())
            {
                users = db.Users.Select(t => t).ToList();
                foreach(User user in users)
                {
                    user.Role = db.Roles.FirstOrDefault(r => r.Id == user.RoleId);
                }
            }
            return View(users);
        }
        public ActionResult EditRole(int Id)
        {
            List<Role> roles = new List<Role>();
            User user = new User();
            using (Context db = new Context())
            {
                roles = db.Roles.Select(r => r).ToList();
                user = db.Users.FirstOrDefault(u => u.Id == Id);
            }
            return View(new Tuple<List<Role>, User>(roles, user));
        }
        [HttpPost]
        public ActionResult EditRole(int UserId, int RoleId)
        {
            using (Context db = new Context())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == UserId);
                user.RoleId = RoleId;
                user.Role = db.Roles.FirstOrDefault(r => r.Id == RoleId);
                db.SaveChanges();
            }
            return RedirectToAction("Console", "Account");
        }
    }
}