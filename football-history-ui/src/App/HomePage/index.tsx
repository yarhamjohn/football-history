import React, { FunctionComponent } from "react";

const HomePage: FunctionComponent = () => {
  return (
    <div>
      <h1>History of the English Football League</h1>
      <p>
        This application currently provides league tables and match results for each of the top 4
        divisions in the English Football League since the 1989-90 season. Further data will be
        added covering the Football League all the way back to 1888 when it was founded, as well as
        pages dedicated to club and league histories.
      </p>

      <h3>League structure</h3>
      <ul>
        <li>
          The Football league was formed in 1888 and initially consisted of a single division, the
          First Division.
        </li>
        <li>A second tier, the Second Division, was added to the league for the 1892-93 season.</li>
        <li>For the 1920-21 season, a third tier, the Third Division was added to the League.</li>
        <li>
          The following year, the Third Division was split into North and South divisions,
          effectively adding a whole new division.
        </li>
        <li>
          In 1958-59, the Third Division North/South split was converted into a single Third
          Division and a new Fourth Division.
        </li>
        <li>
          The 1992-93 season saw the breakaway of the top division to form the new Premier League.
          Divisions Two, Three and Four were renamed to Divisions One, Two and Three.
        </li>
        <li>
          In 2004, Divisions One, Two and Three were renamed to the Championship, League One and
          League Two.
        </li>
      </ul>

      <h3>Rules and regulations</h3>
      <h4>
        <em>Points</em>
      </h4>
      <ul>
        <li>
          From the inception of the league in 1888, 2 points were awarded for a win and 1 for a
          draw.
        </li>
        <li>
          This was changed to 3 points for a win and 1 point for a draw from the 1981-82 season.
        </li>
      </ul>
      <h4>
        <em>Rankings</em>
      </h4>
      <ul>
        <li>
          Initially, 'Goal Average' (actually goal ratio - goals for / goals against) was used to
          separate teams level on points.
        </li>
        <li>For the 1976-77 season, this was replaced with Goal Difference.</li>
        <li>
          After the formation of the Premier League in 1992-93, the Football League used Goals For
          before Goal difference up to the 1998-99 season before changing back from 1999-00
        </li>
      </ul>
      <h4>
        <em>Promotion and Relegation</em>
      </h4>
      <ul>
        <li>
          From 1892-93 to 1897-98, test matches were used to determine promotion and relegation
          between the First and Second Divisions.
        </li>
        <li>
          These test matches took the form of a mini-league with each team from the First Division
          playing 2-legged ties against each team in the Second Division.
        </li>
        <li>
          First Division teams winning the mini-league retained their status, whilst Second Division
          teams losing the mini-league stayed in the Second Division.
        </li>
        <li>
          Second Division teams winning the mini-league had to apply for entry to the First Division
          whilst First Division teams losting the mini-league were offered a place in the Second
          Division.
        </li>
      </ul>
      <ul>
        <li>
          From 1888-89 to 1985-86, a system of election/re-election was used to determine if clubs
          finishing bottom of the lowest division should be replaced in the League with non-League
          clubs.
        </li>
        <li>
          Clubs in the bottom division of the League were required to reapply if they finished in
          the:
          <ul>
            <li>bottom 4 (1888-89 to 1892-93)</li>
            <li>bottom 3 (1893-94 to 1920-21)</li>
            <li>bottom 2 of either Division Three North or South (1921-22 to 1957-58)</li>
            <li>bottom 4 (1958-59 - 1985-86)</li>
          </ul>
        </li>
        <li>Since 1986-87, the election system has been replaced with automatic relegation.</li>
      </ul>
      <ul>
        <li>
          Play-offs to determine which additional team would be promoted from each division were
          introduced from the 1986-87 season.
        </li>
        <li>
          Initially, these involved one team from the higher division and 3 from the lower division.
        </li>
        <li>
          From 1988-89 however, the play-offs were made up of 4 teams from the lower division only.
        </li>
        <li>From the 1989-90 season, the play-off final was changed from 2-legged to 1-legged.</li>
      </ul>
    </div>
  );
};

export { HomePage };
