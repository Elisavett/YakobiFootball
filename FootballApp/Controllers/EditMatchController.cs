using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballApp.Models;

namespace FootballApp.Controllers
{
    public class EditMatchController : Controller
    {
        public static AddMatchModel match;
        public List<PlayerGoal> GetMatchGoals(int MatchId)
        {
            Context db = new Context();
            List<PlayerGoal> pg = (from p in db.Players
                                   join pt in db.PlayersInTeams on p.PlayerId equals pt.PlayerId
                                   join g in db.Goals on pt.TeamId equals  g.TeamInMatchId
                                   where g.MatchId == MatchId && g.PlayerId == pt.PlayersInTeamId
                                   select new PlayerGoal
                                   {
                                       GoalId = g.GoalId,
                                       TeamId = pt.TeamId,
                                       PlayerName = p.FullName,
                                       PlayerId = p.PlayerId
                                   }).ToList();
            return pg;
        }
        public List<PlayerFall> GetMatchFalls(int MatchId)
        {
            Context db = new Context();
            List<PlayerFall> pf = (from p in db.Players
                                   join cm in db.ContraventionsInMatchs on new { p.PlayerId, MatchId } equals new { cm.PlayerId, cm.MatchId }
                                   join pt in db.PlayersInTeams on cm.PlayerId equals pt.PlayerId
                                   join c in db.Contraventions on cm.ContraventionId equals c.ContraventionId
                                   select new PlayerFall
                                   {
                                       ContraventionId = c.ContraventionId,
                                       ContraventionsInMatchId = cm.ContraventionsInMatchId,
                                       TeamId = pt.TeamId,
                                       PlayerName = p.FullName,
                                       PlayerId = p.PlayerId
                                   }).ToList();
            return pf;
        }
        public List<ExtendedPlayerInTeam> GetPlayerInTeams()
        {
            Context db = new Context();
            List<ExtendedPlayerInTeam> Ept = (from tm in db.TeamInMatches
                                              join pt in db.PlayersInTeams on tm.TeamInMatchId equals pt.TeamId
                                              join p in db.Players on pt.PlayerId equals p.PlayerId
                                              join pp in db.PlayerPositions on pt.PositionId equals pp.PlayerPositionId
                                              select new ExtendedPlayerInTeam
                                              {
                                                  PositionId = pp.PlayerPositionId,
                                                  PositionName = pp.Name,
                                                  TeamId = tm.TeamInMatchId,
                                                  PlayerId = p.PlayerId,
                                                  PlayerName = p.FullName
                                              }).ToList();
            return Ept;
        }
        public ActionResult AddMatch()
        {
            using (Context db = new Context())
            {
                match = new AddMatchModel();
                match.match = new Match();
                match.tournaments = (from t in db.Tournaments select t).ToList();
                match.tournamentStages = (from s in db.TournamentStages select s).ToList();
                match.teams = (from t in db.Teams select t).ToList();
                match.trainers = (from tr in db.Trainers select tr).ToList();
                match.judges = (from j in db.Judges select j).ToList();

                db.Coefficients.RemoveRange(db.Coefficients);
                db.SaveChanges();
            }
            return View(match);
        }
        
