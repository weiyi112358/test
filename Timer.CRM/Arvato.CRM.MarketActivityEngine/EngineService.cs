using Arvato.CRM.MarketActivityLogic;
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
using System.Threading.Tasks;
using System.Timers;

namespace Arvato.CRM.MarketActivityEngine
{
    public partial class EngineService : ServiceBase
    {
        private static int countTranslate_Tick;
        private static int counttPush_Tick;
        private static int counttPull_Tick;
        private static int counttNext_Tick;
        private static int counttFinish_Tick;
        private static int counttCoupon_Tick;
        private Timer tActive;
        private Timer tTranslate;
        private Timer tPush;
        private Timer tPull;
        private Timer tNext;
        private Timer tFinish;

        private Timer tCoupon;

        public EngineService()
        {
            //初始化计算次数
            countTranslate_Tick = 0;
            counttPush_Tick = 0;
            counttPull_Tick = 0;
            counttNext_Tick = 0;
            counttFinish_Tick = 0;

            counttCoupon_Tick = 0;

            InitializeComponent();

            tActive = new Timer();
            tActive.Elapsed += tActive_Tick;
            tActive.AutoReset = true;

            tTranslate = new Timer();
            tActive.Elapsed += tTranslate_Tick;
            tActive.AutoReset = true;

            tPush = new Timer();
            tPush.Elapsed += tPush_Tick;
            tPush.AutoReset = true;

            tPull = new Timer();
            tPull.Elapsed += tPull_Tick;
            tPull.AutoReset = true;

            tNext = new Timer();
            tNext.Elapsed += tNext_Tick;
            tNext.AutoReset = true;

            tFinish = new Timer();
            tFinish.Elapsed += tFinish_Tick;
            tFinish.AutoReset = true;


            tCoupon = new Timer();
            tCoupon.Elapsed += tCoupon_Elapsed;
            tCoupon.AutoReset = true;
        }

        protected override void OnStart(string[] args)
        {

            //初始化市场活动
            tActive.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["ActiveInternal"]);
            tActive.Start();

            //翻译市场活动流程
            tTranslate.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["TranslateInternal"]);
            tTranslate.Start();

            //启动活动流程
            tPush.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PushInternal"]);
            tPush.Start();

            //抓取活动流程结果
            tPull.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PullInternal"]);
            tPull.Start();

            tNext.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["NextInternal"]);
            tNext.Start();

            //完成市场活动
            tFinish.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["FinishInternal"]);
            tFinish.Start();

            //生成优惠券
            tCoupon.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["ConponInternal"]);
            tCoupon.Start();
        }

        protected override void OnStop()
        {
            tActive.Stop();
            tTranslate.Stop();
            tPush.Stop();
            tPull.Stop();
            tFinish.Stop();

            tCoupon.Stop();
        }

        /// <summary>
        /// 初始化市场活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tActive_Tick(object sender, EventArgs e)
        {
            try
            {
                ActivityManager.Active();
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Active", ex.ToString());
            }
        }

        /// <summary>
        /// 翻译市场活动步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tTranslate_Tick(object sender, EventArgs e)
        {

            try
            {
                if (countTranslate_Tick == 0 || countTranslate_Tick > 99)
                {
                    countTranslate_Tick += 1;
                    ActivityManager.Translate();
                    countTranslate_Tick = 0;
                }

                else
                {
                    countTranslate_Tick += 1;
                    Log.ServiceLog("MarkerActivityEngin-Translate Delay " + countTranslate_Tick.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }

            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Translate", ex.ToString());
            }

        }

        /// <summary>
        /// 启动活动流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tPush_Tick(object sender, EventArgs e)
        {
            try
            {
                if (counttPush_Tick == 0 || counttPush_Tick > 99)
                {
                    counttPush_Tick += 1;
                    ActivityManager.Push();
                    counttPush_Tick = 0;
                }
                else
                {
                    counttPush_Tick += 1;
                    Log.ServiceLog("MarkerActivityEngin-Push Delay " + counttPush_Tick.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Push", ex.InnerException.ToString());
            }
        }

        /// <summary>
        /// 抓取活动流程结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tPull_Tick(object sender, EventArgs e)
        {
            try
            {
                if (counttPull_Tick == 0 || counttPull_Tick > 99)
                {
                    counttPull_Tick += 1;
                    ActivityManager.Pull();
                    counttPull_Tick = 0;
                }
                else
                {
                    counttPull_Tick += 1;
                    Log.ServiceLog("MarkerActivityEngin-Pull Delay " + counttPull_Tick.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Pull", ex.ToString());
            }
        }

        /// <summary>
        /// 生成下一步记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tNext_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (counttNext_Tick == 0 || counttNext_Tick > 99)
                {
                    counttNext_Tick += 1;
                    ActivityManager.Next();
                    counttNext_Tick = 0;
                }
                else
                {
                    counttNext_Tick += 1;
                    Log.ServiceLog("MarkerActivityEngin-Next Delay " + counttNext_Tick.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Next", ex.ToString());
            }
        }

        /// <summary>
        /// 完成市场活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tFinish_Tick(object sender, EventArgs e)
        {
            try
            {
                if (counttFinish_Tick == 0 || counttFinish_Tick > 99)
                {
                    counttFinish_Tick += 1;
                    ActivityManager.Finish();
                    counttFinish_Tick = 0;
                }
                else
                {
                    counttFinish_Tick += 1;
                    Log.ServiceLog("MarkerActivityEngin-Finish Delay " + counttFinish_Tick.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-Finish", ex.ToString());
            }
        }


        /// <summary>
        /// 生成优惠券
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tCoupon_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (counttCoupon_Tick == 0 || counttCoupon_Tick > 3)
                {
                    counttCoupon_Tick += 1;
                    ActivityManager.CreateCoupon();
                    counttCoupon_Tick = 0;
                }
                else
                {
                    counttCoupon_Tick += 1;
                    Log.ServiceLog("MarkerActivityEngin-CreateCoupon Delay " + counttCoupon_Tick.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (Exception ex)
            {
                Log.ServiceLog("MarketActivityEngin-CreateCoupon", ex.ToString());
            }
        }
    }
}
