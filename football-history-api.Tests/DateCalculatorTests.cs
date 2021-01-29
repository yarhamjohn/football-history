using System;
using football.history.api.Calculators;
using NUnit.Framework;

namespace football.history.api.Tests
{
    [TestFixture]
    public class DateCalculatorTests
    {
        public class SeasonEndDateTests
        {
            [Test]
            public void GetSeasonEndDate_Returns_June_30th_Given_Start_Year_Before_2019()
            {
                var seasonEndDate = DateCalculator.GetSeasonEndDate(2018);
                Assert.That(seasonEndDate, Is.EqualTo(new DateTime(2019, 06, 30)));
            }

            [Test]
            public void GetSeasonEndDate_Returns_August_20th_Given_Start_Year_Of_2019()
            {
                var seasonEndDate = DateCalculator.GetSeasonEndDate(2019);
                Assert.That(seasonEndDate, Is.EqualTo(new DateTime(2020, 08, 20)));
            }

            [Test]
            public void GetSeasonEndDate_Returns_June_30th_Given_Start_Year_After_2019()
            {
                var seasonEndDate = DateCalculator.GetSeasonEndDate(2020);
                Assert.That(seasonEndDate, Is.EqualTo(new DateTime(2021, 06, 30)));
            }
        }

        public class SeasonStartYearTests
        {
            [Test]
            public void GetSeasonStartYear_Returns_2018_Given_June_30th_2019()
            {
                var seasonStartYear =
                    DateCalculator.GetSeasonStartYear(new DateTime(2019, 06, 30));
                Assert.That(seasonStartYear, Is.EqualTo(2018));
            }

            [Test]
            public void GetSeasonStartYear_Returns_2019_Given_July_1st_2019()
            {
                var seasonStartYear =
                    DateCalculator.GetSeasonStartYear(new DateTime(2019, 07, 01));
                Assert.That(seasonStartYear, Is.EqualTo(2019));
            }

            [Test]
            public void GetSeasonStartYear_Returns_2019_Given_August_20th_2020()
            {
                var seasonStartYear =
                    DateCalculator.GetSeasonStartYear(new DateTime(2020, 08, 20));
                Assert.That(seasonStartYear, Is.EqualTo(2019));
            }

            [Test]
            public void GetSeasonStartYear_Returns_2020_Given_August_21st_2020()
            {
                var seasonStartYear =
                    DateCalculator.GetSeasonStartYear(new DateTime(2020, 08, 21));
                Assert.That(seasonStartYear, Is.EqualTo(2020));
            }

            [Test]
            public void GetSeasonStartYear_Returns_2020_Given_June_30th_2021()
            {
                var seasonStartYear =
                    DateCalculator.GetSeasonStartYear(new DateTime(2021, 06, 30));
                Assert.That(seasonStartYear, Is.EqualTo(2020));
            }

            [Test]
            public void GetSeasonStartYear_Returns_2021_Given_July_1st_2021()
            {
                var seasonStartYear =
                    DateCalculator.GetSeasonStartYear(new DateTime(2021, 07, 01));
                Assert.That(seasonStartYear, Is.EqualTo(2021));
            }
        }
    }
}
