using FootballApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballApp.Controllers
{
    public class EditTeamController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string TeamName)
        {
            try
            {
                using (Context db = new Context())
                {
                    Team team = new Team { TeamName = TeamName};
                    db.Teams.Add(team);
                    db.SaveChanges();
                }

                return RedirectToAction("ShowTeams", "Home");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(Team t)
        {
            using (Context db = new Context())
            {
                t.TeamId = db.Teams.FirstOrDefault(tt => tt.TeamName == t.TeamName).TeamId;
            }
            return View(t);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                using (Context db = new Context())
                {
                    Team mpt = new Team();
                    UpdateModel(mpt, collection);
                    Team editTeam = (from p in db.Teams
                                        where p.TeamId == mpt.TeamId
                                        select p).Single();
                    editTeam.TeamName= mpt.TeamName;
                    db.SaveChanges();
                }
                return RedirectToAction("ShowTeams", "Home");
            }
            catch
            {
                return View();
            }
        }
        //public ActionResult Delete(int teamId)
        //{
        //    Context db = new Context();
        //    Team team = (from p in db.Teams
        //                     where p.TeamId == teamId
        //                     select p).Single();
        //    List<TeamInMatch> teamInMatch = (from p in db.TeamInMatches
        //                                     where p.TeamId == teamId
        //                                     select p).ToList();
        //    db.Teams.Remove(team);
        //    db.SaveChanges();
        //    return RedirectToAction("ShowPlayers", "Home");
        //}
    }
}