        public ActionResult DeleteMatch(Match m)
        {
            Context db = new Context();
            List<ContraventionsInMatch> contr = (from cntr in db.ContraventionsInMatchs where cntr.MatchId == m.MatchId select cntr).ToList();
            List<Goal> goals = (from g in db.Goals where g.MatchId == m.MatchId select g).ToList();
            List<Replacement> repl = (from r in db.Replacements where r.MatchId == m.MatchId select r).ToList();
            List<JudgesInMatch> judges = (from j in db.JudgesInMatchs where j.MatchId == m.MatchId select j).ToList();
            TeamInMatch tm1 = (from tm in db.TeamInMatches where tm.TeamInMatchId == m.FirstTeamId select tm).Single();
            TeamInMatch tm2 = (from tm in db.TeamInMatches where tm.TeamInMatchId == m.SecondTeamId select tm).Single();
            List<PlayersInTeam> pt1 = (from pt in db.PlayersInTeams where pt.TeamId == m.FirstTeamId select pt).ToList();
            List<PlayersInTeam> pt2 = (from pt in db.PlayersInTeams where pt.TeamId == m.SecondTeamId select pt).ToList();
            db.ContraventionsInMatchs.RemoveRange(contr);
            db.Goals.RemoveRange(goals);
            db.Replacements.RemoveRange(repl);
            db.JudgesInMatchs.RemoveRange(judges);
            db.SaveChanges();
            db.PlayersInTeams.RemoveRange(pt1);
            db.PlayersInTeams.RemoveRange(pt2); 
            Match mm = (from mat in db.Matches where mat.MatchId == m.MatchId select mat).Single();
            db.Matches.Remove(mm);
            db.SaveChanges();
            db.TeamInMatches.Remove(tm1);
            db.TeamInMatches.Remove(tm2);
            db.Coefficients.RemoveRange(db.Coefficients);
            db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
            public ActionResult EditMatch(Match m, int? FirstTeamTrainerId, int? SecondTeamTrainerId, int? tournamentId, int? stageId,int? view)
            {
            
            if(view==3)
            {
                Match mat = new Match();
                using (Context db  = new Context())
                {
                    mat = (from mtc in db.Matches where mtc.MatchId == m.MatchId select mtc).Single();
                    mat.MatchName = m.MatchName;
                    mat.Date = m.Date;
                    mat.Status = m.Status;
                    db.SaveChanges();
                    TeamInMatch tm1 = (from tm in db.TeamInMatches where tm.TeamInMatchId == m.FirstTeamId select tm).Single();
                    tm1.TrainerId = FirstTeamTrainerId;
                    TeamInMatch tm2 = (from tm in db.TeamInMatches where tm.TeamInMatchId == m.SecondTeamId select tm).Single();
                    tm2.TrainerId = SecondTeamTrainerId;
                }
                    
                if(m.Status == "Завершен")
                {
                    using (Context db = new Context())
                    {
                        if (db.Bets.FirstOrDefault(b => b.Match.MatchId == m.MatchId).satus == null)
                        {

                            int winerId = 0;
                            if (match.FirstTeamPlayersGoals.Count > match.SecondTeamPlayersGoals.Count)
                                winerId = mat.FirstTeamId;
                            else if (match.FirstTeamPlayersGoals.Count < match.SecondTeamPlayersGoals.Count)
                                winerId = mat.SecondTeamId;

                            var winerbets = db.Bets.Where(b => b.Match.MatchId == mat.MatchId && b.Team.TeamId == winerId);
                            var looserbets = db.Bets.Where(b => b.Match.MatchId == mat.MatchId && b.Team.TeamId != winerId);
                            foreach (Bet bet in winerbets)
                            {
                                using(Context DB = new Context())
                                {
                                    bet.User = DB.Bets.Where(bb => bb.Id == bet.Id).Select(u => u.User).Single();
                                    Bet b = DB.Bets.Where(bb => bb.Id == bet.Id).Select(bb=>bb).Single();
                                    b.satus = true;
                                    User us = DB.Users.Where(u => u.Id == bet.User.Id).FirstOrDefault();
                                    us.ballance += bet.Amount * Convert.ToDecimal(bet.Coefficient);
                                    DB.SaveChanges();
                                }
                                
                            }
                            foreach (Bet bet in looserbets) { bet.satus = false; db.SaveChanges(); }
                        }
                    }
                }
            }
            if (m.MatchId == 0)
            {
                using (Context db = new Context())
                {
                    match = new AddMatchModel();
                    TeamInMatch tm1 = new TeamInMatch() { TeamId = m.FirstTeamId, TrainerId = FirstTeamTrainerId };
                    TeamInMatch tm2 = new TeamInMatch() { TeamId = m.SecondTeamId, TrainerId = SecondTeamTrainerId };
                    db.TeamInMatches.Add(tm1);
                    db.TeamInMatches.Add(tm2);
                    db.SaveChanges();
                    m.FirstTeamId = tm1.TeamInMatchId;
                    m.SecondTeamId = tm2.TeamInMatchId;
                    m.TournamentId = Convert.ToInt32(tournamentId);
                    m.TournamentStageId = Convert.ToInt32(stageId);
                    string firstTeamName = (from t in db.Teams
                                            join tm in db.TeamInMatches on t.TeamId equals tm.TeamId
                                            where tm.TeamInMatchId == m.FirstTeamId
                                            select t.TeamName).Single();
                    string secondTeamName = (from t in db.Teams
                                             join tm in db.TeamInMatches on t.TeamId equals tm.TeamId
                                             where tm.TeamInMatchId == m.SecondTeamId
                                             select t.TeamName).Single();
                    m.MatchName = firstTeamName + "-" + secondTeamName;
                    db.Matches.Add(m);
                    db.SaveChanges();
                }
            }
            else
            {
                match = new AddMatchModel();
                List<PlayerFall> pf = GetMatchFalls(m.MatchId);
                match.FirstTeamPlayersFalls = (from p in pf where p.TeamId == m.FirstTeamId select p).ToList();
                match.SecondTeamPlayersFalls = (from p in pf where p.TeamId == m.SecondTeamId select p).ToList();

                List<ExtendedPlayerInTeam> Ept = GetPlayerInTeams();
                match.PlayersInFirstTeam = Ept.Where(t => t.TeamId == m.FirstTeamId).ToList();
                match.PlayersInSecondTeam = Ept.Where(t => t.TeamId == m.SecondTeamId).ToList();

                List<PlayerGoal> pg = GetMatchGoals(m.MatchId);
                match.FirstTeamPlayersGoals = (from p in pg where p.TeamId == m.FirstTeamId select p).ToList();
                match.SecondTeamPlayersGoals = (from p in pg where p.TeamId == m.SecondTeamId select p).ToList();
                using (Context db = new Context())
                {
                    FirstTeamTrainerId = db.TeamInMatches.Where(tm => tm.TeamInMatchId == m.FirstTeamId).Select(tm => tm.TrainerId).Single();
                    SecondTeamTrainerId = db.TeamInMatches.Where(tm => tm.TeamInMatchId == m.SecondTeamId).Select(tm => tm.TrainerId).Single();
                }

            }
            using (Context db = new Context())
            {
                match.match = m;
                match.SecondTeamTrainerId = SecondTeamTrainerId;
                match.FirstTeamTrainerId = FirstTeamTrainerId;
                match.tournament = db.Tournaments.Where(t => t.TournamentId == 1).Select(t => t.Name).Single();
                match.tournamentStage = db.TournamentStages.Where(s => s.TournamentStageId == m.TournamentStageId).Select(s => s.Name).Single();
                match.players = (from p in db.Players select p).ToList();
                match.teams = (from t in db.Teams select t).ToList();
                match.trainers = (from tr in db.Trainers select tr).ToList();
                match.judges = (from j in db.Judges select j).ToList();
                match.FirstTeamId = db.TeamInMatches.Where(t => t.TeamInMatchId == m.FirstTeamId).Select(t => t.TeamId).Single();
                match.SecondTeamId = db.TeamInMatches.Where(t => t.TeamInMatchId == m.SecondTeamId).Select(t => t.TeamId).Single();
                match.positions = (from p in db.PlayerPositions select p).ToList();
                match.statuses = new string[3] { "Ожидается", "Идёт", "Завершен" };
            }
            return View(match);
        }
        public ActionResult AddNewTeam(int? view)
        {
            return View(view);
        }
        [HttpPost]
        public ActionResult AddNewTeam(Team t, int? view)
        {
            Context db = new Context();
            db.Teams.Add(t);
            db.SaveChanges();
            match.teams = (from tm in db.Teams select tm).ToList();
            if (view == 1) return View("AddMatch", match);
            else 
                return View("EditMatch", match);
        }
        public ActionResult AddNewPlayer()
        {
            return View();

        }
        public List<ExtendedPlayerInTeam> AddPlayerInTeam(PlayersInTeam pit)
        {
            Context db = new Context();
            db.PlayersInTeams.Add(pit);
            db.SaveChanges();
            List<ExtendedPlayerInTeam> Ept = GetPlayerInTeams();
            return Ept;
        }
        public ActionResult AddPlayerIn1Team(PlayersInTeam pit, int TeamId)
        {
            List<ExtendedPlayerInTeam> Ept = AddPlayerInTeam(pit);
            match.PlayersInFirstTeam = Ept.Where(t => t.TeamId == TeamId).ToList();
            return View("EditMatch", match);
        }
        public ActionResult AddPlayerIn2Team(PlayersInTeam pit, int TeamId)
        {
            List<ExtendedPlayerInTeam> Ept = AddPlayerInTeam(pit);
            match.PlayersInSecondTeam = Ept.Where(t => t.TeamId == TeamId).ToList();
            return View("EditMatch", match);
        }
        public void DeletePlayer(int TeamId, int TeamPlayerId, int MatchId)
        {
            Context db = new Context();
            if (db.Goals.Where(g => g.MatchId == MatchId && g.PlayerId == TeamPlayerId).Select(g => g.GoalId).SingleOrDefault() != 0)
            {
                Goal goal = db.Goals.Where(g => g.MatchId == MatchId && g.PlayerId == TeamPlayerId).Single();
                db.Goals.Remove(goal);
            }
            if (db.ContraventionsInMatchs.Where(c => c.PlayerId == TeamPlayerId && c.MatchId == MatchId).Select(c => c.ContraventionsInMatchId).SingleOrDefault() != 0)
            {
                ContraventionsInMatch contr = db.ContraventionsInMatchs.Where(c => c.PlayerId == TeamPlayerId && c.MatchId == MatchId).Single();
                db.ContraventionsInMatchs.Remove(contr);
            }
            if (db.Replacements.Where(g => g.MatchId == MatchId && (g.ReplacingPlayerId == TeamPlayerId || g.ReplaceablePlayerId == TeamPlayerId)).Select(g => g.ReplacementId).SingleOrDefault() != 0)
            {
                Replacement repl = db.Replacements.Where(g => g.MatchId == MatchId && (g.ReplacingPlayerId == TeamPlayerId || g.ReplaceablePlayerId == TeamPlayerId)).Single();
                db.Replacements.Remove(repl);
            }
            PlayersInTeam player = db.PlayersInTeams.Where(pt => pt.PlayerId == TeamPlayerId && pt.TeamId == TeamId).First();
            db.PlayersInTeams.Remove(player);
            db.SaveChanges();

        }
        public ActionResult Delete1Player(int TeamId1, int firstTeamPlayerId, int MatchId)
        {
            DeletePlayer(TeamId1, firstTeamPlayerId, MatchId);

            List<ExtendedPlayerInTeam> Ept = GetPlayerInTeams();
            match.PlayersInFirstTeam = Ept.Where(t => t.TeamId == TeamId1).ToList();

            List<PlayerFall> pf = GetMatchFalls(MatchId);
            match.FirstTeamPlayersFalls = (from p in pf where p.TeamId == match.match.FirstTeamId select p).ToList();

            List<PlayerGoal> pg = GetMatchGoals(MatchId);
            match.FirstTeamPlayersGoals = (from p in pg where p.TeamId == match.match.FirstTeamId select p).ToList();

            return View("EditMatch", match);
        }
        public ActionResult Delete2Player(int TeamId2, int secondTeamPlayerId, int MatchId)
        {
            DeletePlayer(TeamId2, secondTeamPlayerId, MatchId);

            List<ExtendedPlayerInTeam> Ept = GetPlayerInTeams();
            match.PlayersInSecondTeam = Ept.Where(t => t.TeamId == TeamId2).ToList();

            List<PlayerFall> pf = GetMatchFalls(MatchId);
            match.SecondTeamPlayersFalls = (from p in pf where p.TeamId == match.match.SecondTeamId select p).ToList();

            List<PlayerGoal> pg = GetMatchGoals(MatchId);
            match.SecondTeamPlayersGoals = (from p in pg where p.TeamId == match.match.SecondTeamId select p).ToList();

            return View("EditMatch", match);
        }
        public ActionResult Delete1Goal(int GoalId1)
        {
            List<PlayerGoal> pg = DeleteGoal(GoalId1);
            match.FirstTeamPlayersGoals = (from p in pg where p.TeamId == match.match.FirstTeamId select p).ToList();

            return View("EditMatch", match);
        }
        public ActionResult Delete2Goal(int GoalId2)
        {
            List<PlayerGoal> pg = DeleteGoal(GoalId2);
            match.SecondTeamPlayersGoals = (from p in pg where p.TeamId == match.match.SecondTeamId select p).ToList();

            return View("EditMatch", match);
        }
        public List<PlayerGoal> DeleteGoal(int GoalId)
        {
            Context db = new Context();
            Goal goal = db.Goals.Where(g => g.GoalId == GoalId).Single();
            db.Goals.Remove(goal);
            db.SaveChanges();
            List<PlayerGoal> pg = GetMatchGoals(goal.MatchId);
            return pg;
      
        }
        public List<PlayerFall> DeleteFall(int FallId)
        {
            Context db = new Context();
            ContraventionsInMatch contr = db.ContraventionsInMatchs.Where(g => g.ContraventionsInMatchId == FallId).Single();
            db.ContraventionsInMatchs.Remove(contr);
            db.SaveChanges();
            List<PlayerFall> pf = GetMatchFalls(contr.MatchId);
            return pf;
        }
        public ActionResult Delete1Fall(int FallId1)
        {
            List<PlayerFall> pf = DeleteFall(FallId1);
            match.FirstTeamPlayersFalls = (from p in pf where p.TeamId == match.match.FirstTeamId select p).ToList();

            return View("EditMatch", match);
        }
        public ActionResult Delete2Fall(int FallId2)
        {
            List<PlayerFall> pf = DeleteFall(FallId2);
            match.SecondTeamPlayersFalls = (from p in pf where p.TeamId == match.match.SecondTeamId select p).ToList();

            return View("EditMatch", match);
        }
        [HttpPost]
        public ActionResult AddNewPlayer(Player p)
        {
            Context db = new Context();
            db.Players.Add(p);
            db.SaveChanges();
            match.players = (from pl in db.Players select pl).ToList();
            return View("EditMatch", match);
        }
        public PlayerGoal AddTeamGoal (int MatchId, int TeamPlayerId)
        {
            Context db = new Context();
            PlayerGoal g = new PlayerGoal();
            g.PlayerId = TeamPlayerId;
            g.PlayerName = db.Players.Where(p => p.PlayerId == TeamPlayerId).Select(p => p.FullName).Single();
            g.MatchId = MatchId;
            g.MatchName = db.Matches.Where(m => m.MatchId == MatchId).Select(m => m.MatchName).Single();
            return g;
        }
        // GET: EditTeam/Edit/5
        public ActionResult Add1TeamGoal(int MatchId, int firstTeamPlayerId)
        {
            PlayerGoal g = AddTeamGoal(MatchId, firstTeamPlayerId);
            return View(new Tuple<PlayerGoal,int, int>(g,1, match.match.FirstTeamId));
        }
        public ActionResult Add2TeamGoal(int MatchId, int secondTeamPlayerId)
        {
            PlayerGoal g = AddTeamGoal(MatchId, secondTeamPlayerId);
            return View("Add1TeamGoal", new Tuple<PlayerGoal, int, int>(g, 2, match.match.SecondTeamId));
        }
        public PlayerFall AddTeamFall(int MatchId, int TeamPlayerId)
        {
            Context db = new Context();
            PlayerFall g = new PlayerFall();
            g.PlayerId = TeamPlayerId;
            g.PlayerName = db.Players.Where(p => p.PlayerId == TeamPlayerId).Select(p => p.FullName).Single();
            g.MatchId = MatchId;
            g.MatchName = db.Matches.Where(m => m.MatchId == MatchId).Select(m => m.MatchName).Single();
            List<Contravention> cntr = (from c in db.Contraventions select c).ToList();
            return g;
        }
        public ActionResult Add1TeamFall(int MatchId, int firstTeamPlayerId)
        {
            Context db = new Context();
            PlayerFall g = AddTeamFall(MatchId, firstTeamPlayerId);
            List <Contravention> cntr = (from c in db.Contraventions select c).ToList();
            return View(new Tuple<PlayerFall, List<Contravention>, int, int>(g, cntr, 1, match.match.FirstTeamId));
        }
        public ActionResult Add2TeamFall(int MatchId, int secondTeamPlayerId)
        {
            Context db = new Context();
            PlayerFall g = AddTeamFall(MatchId, secondTeamPlayerId);
            List<Contravention> cntr = (from c in db.Contraventions select c).ToList();
            return View("Add1TeamFall", new Tuple<PlayerFall, List<Contravention>, int, int>(g, cntr, 2, match.match.SecondTeamId));
        }
        public ActionResult Add1Player(int TeamId1)
        {
            List<PlayerPosition> positions = new List<PlayerPosition>();
            List<Player> players = new List<Player>();
            using (Context db = new Context())
            {
                positions = (from c in db.PlayerPositions select c).ToList();
                players = (from p in db.Players select p).ToList();
            }
            return View("AddTeamPlayer",new Tuple<List<Player>, List<PlayerPosition>, int>(players, positions, TeamId1));
        }
        public ActionResult Add2Player(int TeamId2)
        {
            List<PlayerPosition> positions = new List<PlayerPosition>();
            List<Player> players = new List<Player>();
            using (Context db = new Context())
            {
                positions = (from c in db.PlayerPositions select c).ToList();
                players = (from p in db.Players select p).ToList();
            }
            return View("AddTeam2Player", new Tuple<List<Player>, List<PlayerPosition>, int>(players, positions, TeamId2));
        }
        [HttpPost]
        public ActionResult AddGoal(Goal goal, int teamNum)
        {
            using (Context db = new Context())
            {
                goal.PlayerId = db.PlayersInTeams.FirstOrDefault(pt => pt.PlayerId == goal.PlayerId && pt.TeamId == goal.TeamInMatchId).PlayersInTeamId;
                db.Goals.Add(goal);
                db.SaveChanges();
                List<PlayerGoal> pg = GetMatchGoals(goal.MatchId);
                if (teamNum == 1)
                {
                    match.FirstTeamPlayersGoals = (from p in pg where p.TeamId == goal.TeamInMatchId select p).ToList();

                }
                if (teamNum == 2)
                {
                    match.SecondTeamPlayersGoals = (from p in pg where p.TeamId == goal.TeamInMatchId select p).ToList();
                }
            }

            return View("EditMatch", match);
        }
        [HttpPost]
        public ActionResult AddFall(ContraventionsInMatch contr, int teamNum, int teamId)
        {
            Context db = new Context();
            db.ContraventionsInMatchs.Add(contr);
            db.SaveChanges();
            List<PlayerFall> pf = GetMatchFalls(contr.MatchId);
            if (teamNum == 1)
            {
                match.FirstTeamPlayersFalls = (from p in pf where p.TeamId == teamId select p).ToList();

            }
            if (teamNum == 2)
            {
                match.SecondTeamPlayersFalls = (from p in pf where p.TeamId == teamId select p).ToList();
            }
            return View("EditMatch", match);
        }

    }
}
