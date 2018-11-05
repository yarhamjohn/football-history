using System.Collections.Generic;

public class LeagueTable
    {
        public string Competition { get; set; }
        public string Season { get; set; }
        public List<LeagueTableRow> LeagueTableRow { get; set; }
    }