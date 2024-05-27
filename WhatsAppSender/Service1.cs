using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WhatsAppSender.DbModel;
using WhatsAppSender.Settings;

namespace WhatsAppSender
{
    public partial class Service1 : ServiceBase
    {
        private string connectionString;
        Timer timer = new Timer(); // name space(using System.Timers;)
        public Service1()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            //WriteToFile("Service is started at " + DateTime.Now);
            int timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);

            PerformAction().GetAwaiter().GetResult();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = timerInterval * 1000; //number in milisecinds
            timer.Enabled = true;

        }

        protected override void OnStop()
        {
            //WriteToFile("Service is stopped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            PerformAction().GetAwaiter().GetResult();
        }



        private async Task<List<Queue>> PerformAction()
        {
            try
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
            catch (Exception ex)
            {
                string path = @"C:\WsSendingLogs\";
                string fileName = DateTime.Now.ToString("yyyy-dd-M");
                string fullPath = $"{path}{fileName}.txt";

                using (StreamWriter sw = File.AppendText(fullPath))
                {
                    sw.WriteLine($"{ex.ToString()}");
                }
                throw ex;
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

                result = (await connection.QueryAsync<WhatsAppSender.DbModel.Settings>(sql)).ToDictionary(x => x.Key, x => x.Value);
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
                }, Newtonsoft.Json.Formatting.Indented);

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
                var sql = $"UPDATE Queue Set Sent= 1, SentDate='{DateTime.Now.ToString("MM/dd/yyyy")}' WHERE Id = {id}";
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync(sql);
                connection.Close();
            }
        }


















        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
