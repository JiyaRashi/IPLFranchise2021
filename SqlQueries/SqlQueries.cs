using IPLFranchise2021.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace IPLFranchise2021
{
    public interface ISqlQueries
    {
        void InsertDBFPLTeamTotalPoints(ObservableCollection<FPLTeamTotalPoints> FPLTeamPoints,IPLSchedule iPLSchedule);
    }
    public class SqlQueries: ISqlQueries
    {
        public void InsertDBFPLTeamTotalPoints(ObservableCollection<FPLTeamTotalPoints> FPLTeamPoints,IPLSchedule IPLSchedule)
        {
            SqlConnection conn;
            string connectionString = @"Data Source = localhost\SQLEXPRESS;Trusted_Connection=True; Initial Catalog = FPL_DB;";
           
            SqlConnection connection = new SqlConnection(connectionString);
           

            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryInsert = "INSERT INTO FPLTeamPoints (TeamName, TeamPoints,Date,MatchNo,Match,Matchyear) " +
               "VALUES (@TeamName,@TeamPoints,@Date,@MatchNo,@Match,@Matchyear)";
                using (SqlCommand query = new SqlCommand(queryInsert))
                {
                    query.Connection = conn;

                    foreach (var item in FPLTeamPoints)
                    {
                        //query.Parameters.Add("@ID", SqlDbType.Int, 150).Value =;
                        query.Parameters.Add("@TeamName", SqlDbType.NVarChar, 150).Value = item.FPLTeam;
                        query.Parameters.Add("@TeamPoints", SqlDbType.Int, 150).Value = item.Points;
                        query.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = IPLSchedule.Date;
                        query.Parameters.Add("@MatchNo", SqlDbType.Int, 150).Value = IPLSchedule.MatchNo;
                        query.Parameters.Add("@Match", SqlDbType.NVarChar, 250).Value = IPLSchedule.Match;
                        query.Parameters.Add("@Matchyear", SqlDbType.Int, 250).Value = 2021;
                        query.ExecuteNonQuery();
                    }
                }
                conn.Close();

            }
        }
    }
}
