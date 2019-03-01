using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class LeagueTableBuilderTests
    {
        private LeagueTableBuilder _leagueTableBuilder;
        
        [SetUp]
        public void SetUp()
        {
            _leagueTableBuilder = new LeagueTableBuilder();
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithNoRows_GivenNoMatches()
        {
            var leagueMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Build_ShouldReturnLeagueTableWithTwoRows_GivenOneMatch()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithTwoRows_GivenAHomeAndAwayPairOfMatchesBetweenTheSameTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team2", AwayTeam = "Team1" }
            };
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithThreeRows_GivenTwoMatchesBetweenThreeTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team3" }
            };
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(3));
        }
        
        [Test]
        public void Build_ShouldThrowAnException_GivenTwoMatchesWithTheSameHomeAndAwayTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
                
        [Test]
        public void Build_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" }
            };
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenOneDraw()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 0,
                    AwayGoals = 0
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 1,
                        Won = 0,
                        Lost = 0,
                        Drawn = 1
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 0,
                        Lost = 0,
                        Drawn = 1
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenOneHomeWin()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 1,
                    AwayGoals = 0
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 1,
                        Won = 1,
                        Lost = 0,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 0,
                        Lost = 1,
                        Drawn = 0
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
                        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenOneAwayWin()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 0,
                    AwayGoals = 1
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 1,
                        Won = 0,
                        Lost = 1,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 1,
                        Lost = 0,
                        Drawn = 0
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenTwoDraws()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 0,
                    AwayGoals = 0
                },
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team3",
                    HomeGoals = 1,
                    AwayGoals = 1
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 2,
                        Won = 0,
                        Lost = 0,
                        Drawn = 2
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 0,
                        Lost = 0,
                        Drawn = 1
                    },
                    new LeagueTableRow
                    {
                        Team = "Team3",
                        Played = 1,
                        Won = 0,
                        Lost = 0,
                        Drawn = 1
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenTwoWins()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 1,
                    AwayGoals = 0
                },
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team3",
                    HomeGoals = 1,
                    AwayGoals = 0
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 2,
                        Won = 2,
                        Lost = 0,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 0,
                        Lost = 1,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team3",
                        Played = 1,
                        Won = 0,
                        Lost = 1,
                        Drawn = 0
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenTwoDefeats()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 0,
                    AwayGoals = 1
                },
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team3",
                    HomeGoals = 0,
                    AwayGoals = 1
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 2,
                        Won = 0,
                        Lost = 2,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 1,
                        Lost = 0,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team3",
                        Played = 1,
                        Won = 1,
                        Lost = 0,
                        Drawn = 0
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
                
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenOneWinAndOneDefeat()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 1,
                    AwayGoals = 0
                },
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team3",
                    HomeGoals = 0,
                    AwayGoals = 1
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 2,
                        Won = 1,
                        Lost = 1,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 0,
                        Lost = 1,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team3",
                        Played = 1,
                        Won = 1,
                        Lost = 0,
                        Drawn = 0
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
                
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenOneWinAndOneDraw()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 1,
                    AwayGoals = 0
                },
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team3",
                    HomeGoals = 1,
                    AwayGoals = 1
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 2,
                        Won = 1,
                        Lost = 0,
                        Drawn = 1
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 0,
                        Lost = 1,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team3",
                        Played = 1,
                        Won = 0,
                        Lost = 0,
                        Drawn = 1
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
                        
        [Test]
        public void Build_ShouldReturnCorrectLeagueTableRows_GivenOneDefeatAndOneDraw()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team2",
                    HomeGoals = 0,
                    AwayGoals = 1
                },
                new MatchDetailModel
                {
                    HomeTeam = "Team1",
                    AwayTeam = "Team3",
                    HomeGoals = 1,
                    AwayGoals = 1
                }
            };
            
            var actualLeagueTable = _leagueTableBuilder.Build(leagueMatches);
            var expectedLeagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow
                    {
                        Team = "Team1",
                        Played = 2,
                        Won = 0,
                        Lost = 1,
                        Drawn = 1
                    },
                    new LeagueTableRow
                    {
                        Team = "Team2",
                        Played = 1,
                        Won = 1,
                        Lost = 0,
                        Drawn = 0
                    },
                    new LeagueTableRow
                    {
                        Team = "Team3",
                        Played = 1,
                        Won = 0,
                        Lost = 0,
                        Drawn = 1
                    }
                }
            };
            
            Assert.That(RowsContainCorrectCumulativeResults(actualLeagueTable, expectedLeagueTable), Is.True);
        }
        
        private static bool RowsContainCorrectCumulativeResults(LeagueTab actualLeagueTable, LeagueTab expectedLeagueTable)
        {
            if (actualLeagueTable.Rows.Count != expectedLeagueTable.Rows.Count)
            {
                return false;
            }

            var actualLeagueRows = actualLeagueTable.Rows.OrderBy(r => r.Team).ToList();
            var expectedLeagueRows = expectedLeagueTable.Rows.OrderBy(r => r.Team).ToList();

            for (var i = 0; i < actualLeagueRows.Count; i++)
            {
                var actual = actualLeagueRows[i];
                var expected = expectedLeagueRows[i];
                if (actual.Team != expected.Team 
                    || actual.Played != expected.Played 
                    || actual.Won != expected.Won 
                    || actual.Lost != expected.Lost 
                    || actual.Drawn != expected.Drawn)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
