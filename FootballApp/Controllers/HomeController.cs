using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootballApp.Models;
using System.Data.Entity;

namespace FootballApp.Controllers
{
    public class HomeController : Controller
    {
        Context db = new Context();

        public ActionResult StartPage()
        {
            return View();
        }
        public ActionResult Index()
        {
            DefineCoafficients();
            List<Schedule> l = new List<Schedule>();
            l = (from m in db.Matches
                 join t in db.Tournaments on m.TournamentId equals t.TournamentId
                 join s in db.TournamentStages on m.TournamentStageId equals s.TournamentStageId
                 join coef in db.Coefficients on m.MatchId equals coef.matchId
                 where m.Status == "Ожидается"
                 select new Schedule
                 {
                     Date = m.Date,
                     matchName = m.MatchName,
                     matchId = m.MatchId,
                     tournament = t.Name,
                     tournamentStage = s.Name,
                     goals = "-",
                     Status = m.Status,
                     FirstTeamCoef = coef.firstTeamCoef,
                     SecondTeamCoef = coef.secondTeamCoef,
                     DrawCoef = coef.drawCoef
                 }).OrderBy(s => s.Date).ToList();
            if (db.Matches.Where(q => q.Status == "Идет").ToList().Count == 0)
            {
                List<MatchesPlayersTeams> Scores =
                  (from m in db.Matches
                   join tm in db.TeamInMatches on m.FirstTeamId equals tm.TeamInMatchId
                   join pt in db.PlayersInTeams on tm.TeamInMatchId equals pt.TeamId
                   join p in db.Players on pt.PlayerId equals p.PlayerId
                   join t in db.Teams on tm.TeamId equals t.TeamId
                   join g in db.Goals on tm.TeamInMatchId equals g.TeamInMatchId into subg
                   from goal in subg.DefaultIfEmpty()
                   where m.Status != "Ожидается" && (goal.PlayerId == pt.PlayersInTeamId || goal == null)
                   select new MatchesPlayersTeams
                   {
                       matchName = m.MatchName,
                       teamName = t.TeamName,
                       matchId = m.MatchId,
                       teamId = pt.TeamId,
                       playerName = p.FullName,
                       date = m.Date,
                       goalId = goal.GoalId
                   }).ToList();
                List<MatchesPlayersTeams> Scores2 =
                  (from m in db.Matches
                   join tm in db.TeamInMatches on m.SecondTeamId equals tm.TeamInMatchId
                   join pt in db.PlayersInTeams on tm.TeamInMatchId equals pt.TeamId
                   join p in db.Players on pt.PlayerId equals p.PlayerId
                   join t in db.Teams on tm.TeamId equals t.TeamId
                   join g in db.Goals on tm.TeamInMatchId equals g.TeamInMatchId into subg
                   from goal in subg.DefaultIfEmpty()
                   where m.Status != "Ожидается" && (goal.PlayerId == pt.PlayersInTeamId || goal == null)
                   select new MatchesPlayersTeams
                   {
                       matchName = m.MatchName,
                       teamName = t.TeamName,
                       matchId = m.MatchId,
                       teamId = pt.TeamId,
                       playerName = p.FullName,
                       date = m.Date,
                       goalId = goal.GoalId
                   }).ToList();
                //List<MatchesPlayersTeams> Scores;
                Scores.AddRange(Scores2);
                List<MatchScores> ms = (from Score in Scores
                                        group Score by new { Score.matchName, Score.teamId, Score.matchId, Score.teamName, Score.date } into h
                                        select new MatchScores
                                        {
                                            Date = h.Key.date,
                                            teamId = h.Key.teamId,
                                            matchId = h.Key.matchId,
                                            matchName = h.Key.matchName,
                                            teamName = h.Key.teamName,
                                            goals = h.Count(k => k.goalId != null)
                                        }).ToList();
                l.AddRange((from o in ms
                            join o2 in ms on o.matchId equals o2.matchId
                            join m in db.Matches on o.matchId equals m.MatchId
                            join t in db.Tournaments on m.TournamentId equals t.TournamentId
                            join s in db.TournamentStages on m.TournamentStageId equals s.TournamentStageId
                            where o.teamName != o2.teamName && (o.goals > o2.goals || o.goals == o2.goals) && o2.matchId == m.MatchId && m.Status != "Ожидается"
                            select new Schedule
                            {
                                Date = m.Date,
                                matchName = m.MatchName,
                                matchId = m.MatchId,
                                tournament = t.Name,
                                tournamentStage = s.Name,
                                goals = o.goals + ":" + o2.goals,
                                Status = m.Status,
                                FirstTeamCoef = 0,
                                SecondTeamCoef = 0,
                                DrawCoef = 0
                            }).OrderBy(s => s.Date).GroupBy(g => new { g.FirstTeamCoef, g.SecondTeamCoef, g.DrawCoef, g.matchId, g.matchName, g.Status, g.tournament, g.tournamentStage, g.goals, g.Date }).Select(g =>
                            new Schedule
                            {
                                FirstTeamCoef = g.Key.FirstTeamCoef,
                                SecondTeamCoef = g.Key.SecondTeamCoef,
                                DrawCoef = g.Key.DrawCoef,
                                Date = g.Key.Date,
                                goals = g.Key.goals,
                                matchName = g.Key.matchName,
                                Status = g.Key.Status,
                                tournament = g.Key.tournament,
                                tournamentStage = g.Key.tournamentStage,
                                matchId = g.Key.matchId
                            }).ToList());
            }

            return View(l);
        }
        public void DefineCoafficients()
        {
            List<TeamsGoals> TeamGoalNums = new List<TeamsGoals>();
            using (Context BD = new Context())
            {
                var smth =
                   (from m in BD.Matches
                    from tm in BD.TeamInMatches
                    from g in BD.Goals
                    where (m.FirstTeamId == tm.TeamInMatchId || m.SecondTeamId == tm.TeamInMatchId) && g.TeamInMatchId == tm.TeamInMatchId
                    select new
                    {
                        teamId = tm.TeamId,
                        teamInMatchId = tm.TeamInMatchId,
                        goalId = g.GoalId
                    });
                TeamGoalNums = (
                    from s in smth
                    group s by s.teamId into h
                    select new TeamsGoals
                    {
                        GoalsNum = h.Count(),
                        TeamId = h.Key
                    }).ToList();
                foreach (Match match in BD.Matches)
                {
                    if (BD.Coefficients.Where(c => c.matchId == match.MatchId).ToList().Count == 0)
                    {
                        TeamInMatch tm1 = BD.TeamInMatches.FirstOrDefault(tm => tm.TeamInMatchId == match.FirstTeamId);
                        TeamInMatch tm2 = BD.TeamInMatches.FirstOrDefault(tm => tm.TeamInMatchId == match.SecondTeamId);
                        int teamsGoals1 = TeamGoalNums.FirstOrDefault(tgn => tgn.TeamId == tm1.TeamId) != null ? TeamGoalNums.FirstOrDefault(tgn => tgn.TeamId == tm1.TeamId).GoalsNum : 0;
                        int teamsGoals2 = TeamGoalNums.FirstOrDefault(tgn => tgn.TeamId == tm2.TeamId) != null ? TeamGoalNums.FirstOrDefault(tgn => tgn.TeamId == tm2.TeamId).GoalsNum : 0;
                        int RankDifference = Math.Abs(teamsGoals1 - teamsGoals2);
                        Coefficients coefficients = new Coefficients();
                        coefficients.matchId = match.MatchId;
                        switch (RankDifference)
                        {
                            case 0:
                                coefficients.drawCoef = 2;
                                coefficients.firstTeamCoef = 1.8;
                                coefficients.secondTeamCoef = 1.8;
                                break;
                            case 1:
                            case 2:
                                coefficients.drawCoef = 1.5;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 2;
                                    coefficients.secondTeamCoef = 3;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 3;
                                    coefficients.secondTeamCoef = 2;
                                }
                                break;
                            case 3:
                            case 4:
                                coefficients.drawCoef = 2.7;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 1.8;
                                    coefficients.secondTeamCoef = 2.7;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 2.7;
                                    coefficients.secondTeamCoef = 1.8;
                                }
                                break;
                            case 5:
                            case 6:
                                coefficients.drawCoef = 3.5;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 1.5;
                                    coefficients.secondTeamCoef = 2.5;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 2.5;
                                    coefficients.secondTeamCoef = 1.5;
                                }
                                break;
                            case 7:
                            case 8:
                                coefficients.drawCoef = 3.5;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 1.3;
                                    coefficients.secondTeamCoef = 2.4;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 2.4;
                                    coefficients.secondTeamCoef = 1.3;
                                }
                                break;
                            case 9:
                            case 10:
                                coefficients.drawCoef = 4;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 1.2;
                                    coefficients.secondTeamCoef = 2.3;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 2.3;
                                    coefficients.secondTeamCoef = 1.2;
                                }
                                break;
                            case 11:
                            case 12:
                                coefficients.drawCoef = 4;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 0.11;
                                    coefficients.secondTeamCoef = 0.22;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 2.2;
                                    coefficients.secondTeamCoef = 1.1;
                                }
                                break;
                            default:
                                coefficients.drawCoef = 4;
                                if (teamsGoals1 > teamsGoals2)
                                {
                                    coefficients.firstTeamCoef = 1.06;
                                    coefficients.secondTeamCoef = 2.2;
                                }
                                else
                                {
                                    coefficients.firstTeamCoef = 2.2;
                                    coefficients.secondTeamCoef = 1.06;
                                }
                                break;
                        }
                        using (Context b = new Context())
                        {
                            b.Coefficients.Add(coefficients);
                            b.SaveChanges();
                        }
                    }
                }
            }
        }
        public List<TeamTrainer> TeamRank()
        {
            List<PlayerCharacteristics> bp = GetPlayerCharacteristics();
            List<PlayerCharacteristics> bpch = bp.GroupBy(g => g.teamName).Select(g => new PlayerCharacteristics { teamName = g.Key, goals = g.Max(r => r.goals), ContrNum = g.Min(r => r.redCardsNum + r.yellowCardsNum) }).Where(g => g.goals != 0).ToList();
            var tt = (
                from t in db.Teams
                join tm in db.TeamInMatches on t.TeamId equals tm.TeamInMatchId
                join tr in db.Trainers on tm.TrainerId equals tr.TrainerId
                select new
                {
                    teamId = t.TeamId,
                    teamName = t.TeamName,
                    trainerName = tr.FullName
                }).ToList();
            List<TeamTrainer> teams = (from t in tt
                                       join bpbp in bp on t.teamName equals bpbp.teamName
                                       join b in bpch on t.teamName equals b.teamName
                                       where b.goals == bpbp.goals
                                       select new TeamTrainer
                                       {
                                           teamName = t.teamName,
                                           trainerName = t.trainerName,
                                           PlayerContrNum = b.ContrNum,
                                           PlayerGoalsNum = b.goals,
                                           bestPlayer = bpbp.playerName
                                       }
                    ).ToList();
            return teams;
        }
        public ActionResult ShowTeams()
        {
            return View(TeamRank());
        }
        public List<PlayerCharacteristics> GetPlayerCharacteristics()
        {
            List<MatchesPlayersTeams> Scores =
                  (from m in db.Matches
                   join tm1 in db.TeamInMatches on m.FirstTeamId equals tm1.TeamInMatchId
                   join tm2 in db.TeamInMatches on m.SecondTeamId equals tm2.TeamInMatchId
                   from pt in db.PlayersInTeams
                   join p in db.Players on pt.PlayerId equals p.PlayerId
                   join pp in db.PlayerPositions on pt.PositionId equals pp.PlayerPositionId
                   join t in db.Teams on pt.TeamId equals t.TeamId
                   join g in db.Goals on pt.PlayersInTeamId equals g.PlayerId into subg
                   from goal in subg.DefaultIfEmpty()
                   where goal.TeamInMatchId == tm1.TeamInMatchId || goal.TeamInMatchId == tm2.TeamInMatchId
                   select new MatchesPlayersTeams
                   {
                       playerId = p.PlayerId,
                       matchName = m.MatchName,
                       teamName = t.TeamName,
                       matchId = m.MatchId,
                       teamId = pt.TeamId,
                       playerName = p.FullName,
                       date = m.Date,
                       goalId = goal.GoalId,
                       position = pp.Name
                   }).ToList();
            List<MatchesPlayersTeams> Contrs =
            (from m in db.Matches
             join tm1 in db.TeamInMatches on m.FirstTeamId equals tm1.TeamInMatchId
             join tm2 in db.TeamInMatches on m.SecondTeamId equals tm2.TeamInMatchId
             from pt in db.PlayersInTeams
             join p in db.Players on pt.PlayerId equals p.PlayerId
             join pp in db.PlayerPositions on pt.PositionId equals pp.PlayerPositionId
             join t in db.Teams on pt.TeamId equals t.TeamId
             join cm in db.ContraventionsInMatchs on new { m.MatchId, p.PlayerId } equals new { cm.MatchId, cm.PlayerId } into subc
             from contr in subc.DefaultIfEmpty()
             where (tm1.TeamInMatchId == pt.TeamId || tm2.TeamInMatchId == pt.TeamId)
             select new MatchesPlayersTeams
             {
                 teamName = t.TeamName,
                 playerId = p.PlayerId,
                 playerName = p.FullName,
                 position = pp.Name,
                 contrType = contr.ContraventionId
             }).ToList();
            var PlayersGoals = (from Score in Scores
                                group Score by new { Score.playerId, Score.playerName, Score.teamName, Score.position } into h
                                select new { position = h.Key.position, playerId = h.Key.playerId, playerName = h.Key.playerName, teamName = h.Key.teamName, goals = h.Count(k => k.goalId != null) }).ToList();
            var RedCards = (from Contr in Contrs
                            group Contr by new { Contr.playerId, Contr.playerName, Contr.position, Contr.teamName } into h
                            select new { position = h.Key.position, playerId = h.Key.playerId, playerName = h.Key.playerName, teamName = h.Key.teamName, redCardsNum = h.Count(k => k.contrType != null && k.contrType == 1) }).ToList();
            var YellowCards = (from Contr in Contrs
                               group Contr by new { Contr.playerId, Contr.playerName, Contr.position, Contr.teamName } into h
                               select new { position = h.Key.position, playerId = h.Key.playerId, playerName = h.Key.playerName, teamName = h.Key.teamName, yellowCardsNum = h.Count(k => k.contrType != null && k.contrType == 2) }).ToList();

            List<PlayerCharacteristics> pc = (from g in PlayersGoals
                                              join rc in RedCards on new { g.playerId, g.position, g.teamName } equals new { rc.playerId, rc.position, rc.teamName }
                                              join yc in YellowCards on new { g.playerId, g.position, g.teamName } equals new { yc.playerId, yc.position, yc.teamName }
                                              select new PlayerCharacteristics
                                              {
                                                  playerId = g.playerId,
                                                  playerName = g.playerName,
                                                  teamName = g.teamName,
                                                  goals = g.goals,
                                                  redCardsNum = rc.redCardsNum,
                                                  yellowCardsNum = yc.yellowCardsNum,
                                                  position = g.position
                                              }).OrderByDescending(t => t.goals).ToList();
            return pc;
        }
        public ActionResult ShowPlayers()
        {
            return View(GetPlayerCharacteristics());
        }
        public ActionResult Tournament()
        {
            List<ShowTournaments> tournaments = db.Tournaments.Select(
                t => new ShowTournaments
                {
                    TournamentId = t.TournamentId,
                    Name = t.Name,

                    Stages = db.TournamentStages.Select(
                s => new TournamentStageDTO
                {
                    TournamentStageId = s.TournamentStageId,
                    Name = s.Name
                }).ToList()
                }).ToList();
            return View(tournaments);
        }
        public List<MatchScores> TeamScores1 = new List<MatchScores>();
        public List<MatchScores> TeamScores(int tournament, int stage)
        {
            List<MatchesPlayersTeams> Scores =
              (from m in db.Matches
               join tm in db.TeamInMatches on m.FirstTeamId equals tm.TeamInMatchId
               join pt in db.PlayersInTeams on tm.TeamInMatchId equals pt.TeamId
               join p in db.Players on pt.PlayerId equals p.PlayerId
               join t in db.Teams on tm.TeamId equals t.TeamId
               join g in db.Goals on new { m.MatchId, pt.PlayerId } equals new { g.MatchId, g.PlayerId } into subg
               from goal in subg.DefaultIfEmpty()
               where (m.TournamentId == tournament && m.TournamentStageId == stage)
               select new MatchesPlayersTeams
               {
                   matchName = m.MatchName,
                   teamName = t.TeamName,
                   matchId = m.MatchId,
                   teamId = pt.TeamId,
                   playerName = p.FullName,
                   date = m.Date,
                   goalId = goal.GoalId,
                   goalPeriod = goal.Period
               }).Distinct().ToList();
            List<MatchesPlayersTeams> Scores2 =
              (from m in db.Matches
               join tm in db.TeamInMatches on m.SecondTeamId equals tm.TeamInMatchId
               join pt in db.PlayersInTeams on tm.TeamInMatchId equals pt.TeamId
               join p in db.Players on pt.PlayerId equals p.PlayerId
               join t in db.Teams on tm.TeamId equals t.TeamId
               join g in db.Goals on new { m.MatchId, pt.PlayerId } equals new { g.MatchId, g.PlayerId } into subg
               from goal in subg.DefaultIfEmpty()
               where (m.TournamentId == tournament && m.TournamentStageId == stage)
               select new MatchesPlayersTeams
               {
                   matchName = m.MatchName,
                   teamName = t.TeamName,
                   matchId = m.MatchId,
                   teamId = pt.TeamId,
                   playerName = p.FullName,
                   date = m.Date,
                   goalId = goal.GoalId,
                   goalPeriod = goal.Period
               }).Distinct().ToList();
            //List<MatchesPlayersTeams> Scores;
            Scores.AddRange(Scores2);
            List<MatchScores> ms = (from Score in Scores
                                    group Score by new { Score.matchName, Score.teamId, Score.matchId, Score.teamName, Score.date } into h
                                    select new MatchScores
                                    {
                                        Date = h.Key.date,
                                        teamId = h.Key.teamId,
                                        matchId = h.Key.matchId,
                                        matchName = h.Key.matchName,
                                        teamName = h.Key.teamName,
                                        goals = h.Count(k => k.goalId != null)
                                    }).ToList();
            foreach (MatchScores m in ms)
            {
                if (m.goals != 0)
                {
                    m.extendedGoals = (from s in Scores
                                       where (s.teamName == m.teamName && s.matchName == m.matchName && s.date == m.Date && s.goalPeriod != null)
                                       select new ExtendedGoal
                                       {
                                           PlayerName = s.playerName,
                                           Period = s.goalPeriod,
                                           Team = s.teamName,
                                           TeamId = s.teamId
                                       }).ToList();
                }
            }
            return ms;
        }
        public ActionResult Matches(Match y)
        {
            List<ExtendedMatch> extendedMatches = new List<ExtendedMatch>();

            var sc = TeamScores(y.TournamentId, y.TournamentStageId);
            List<Scores> l = (from o in sc
                              join o2 in sc on o.matchName equals o2.matchName
                              join m in db.Matches on o.matchName equals m.MatchName
                              where o.teamName != o2.teamName && o.goals > o2.goals
                              select new Scores
                              {
                                  match = o.matchName,
                                  goals = (o.goals + ":" + o2.goals).ToString(),
                                  date = o.Date,
                                  matchId = o.matchId,
                                  firstTeamId = o.teamId,
                                  secondTeamId = o2.teamId,
                                  goalsMadeBy1 = o.extendedGoals,
                                  goalsMadeBy2 = o2.extendedGoals,
                              }).ToList();
            ExtendedMatch em = new ExtendedMatch();
            em.Scores = l;
            em.tournamentId = y.TournamentId;
            em.tournamentStageId = y.TournamentStageId;
            em.tournament = db.Tournaments.Where(t => t.TournamentId == 1).Select(t => t.Name).First();
            em.tournamentStage = db.TournamentStages.Where(s => s.TournamentStageId == y.TournamentStageId).Select(s => s.Name).First();

            extendedMatches.Add(em);

            return View(extendedMatches);
        }
        public ActionResult AboutMatches(Match m)
        {
            //List<ExtendedTeam> teams  = db.Teams.Select(
            //   t => new ExtendedTeam
            //   {
            //       TeamId = t.TeamId,
            //       TeamName = t.TeamName,

            //       Players = (from pt in db.PlayersInTeams
            //                  join p in db.Players on pt.PlayerId equals p.PlayerId
            //                  join pp in db.PlayerPositions on pt.PositionId equals pp.PlayerPositionId
            //                  where (pt.TeamId == t.TeamId)
            //                  select new ExtendedPlayer
            //                  {
            //                      PlayerId = p.PlayerId,
            //                      FullName = p.FullName,
            //                      PlayersInTeamId = pt.PlayersInTeamId,
            //                      PositionId = pp.PlayerPositionId,
            //                      TeamId = pt.TeamId,
            //                      Name = pp.Name
            //                  }).ToList()
            //   }).Where(t => t.TeamId == m.FirstTeamId || t.TeamId == m.SecondTeamId).ToList();
            return View();
        }
        public ActionResult AboutMatch(string matchIdScores)
        {
            int matchId = Convert.ToInt32(matchIdScores.Split('0')[0]);

            string goals = "";
            matchIdScores = matchIdScores.Remove(0, matchIdScores.Split('0')[0].Length);
            string[] goalsSplit = matchIdScores.Split('0');
            if(matchIdScores == "") goals = "0:0";
            else
            if (matchIdScores[matchIdScores.Length - 1] == '0')
            {
                if (matchIdScores.Length == 1)
                {
                    goals = "-";
                }
                else
                {
                    goals = goalsSplit[1] + ":" + goalsSplit[2];
                }
            }
            else
                if (matchIdScores[matchIdScores.Length - 1] == '1')
            {
                goals = goalsSplit[1][0] + ":0";
            }
            else
                if (matchIdScores[matchIdScores.Length - 1] == '2')
            {
                goals = "0:" + goalsSplit[1][0];
            }
            using (Context db = new Context())
            {
                Match m = new Match();
                m = db.Matches.FirstOrDefault(mm => mm.MatchId == matchId);
                TeamInMatch tm1 = db.TeamInMatches.FirstOrDefault(tm => tm.TeamInMatchId == m.FirstTeamId);
                TeamInMatch tm2 = db.TeamInMatches.FirstOrDefault(tm => tm.TeamInMatchId == m.SecondTeamId);
                List<PlayerFall> pf = GetMatchFalls(matchId);
                List<ExtendedPlayer> ep = GetPlayerInTeams();
                ExtendedTeam team = new ExtendedTeam
                {
                    tournament = db.Tournaments.FirstOrDefault(t => t.TournamentId == m.TournamentId) != null ? db.Tournaments.FirstOrDefault(t => t.TournamentId == m.TournamentId).Name : "",
                    Stage = db.TournamentStages.FirstOrDefault(s => s.TournamentStageId == m.TournamentStageId) != null? db.TournamentStages.FirstOrDefault(s => s.TournamentStageId == m.TournamentStageId).Name : "",
                    Judge = db.Judges.FirstOrDefault(
                        j => j.JudgeId == (db.JudgesInMatchs.FirstOrDefault(jm => jm.MatchId == m.MatchId) !=null ? db.JudgesInMatchs.FirstOrDefault(jm => jm.MatchId == m.MatchId).JudgeId : 1)).FullName,
                    status = m.Status,
                    scores = goals,
                    date = m.Date,
                    Team1 = db.Teams.FirstOrDefault(t => t.TeamId == tm1.TeamId) != null ? db.Teams.FirstOrDefault(t => t.TeamId == tm1.TeamId).TeamName : "",
                    Team2 = db.Teams.FirstOrDefault(t => t.TeamId == tm2.TeamId) != null ? db.Teams.FirstOrDefault(t => t.TeamId == tm2.TeamId).TeamName : "",
                    trainer1 = db.Trainers.FirstOrDefault(t => t.TrainerId == tm1.TrainerId) != null ? db.Trainers.FirstOrDefault(t => t.TrainerId == tm1.TrainerId) : new Trainer(),
                    trainer2 = db.Trainers.FirstOrDefault(t => t.TrainerId == tm2.TrainerId) != null ? db.Trainers.FirstOrDefault(t => t.TrainerId == tm2.TrainerId) : new Trainer(),
                    Players1 = (from p in ep
                                where p.TeamId == m.FirstTeamId
                                select new ExtendedPlayer
                                {
                                    PlayerId = p.PlayerId,
                                    FullName = p.FullName,
                                    PlayersInTeamId = p.PlayersInTeamId,
                                    PositionId = p.PositionId,
                                    PositionName = p.Name,
                                    TeamId = p.TeamId,
                                    Name = p.Name
                                }).ToList(),
                    Players2 = (from p in ep
                                           where p.TeamId == m.SecondTeamId
                                           select new ExtendedPlayer
                                           {
                                               PlayerId = p.PlayerId,
                                               FullName = p.FullName,
                                               PlayersInTeamId = p.PlayersInTeamId,
                                               PositionId = p.PositionId,
                                               PositionName = p.Name,
                                               TeamId = p.TeamId,
                                               Name = p.Name
                                           }).ToList(),
                    Goals1 = (from p in db.Players
                              join pt in db.PlayersInTeams on p.PlayerId equals pt.PlayerId
                              join g in db.Goals on pt.TeamId equals g.TeamInMatchId
                              where g.MatchId == matchId && g.PlayerId == pt.PlayersInTeamId && pt.TeamId == m.FirstTeamId
                              select new ExtendedGoal
                              {
                                  PlayerName = p.FullName,
                                  Time = g.Time,
                                  Period = g.Period

                              }).ToList(),
                    Goals2 = (from p in db.Players
                              join pt in db.PlayersInTeams on p.PlayerId equals pt.PlayerId
                              join g in db.Goals on pt.TeamId equals g.TeamInMatchId
                              where g.MatchId == matchId && g.PlayerId == pt.PlayersInTeamId && pt.TeamId == m.SecondTeamId
                              select new ExtendedGoal
                              {
                                  PlayerName = p.FullName,
                                  Time = g.Time,
                                  Period = g.Period

                              }).ToList(),
                    Contraventions1 = (from p in pf
                                       where p.TeamId == m.FirstTeamId
                                       select new ExtendedContravention
                                       {
                                           PlayerName = p.PlayerName,
                                           Name = p.ContraventionName,
                                           Time = p.Time,
                                           Period = p.Period
                                       }).ToList(),
                    Contraventions2 = (from p in pf
                                       where p.TeamId == m.SecondTeamId
                                       select new ExtendedContravention
                                       {
                                           PlayerName = p.PlayerName,
                                           Name = p.ContraventionName,
                                           Time = p.Time,
                                           Period = p.Period
                                       }).ToList(),

                };
                return View("AboutMatches", team);
            }
            
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
                                       Time = cm.Time,
                                       Period = cm.Period,
                                       ContraventionId = c.ContraventionId,
                                       ContraventionsInMatchId = cm.ContraventionsInMatchId,
                                       TeamId = pt.TeamId,
                                       PlayerName = p.FullName,
                                       PlayerId = p.PlayerId
                                   }).ToList();
            return pf;
        }
        public List<ExtendedPlayer> GetPlayerInTeams()
        {
            Context db = new Context();
            List<ExtendedPlayer> Ept = (from tm in db.TeamInMatches
                                              join pt in db.PlayersInTeams on tm.TeamInMatchId equals pt.TeamId
                                              join p in db.Players on pt.PlayerId equals p.PlayerId
                                              join pp in db.PlayerPositions on pt.PositionId equals pp.PlayerPositionId
                                              select new ExtendedPlayer
                                              {
                                                  PlayerId = p.PlayerId,
                                                  FullName = p.FullName,
                                                  PlayersInTeamId = pt.PlayersInTeamId,
                                                  PositionId = pp.PlayerPositionId,
                                                  PositionName = pp.Name,
                                                  TeamId = pt.TeamId,
                                                  Name = pp.Name

                                              }).ToList();
            return Ept;
        }
    }
}