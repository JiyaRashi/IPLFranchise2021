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
            string connectionString = @"Data Source = localhost\SQLEXPRESS;Trusted_Connection=True; Initial Catalog = FPL_DB;";
           
            using (var conn = new SqlConnection(connectionString))
            {
                string insertFPLTotoalPoints = "INSERT INTO FPLTeamPoints (TeamName, TeamPoints,Date,MatchNo,Match,Matchyear) " +
               "VALUES (@TeamName,@TeamPoints,@Date,@MatchNo,@Match,@Matchyear)";
                
                    foreach (var item in FPLTeamPoints)
                    {
                    var query = new SqlCommand(insertFPLTotoalPoints, conn);
                    //query.Parameters.Add("@ID", SqlDbType.Int, 150).Value =;
                        query.Parameters.AddWithValue("@TeamName", item.FPLTeam);
                        query.Parameters.AddWithValue("@TeamPoints", item.Points);
                        query.Parameters.AddWithValue("@Date", IPLSchedule.Date);
                        query.Parameters.AddWithValue("@MatchNo", IPLSchedule.MatchNo);
                        query.Parameters.AddWithValue("@Match", IPLSchedule.Match);
                        query.Parameters.AddWithValue("@Matchyear", 2021);
                    conn.Close();
                    conn.Open();

                    query.ExecuteNonQuery();
                    }
                
                
                conn.Close();

            }
        }
    }
}
