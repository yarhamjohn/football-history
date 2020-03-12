using FootballHistoryTest.Api.Repositories.Match;

namespace FootballHistoryTest.Api.Calculators
{
    public static class MatchCalculator
    {
        public static bool MatchInvolvesTeam(MatchModel match, string team)
        {
            return match.HomeTeam == team || match.AwayTeam == team;
        }
        
        public static string? GetMatchWinner(MatchModel matchModel)
        {
            if (matchModel.HomeGoals > matchModel.AwayGoals)
            {
                return matchModel.HomeTeam;
            }

            if (matchModel.HomeGoals < matchModel.AwayGoals)
            {
                return matchModel.AwayTeam;
            }

            if (matchModel.HomeGoalsExtraTime > matchModel.AwayGoalsExtraTime)
            {
                return matchModel.HomeTeam;
            }

            if (matchModel.HomeGoalsExtraTime < matchModel.AwayGoalsExtraTime)
            {
                return matchModel.AwayTeam;
            }

            if (matchModel.HomePenaltiesScored > matchModel.AwayPenaltiesScored)
            {
                return matchModel.HomeTeam;
            }

            if (matchModel.HomePenaltiesScored < matchModel.AwayPenaltiesScored)
            {
                return matchModel.AwayTeam;
            }

            return null;
        }

        public static bool TeamWonMatch(MatchModel match, string team)
        {
            return match.HomeTeam == team && HomeTeamWon(match) ||
                   match.AwayTeam == team && AwayTeamWon(match);
        }
        
        public static bool TeamLostMatch(MatchModel match, string team)
        {
            return match.HomeTeam == team && AwayTeamWon(match) ||
                   match.AwayTeam == team && HomeTeamWon(match);
        }
        
        public static bool TeamDrewMatch(MatchModel match, string team)
        {
            return !TeamWonMatch(match, team) && !TeamLostMatch(match, team);
        }

        private static bool HomeTeamWon(MatchModel match)
        {
            return match.HomeGoals > match.AwayGoals
                   || match.HomeGoalsExtraTime > match.AwayGoalsExtraTime
                   || match.HomePenaltiesScored > match.AwayPenaltiesScored;
        }
        
        private static bool AwayTeamWon(MatchModel match)
        {
            return match.HomeGoals < match.AwayGoals
                   || match.HomeGoalsExtraTime < match.AwayGoalsExtraTime
                   || match.HomePenaltiesScored < match.AwayPenaltiesScored;
        }
    }
}