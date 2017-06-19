using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arvato.CRM.CommunicateEngine
{
    public partial class CommunicateService : ServiceBase
    {
        //private System.Timers.Timer mailTimer;
        private System.Timers.Timer smsSendTimer;
        //private System.Timers.Timer smsSendBatchTimer;
        //private System.Timers.Timer smsReceiveTimer;
        //private System.Timers.Timer mailSendLogTimer;
        private System.Timers.Timer smsSendLogTimer;
        //private System.Timers.Timer smsReceiveLogTimer;

        private System.Timers.Timer weChatTimer;
        private System.Timers.Timer weChatLogTimer;
        private string LogName = "Arvato.CRM.CommunicateEngine";

   
        public CommunicateService()
        {

            InitializeComponent();

            //mailTimer = new System.Timers.Timer();
            //mailTimer.AutoReset = true;
            //mailTimer.Elapsed += mailTimer_Tick;

            smsSendTimer = new System.Timers.Timer();
            smsSendTimer.AutoReset = true;
            smsSendTimer.Elapsed += smsSendTimer_Tick;

            weChatTimer = new System.Timers.Timer();
            weChatTimer.AutoReset = true;
            weChatTimer.Elapsed += weChatTimer_Elapsed;


            smsSendLogTimer = new System.Timers.Timer();
            smsSendLogTimer.AutoReset = true;
            smsSendLogTimer.Elapsed += smsSendLogTimer_Tick;

            weChatLogTimer = new System.Timers.Timer();
            weChatLogTimer.AutoReset = true;
            weChatLogTimer.Elapsed += weChatLogTimer_Elapsed;


            Log.ServiceLog("Communication Init");
        }

  

        protected override void OnStart(string[] args)
        {
            var isCloseSms = Convert.ToBoolean(ConfigurationManager.AppSettings["IsCloseSms"].ToString());
            var IsCloseWechat = Convert.ToBoolean(ConfigurationManager.AppSettings["IsCloseWechat"].ToString());

            if (!isCloseSms)
            {
                smsSendTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SmsSendInterval"].ToString());
                smsSendTimer.Start();

                smsSendLogTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["SmsSendLogInterval"].ToString());
                smsSendLogTimer.Start();
                 
            }

            if (!IsCloseWechat)
            {
                weChatTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["WeChatSendInterval"].ToString());
                weChatTimer.Start();
                 
                weChatLogTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["WeChatSendLogInterval"].ToString());
                weChatLogTimer.Start();
                 
            }

            Log.ServiceLog(string.Format ("Communication Start -- sms Start {0} -- wechart Start {1} ",!isCloseSms,!IsCloseWechat));
        }

        protected override void OnStop()
        {
            //mailTimer.Stop();
            smsSendTimer.Stop();
            weChatTimer.Stop();
            //smsSendBatchTimer.Stop();
            //smsReceiveTimer.Stop();
            //mailSendLogTimer.Stop();
            smsSendLogTimer.Stop();
            weChatLogTimer.Stop();
            //smsReceiveLogTimer.Stop(); 


            Log.ServiceLog("Communication stop");
        }

        void weChatLogTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        { 
            try
            {
                WechatManager.Send();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        void weChatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            { 
                WechatManager.SendLog(); 
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void mailTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MailManager.Send();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void smsSendTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            { 
                SmsManager.Send(); 
               // SmsManager.SendByCheXiang();
                //SmsManager.Receive();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void smsSendBatchTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //SmsManager.Send();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void smsReceiveTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                SmsManager.SendBatch();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void mailSendLogTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MailManager.SendLog();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void smsSendLogTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                SmsManager.SendLog();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }

        private void smsReceiveLogTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                SmsManager.ReceiveLog();
            }
            catch (Exception ex)
            {
                Log.ServiceLog(LogName, ex.ToString());
            }
        }
    }
}
