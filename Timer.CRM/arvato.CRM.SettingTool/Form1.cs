using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arvato.CRM.SettingTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string errorSql = ""; 

        private void onLoadFormat()
        {
            //KPI类型下拉框
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DisplayMember = "OptionText";
            comboBox1.ValueMember = "OptionValue";

            //计算时间
            //dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            //dateTimePicker2.Format = DateTimePickerFormat.Custom;
            //dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            //dateTimePicker1.ShowUpDown = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化配置
            onLoadFormat();

            //初始化数据
            using (var db = new DBEntities())
            {
                //KPI类型
                comboBox1.DataSource = db.TD_SYS_BizOption.Where(o => o.OptionType == "KPIType").ToList();
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            richTextBox1.Text = "";
            string kpitype = comboBox1.SelectedValue.ToString();
            string kpitypename = comboBox1.Text;
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    int count = (dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days;

                    var query = from kt in db.TM_CRM_KPITarget
                                join k in db.TM_CRM_KPI on new { kt.KPIID, kt.KPIType } equals new { k.KPIID, k.KPIType }
                                where kt.KPIType == kpitype
                                select new
                                {
                                    kt.KPIID,
                                    kt.KPITypeValue,
                                    kt.KPIType,
                                    k.KPIName,
                                    k.ComputeScript
                                };
                    if (textBox1.Text.Trim() != "") query = query.Where(o => o.KPITypeValue == textBox1.Text.Trim());

                    if (textBox2.Text.Trim() != "") { int id = int.Parse(textBox2.Text); query = query.Where(kpi => kpi.KPIID == id); }
                    var list = query.ToList();
                    progressBar1.Maximum = list.Count * (count + 1);
 
                    string delStr = "Delete TM_CRM_KPIResult where ComputeTime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and ComputeTime <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59' and KPIType = '" + kpitype + "' ";
                    if (textBox1.Text.Trim() != "") delStr += " AND KPITypeValue = '" + textBox1.Text.Trim() + "' ";
                    if (textBox2.Text.Trim() != "") delStr += " AND KPIID = " + textBox2.Text.Trim();

                    db.Database.ExecuteSqlCommand(delStr);

                    foreach (var l in list)
                    {                      
                        for (int i = 0; i <= count; i++)
                        {
                            var curDatetime = dateTimePicker1.Value.AddDays(i);
                            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";

                            //var kd = db.TM_CRM_KPI.Single(o => o.KPIID == l.KPIID && o.KPIType == kpitype);
                            if (!string.IsNullOrEmpty(l.ComputeScript))
                            {
                                var cmd = db.Database.Connection.CreateCommand();
                                cmd.CommandTimeout = 240;
                                cmd.CommandText = l.ComputeScript.Replace("[KPITYPE]", "'" + kpitype + "'").Replace("[KPIID]", "'" + l.KPIID + "'").Replace("[KPITypeVALUE]", "'" + l.KPITypeValue + "'").Replace("[DateNow]", "'" + curDatetime.ToString("yyyy-MM-dd") + "'").Replace("[DatetimeNow]", curTime).Replace("[YearNow]", "'" + curDatetime.ToString("yyyy") + "'").Replace("[MonthNow]", "'" + curDatetime.ToString("MM") + "'").Replace("[DayNow]", curDatetime.ToString("dd"));//替换脚本中的一些通配符

                                errorSql = cmd.CommandText;

                                cmd.CommandType = System.Data.CommandType.Text;
                                if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                                cmd.ExecuteNonQuery();

                                string msg = string.Format("{3}{0}-{1}-{4}在{2}的脚本运行成功{5}", l.KPIID, l.KPIName, curDatetime.ToString("yyyy-MM-dd"), kpitypename, l.KPITypeValue, cmd.CommandText.ToString());
                                progressBar1.Value += 1;
                                
                                
                                //richTextBox1.Text += msg + "\n";
                                LogHelper.WriteInfoLog(msg);
                            }
                        }
                    }
                }
                MessageBox.Show("执行成功");
            }
            catch (Exception ex)
            {
                
                richTextBox1.Text += ex.Message;
                LogHelper.WriteErrorLog(ex.Message + "\n" +errorSql);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            richTextBox1.Text = "";
            string kpitype = comboBox1.SelectedValue.ToString();
            string kpitypename = comboBox1.Text;
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    int count = (dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days;

                    string delStr = "Delete TM_CRM_KPIResult where ComputeTime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and ComputeTime <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59' and KPIType = '" + kpitype + "' ";
                    if (textBox1.Text.Trim() != "") delStr += " AND KPITypeValue = '" + textBox1.Text.Trim() + "' ";
                    if (textBox2.Text.Trim() != "") delStr += " AND KPIID = " + textBox2.Text.Trim();

                    db.Database.ExecuteSqlCommand(delStr);
                    progressBar1.Value = 100;

                }
                MessageBox.Show("执行成功");
            }
            catch (Exception ex)
            {

                richTextBox1.Text += ex.Message;
                LogHelper.WriteErrorLog(ex.Message + "\n" + errorSql);
            }
        }

    }
}
