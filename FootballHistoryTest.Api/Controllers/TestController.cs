// using System;
// using System.Collections.Generic;
// using FootballHistoryTest.Api.Domain;
// using Microsoft.AspNetCore.Mvc;
//
// namespace FootballHistoryTest.Api.Controllers
// {
//     [Route("api/[controller]")]
//     public class TestController : Controller
//     {
//         public class TestLeaguePosition
//         {
//             public DateTime Date { get; set; }
//             public int Position { get; set; }
//         }
//
//         public class TestHistoricalPosition
//         {
//             public TestSeason Season { get; set; }
//             public int AbsolutePosition { get; set; }
//             public string Status { get; set; }
//         }
//         
//         [HttpGet("[action]")]
//         public TestLeagueTable GetLeagueTable(int tier, int startYear)
//         {
//             return new TestLeagueTable();
//         }
//         
//         [HttpGet("[action]")]
//         public List<TestLeaguePosition> GetLeaguePositions(int tier, int startYear, string team)
//         {
//             return new List<TestLeaguePosition>();
//         }
//         
//         [HttpGet("[action]")]
//         public List<TestHistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team)
//         {
//             return new List<TestHistoricalPosition>();
//         }
//     }
// }