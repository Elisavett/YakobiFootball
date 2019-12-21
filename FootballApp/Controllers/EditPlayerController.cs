using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballApp.Models;

namespace FootballApp.Controllers
{
    public class EditPlayerController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string FullName)
        {
            try
            {
                using (Context db = new Context())
                {
                    Player player = new Player { FullName = FullName };
                    db.Players.Add(player);
                    db.SaveChanges();
                }
                return RedirectToAction("ShowPlayers", "Home");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(Player p)
        {
            using (Context db = new Context())
            {
                p.PlayerId = db.Players.FirstOrDefault(pl => pl.FullName == p.FullName).PlayerId;
            }
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                using (Context db = new Context())
                {
                    Player mpt = new Player();
                    UpdateModel(mpt, collection);
                    Player newPlayer = (from p in db.Players
                                        where p.PlayerId == mpt.PlayerId
                                        select p).Single();
                    newPlayer.FullName = mpt.FullName;
                    db.SaveChanges();
                }
                return RedirectToAction("ShowPlayers", "Home");
            }
            catch 
            {
                return View();
            }
        }
        //public ActionResult Delete(int playerId)
        //{
        //    Context db = new Context();
        //    Player player = (from p in db.Players
        //                     where p.PlayerId == playerId
        //                     select p).Single();
        //    List<PlayersInTeam> playersInTeams = (from p in db.PlayersInTeams
        //                                   where p.PlayerId == playerId
        //                                   select p).ToList();
        //    db.PlayersInTeams.RemoveRange(playersInTeams);
        //    db.Players.Remove(player);
        //    db.SaveChanges();
        //    return RedirectToAction("ShowPlayers", "Home");
        //}
    }
}
