using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using wsSendingService.DbModel;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using wsSendingService.Settings;
using Newtonsoft.Json;
using System.IO;

namespace wsSendingService
{
    public class DataJob : IJob
    {
        private string connectionString;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await PerformAction();
            }
            catch (Exception ex)
            {
                string path = @"C:\WsSendingLogs\";
                string fileName = DateTime.Now.ToString("yyyy-dd-M");
                string fullPath = $"{path}{fileName}.txt";

                using (StreamWriter sw = File.AppendText(fullPath))
                {
                    sw.WriteLine($"{ex.ToString()}");
                }
            }
        }
        private async Task<List<Queue>> PerformAction()
        {
            connectionString = ConfigurationManager.ConnectionStrings["WsSendingSqlConn"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM Queue Q WHERE Q.Sent = 0";
                await connection.OpenAsync();

                var result = await connection.QueryAsync<Queue>(sql);
  
                connection.Close();
                await SendMessage(result.ToList());
                return result.ToList();
            }
        }

        private async Task SendMessage(List<Queue> queues)
        {
            string accountSid = string.Empty;
            string authToken = string.Empty;

            var result = new Dictionary<string, string>();

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM Settings";
                await connection.OpenAsync();

                result = (await connection.QueryAsync<DbModel.Settings>(sql)).ToDictionary(x=> x.Key, x=> x.Value);
                connection.Close();
            }

            accountSid = result[SettingConst.TwilioWhatsAppSid];
            authToken = result[SettingConst.TwilioWhatsAppToken];

            TwilioClient.Init(accountSid, authToken);

            foreach (var item in queues)
            {
                var contentVariables = JsonConvert.SerializeObject(new Dictionary<string, Object>()
                {
                    //{"1", item.Message},
                    {"1", item.AttachmentPath}
                }, Formatting.Indented);

                var message = MessageResource.Create(
                    contentSid: result[SettingConst.TwilioWhatsAppFileAttachmentSid],
                    messagingServiceSid: result[SettingConst.TwilioWhatsAppMessagingServiceSid],
                    contentVariables: contentVariables,
                    from: new PhoneNumber("whatsapp:+50766640537"),
                    to: new PhoneNumber($"whatsapp:+{item.PhoneNumber}")
                );
                await UpdateQueueById(item.Id);
            }
        } 
        
        private async Task UpdateQueueById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = $"UPDATE Queue Set Sent= 1, SentDate={DateTime.Now} WHERE Id = {id}";
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync(sql);
                connection.Close();
            }
        }
    }
}

