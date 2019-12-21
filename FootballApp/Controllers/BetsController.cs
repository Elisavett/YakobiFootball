using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballApp.Models;

namespace FootballApp.Controllers
{
    public class BetsController : Controller
    {
        [HttpGet]
        public ActionResult makeBet(int TeamId, int MatchId)
        {
            Bet bet = new Bet();
            User user = new User();
            using (Context db = new Context())
            {
                if (TeamId != 0)
                {
                    bet.Team = db.Teams.FirstOrDefault(t => t.TeamId ==
                    db.TeamInMatches.FirstOrDefault(tm => tm.TeamInMatchId == TeamId).TeamId);
                }
                else
                    bet.Team = new Team() { TeamId = 0 };
                bet.Match = db.Matches.FirstOrDefault(m => m.MatchId == MatchId);
                bet.User = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                if(TeamId == bet.Match.FirstTeamId)
                    bet.Coefficient = db.Coefficients.FirstOrDefault(c => c.matchId == MatchId).firstTeamCoef;
                else if(TeamId == bet.Match.SecondTeamId)
                    bet.Coefficient = db.Coefficients.FirstOrDefault(c => c.matchId == MatchId).secondTeamCoef;
                else bet.Coefficient = db.Coefficients.FirstOrDefault(c => c.matchId == MatchId).drawCoef;
                user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            }
            return View(new Tuple<Bet, User>(bet, user));
        }
        [HttpPost]
        public ActionResult makeBet(int MatchId, int TeamId, double Coefficient, decimal Amount)
        {
            using (Context db = new Context())
            {
                Match match = db.Matches.FirstOrDefault(m => m.MatchId == MatchId);
                Team team = db.Teams.FirstOrDefault(t => t.TeamId == db.TeamInMatches.FirstOrDefault(tm => tm.TeamInMatchId == TeamId).TeamId);
                User user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                user.ballance -= Amount;
                Bet bet = new Bet();
                bet.Match = match;
                bet.Team = team;
                bet.User = user;
                bet.Coefficient = Coefficient;
                bet.Amount = Amount;
                db.Bets.Add(bet);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}