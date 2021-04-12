using IPLFranchise2021.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021
{
    public static class BatsmanExtension
    {
        public static IEnumerable<Batsman> ToBat(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {

                var columns = line.Split(',');

                yield return new Batsman
                {
                    BatsmanName = columns[0],
                    FielderDetails = columns[1],
                    Runs = int.Parse(columns[2]),
                    Balls = int.Parse(columns[3]),
                    SR = double.Parse(columns[4]),
                    Fours = int.Parse(columns[5]),
                    Sixes = int.Parse(columns[6]),
                };
            }
        }

        public static IEnumerable<BowlSide> ToBowl(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {

                var columns = line.Split(',');

                yield return new BowlSide
                {
                    BowlerName = columns[0],
                    Overs = double.Parse(columns[1]),
                    OverRuns = int.Parse(columns[2]),
                    Wickets = int.Parse(columns[3]),
                    Econ = double.Parse(columns[4]),
                    Dot = int.Parse(columns[5]),
                    Maiden = int.Parse(columns[6]),
                    HatTrick = Convert.ToBoolean(columns[7])
                };
            }
        }

        public static IEnumerable<IPLSchedule> ToSchedule(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {

                var columns = line.Split(',');

                yield return new IPLSchedule
                {
                    Date = columns[0].Trim(),
                    Match = columns[1].Trim(),
                    Time = columns[2].Trim(),
                    Venue = columns[3].Trim(),
                    MatchNo = int.Parse(columns[5])
                };
            }
        }

        public static IEnumerable<TeamListName> ToNameList(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {

                var columns = line.Split(',');

                yield return new TeamListName
                {
                    Name = columns[0].Trim(),
                    Team = columns[1].Trim(),
                };
            }
        }
    }
}
