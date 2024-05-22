using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using wsSendingService.DbModel;

namespace wsSendingService
{
    public class DataJob : IJob
    {
        private string connectionString;

        public async Task Execute(IJobExecutionContext context)
        {
            connectionString = ConfigurationManager.ConnectionStrings["WsSendingSqlConn"].ConnectionString;
            await GetQueueData();
        }
        private async Task<List<Queue>> GetQueueData()
        {
           
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var sql = "SELECT * FROM Queue";
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<Queue>(sql);
  
                    connection.Close();
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //int timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);
            //string fileName = @"G:\Saiful\Fiverr\gllbertoanlno\doc\file.txt";

            //using (FileStream fs = File.OpenWrite(fileName))
            //{
            //    Byte[] info = new UTF8Encoding(true)
            //                         .GetBytes($"DateTime: {DateTime.Now.ToString()}");

            //    fs.Write(info, 0, info.Length);
            //}


            //if (!File.Exists(fileName))
            //{
            //    using (StreamWriter sw = File.CreateText(fileName))
            //    {
            //        sw.WriteLine("DateTime: {0}", DateTime.Now.ToString());
            //    }
            //    System.IO.File.Create("G:\\Saiful\\Fiverr\\gllbertoanlno\\doc\\" + "file.txt");
            //}

            //using (FileStream sw = File.OpenWrite(fileName))
            //{
            //    sw.WriteLine("DateTime: {0}", DateTime.Now.ToString());
            //}


            //System.IO.File.Create("G:\\Saiful\\Fiverr\\gllbertoanlno\\doc\\" + "file.txt");
            // Insert data into SQL Server
            // Example code to insert data...
            // using SqlConnection, SqlCommand, etc.
        }
            
    }
}

