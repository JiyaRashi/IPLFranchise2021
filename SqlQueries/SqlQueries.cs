using IPLFranchise2021.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace IPLFranchise2021
{
    public interface ISqlQueries
    {
        void InsertDBFPLTeamTotalPoints(ObservableCollection<FPLTeamTotalPoints> FPLTeamPoints, IPLSchedule iPLSchedule);
    }
    public class SqlQueries : ISqlQueries
    {
        public void InsertDBFPLTeamTotalPoints(ObservableCollection<FPLTeamTotalPoints> FPLTeamPoints, IPLSchedule IPLSchedule)
        {
            string connectionString = @"Data Source = localhost\SQLEXPRESS;Trusted_Connection=True; Initial Catalog = FPL_DB;";


            using (var conn = new SqlConnection(connectionString))
            {
                // string selectFPLMatch = "Select MatchNo from FPLTeamPoints where MatchNo=@MatchNo";
                // string insertFPLTotoalPoints = "INSERT INTO FPLTeamPoints (TeamName, TeamPoints,Date,MatchNo,Match,Matchyear) " +
                //"VALUES (@TeamName,@TeamPoints,@Date,@MatchNo,@Match,@Matchyear)";
                //SqlCommand SelectMatchNoQuery = new SqlCommand(selectFPLMatch, conn);
                //SelectMatchNoQuery.Parameters.Clear();
                //SelectMatchNoQuery.Parameters.AddWithValue("@MatchNo", IPLSchedule.MatchNo);
                SqlCommand cmd = new SqlCommand("SP_GetMatchNo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                IDbDataParameter IniParameter = cmd.CreateParameter();
                IniParameter.ParameterName = "@MatchNo";
                IniParameter.Value = IPLSchedule.MatchNo;

                IDbDataParameter ReturnParameter = cmd.CreateParameter();
                ReturnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(IniParameter);
                cmd.Parameters.Add(ReturnParameter);

                // SqlParameter parm = new SqlParameter("@return", SqlDbType.Int);
                //parm.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(parm);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                int count =(int)ReturnParameter.Value; 
                // string UpdateFPLTotoalPointsQuery = "Update FPLTeamPoints SET TeamName=TeamName, TeamPoints=@TeamPoints,Date=@Date,MatchNo=@MatchNo,Match=@Match,Matchyear=@Matchyear where MatchNo=@MatchNo";
                // SqlCommand insertquery = new SqlCommand(insertFPLTotoalPoints, conn);
                // SqlCommand updateQuery = new SqlCommand(UpdateFPLTotoalPointsQuery, conn);
                // updateQuery.Parameters.AddWithValue("@MatchNo", IPLSchedule.MatchNo);

                // SqlCommand query = result > 0 ? updateQuery : insertquery;

                string Sp_Insert = "SP_InsertFPLTotalPoints";
                string Sp_Update = "UpdateFPLTotalPoints";
                //Stored Procedure name   
                string Sp_Name = count > 0 ? Sp_Update : Sp_Insert;
                SqlCommand query = new SqlCommand(Sp_Name, conn);  //creating  SqlCommand  object  
                query.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  

                foreach (var item in FPLTeamPoints)
                {
                    query.Parameters.Clear();
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
                    conn.Close();

                }


                conn.Close();

            }
        }
    }
}
