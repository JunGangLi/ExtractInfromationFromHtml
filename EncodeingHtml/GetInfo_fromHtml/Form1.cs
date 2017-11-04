using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace GetInfo_fromHtml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private System.Timers.Timer timer = new System.Timers.Timer();


        //private DateTime pretime = new DateTime();
        //private string cityListTXTPath = string.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
            int afterMinutes=0;
            if (!checkBox1.Checked)
            {
                if (!string.IsNullOrEmpty(dataPath.Text))
                {
                    if (!timer.Enabled)
                    {
                        List<string> MonitorSitUrl = EnvironmentalData.getCityHttp();
                        foreach (var item in MonitorSitUrl)
                        {
                            List<string[]> data= EnvironmentalData.getRealtimeData(item);
                        }

                       



                       // Action<DateTime, DateTime, string, string> getdata02 = new Action<DateTime, DateTime, string, string>(getDay);
                        
                        //getDay(tempTime, tempTime, filePath + "\\cityList.txt", "");
                       // getdata02.BeginInvoke(tempTime, tempTime, filePath + "\\cityList.txt", "", null, null); 

                        //Action<string, string> getRealtime = new Action<string, string>(autoGet);                           
                        //string path = Path.GetDirectoryName(dataPath.Text);
                        //getRealtime.BeginInvoke(path + "\\cityList.txt", dataPath.Text.Trim(), null, null);
                        ////this.BeginInvoke(getRealtime, path+"\\cityList.txt", dataPath.Text);
                        //timer.Enabled = true;
                        //timer.Interval = 3600000 ;
                        //timer.AutoReset = true;
                        //timer.Elapsed += new System.Timers.ElapsedEventHandler(getEveryHours);
                    }
                }
            }
            else
            {
                if (afterTime.Text != null & int.TryParse(afterTime.Text.Trim(), out afterMinutes))
                {
                    try
                    {
                        int time = Convert.ToInt32(afterTime);
                        timer.Interval = time*60000;
                    }
                    catch (Exception)
                    {
                        timer.Interval = 3600000;
                    }
                    timer.Enabled = true;                   
                    timer.AutoReset = true;
                   // timer.Elapsed += new System.Timers.ElapsedEventHandler(getEveryHours);

                }
            }

         
        }

       

        //private void getEveryHours(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(citylist.Text) & !string.IsNullOrEmpty(dataPath.Text))
        //    {
        //        Action<string, string> getRealtime = autoGet;

        //        this.BeginInvoke(getRealtime, citylist.Text, dataPath.Text);
        //    }
        //}

        private void autoGet(string cityList, string dataPath)
        {
            timer.Interval = 3600000;
            EnvironmentalData.getCityHttp();
            StreamReader sr = new StreamReader(cityList);
            StreamWriter sw = new StreamWriter(dataPath, true);
            while (!sr.EndOfStream)
            {
                string tempstr = sr.ReadLine();
                string[] cityhttp = tempstr.Split(new string[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries);
                List<string[]> cityDataList = EnvironmentalData.getRealtimeData(cityhttp[1]);
                foreach (var item in cityDataList)
                {
                    sw.Write(cityhttp[0] + ";");
                    for (int i = 0; i <item.Length; i++)
                    {
                        sw.Write(item[i] + ";");
                    }
                    sw.Write("\r\n");
                }
            }
            sr.Close();
            sw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
             if (ofd.ShowDialog() == DialogResult.OK)
             {
                 dataPath.Text = ofd.FileName;
             }
        }

      
        private void button3_Click(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(complete))
            {
                lock (complete)
                {                   
                    Action CitiesDisply = new Action(() =>
                    {
                        List<string> cities = getCityList(DateTime.Now.AddDays(-3));
                        if (cities.Count < 1)
                            MessageBox.Show("获取列表失败！");
                        updateCombox(cities, comboBox1);
                    });
                    CitiesDisply.BeginInvoke(null, null);
                }
            }
            else
                MessageBox.Show("正在获取城市列表");

           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                afterTime.Enabled = true;
            }
            else
            {
                afterTime.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
               // timer.Elapsed -= getEveryHours;
            }
            
        }

        private void btHistoryAirQ_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "文本文件|*.txt";
            if (dateTimeStart.Enabled & dateTimeEnd.Enabled)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (dateTimeEnd.Value.CompareTo(DateTime.Now) > 0)
                    {
                        dateTimeEnd.Value = DateTime.Now;
                    }

                    if (dateTimeStart.Value.CompareTo(dateTimeEnd.Value) > 0)
                    {
                        dateTimeStart.Value = dateTimeEnd.Value;
                    }                    
                    List<string> cities = new List<string>();
                    if (comboBox1.Items.Count<1)
                    {
                        MessageBox.Show("请先获取城市列表！");
                    }

                    if (comboBox1.SelectedIndex == -1)
                    {
                        if (MessageBox.Show("是否要获取所有城市的空气质量数据？", "注意选择城市列表", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            foreach (var item in comboBox1.Items)
                            {
                                cities.Add(item.ToString().Split(new string[]{";","；"},StringSplitOptions.RemoveEmptyEntries)[0]);
                            }
                        }
                        else
                            return;
                    }
                    else
                        cities.Add(comboBox1.SelectedItem.ToString().Split(new string[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries)[0]);

                    Action<string, DateTime, DateTime, List<string>> getdata02 = new Action<string, DateTime, DateTime, List<string>>(getHourData);
                    getdata02.BeginInvoke(ofd.FileName,dateTimeStart.Value,dateTimeEnd.Value,cities, null, null);     
                }      
            }
        }

       

        private void getHourData(object opath, DateTime start, DateTime end,List<string> cities)
        {           
            string path = opath.ToString();
            StreamWriter sw = new StreamWriter(path, true);
            int index = path.IndexOf(Path.GetExtension(path));
            string pathLog=path.Insert(index,DateTime.Now.Millisecond.ToString());
            StreamWriter sw1 = new StreamWriter(pathLog, true);
            DateTime currenttime = start;
            DateTime time = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(start.Year, start.Month, start.Day+1, 0, 0, 0);
            int countNull = 0;
            while (end.CompareTo(time) > 0)
            {
                for (int i = 0; i < cities.Count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(cities[i] + ";");
                    sb.Append(string.Format("{0:g}", time) + ";");                   
                    string[] data = EnvironmentalData.getHoursData(cities[i], time);
                    if (data == null)
                    {
                        sw1.WriteLine(cities[i]+";"+time.ToString());
                        continue;
                    }
                    int dataNull = 0;
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(data[j]))
                        {
                            dataNull++;
                        }
                        else
                        {
                            sb.Append(data[j] + ";");
                        }
                    }
                    if (dataNull == 8)
                    {
                        countNull++;
                    }
                    else
                    {                       
                        sw.WriteLine(sb.ToString());                       
                    }
                }
                time=time.AddHours(1);
                GC.Collect();
            }           
            sw1.Close();
            sw.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "文本文件|*.txt";
            if (dateTimeStart.Enabled & dateTimeEnd.Enabled)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (dateTimeEnd.Value.CompareTo(DateTime.Now) > 0)
                    {
                        dateTimeEnd.Value = DateTime.Now;
                    }

                    if (dateTimeStart.Value.CompareTo(dateTimeEnd.Value) > 0)
                    {
                        dateTimeStart.Value = dateTimeEnd.Value;
                    }

                    List<string> cities = new List<string>();
                    if (comboBox1.Items.Count < 1)
                    {
                        MessageBox.Show("请先获取城市列表！");
                    }

                    if (comboBox1.SelectedIndex == -1)
                    {
                        if (MessageBox.Show("是否要获取所有城市的空气质量数据？", "注意选择城市列表", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            foreach (var item in comboBox1.Items)
                            {
                                cities.Add(item.ToString().Split(new string[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                            }
                        }
                        else
                            return;
                    }
                    else
                        cities.Add(comboBox1.SelectedItem.ToString().Split(new string[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries)[0]);

                    Action<DateTime, DateTime, string,List<string>> getdata02 = new Action<DateTime, DateTime, string,List<string>>(getDayData);
                    getdata02.BeginInvoke(dateTimeStart.Value, dateTimeEnd.Value, ofd.FileName, cities, null, null);                     
                }
            }
        }

        private List<string> getCityList(DateTime start)
        {
            List<string> cityList=new List<string>();
            List<string> tempcityList = new List<string>();            
            for (int j = 0; j < 2; j++)
			{
                int pageNo = 1;
                tempcityList=new List<string>();                
                start=start.AddDays(-7*j);
                List<string[]> dayData = null;
                int uablecount = 0;
                do
                {
                    dayData = null;
                    dayData = EnvironmentalData.getDayData(pageNo, "", start, start);
                    if (dayData == null)
                    {
                        //页数；城市；开始时间；结束时间；                        
                        uablecount++;
                        continue;
                    }
                    else if (dayData.Count == 0)
                    {
                        uablecount++;
                        continue;
                    }
                    for (int i = 0; i < dayData.Count; i++)
                    {
                        tempcityList.Add(string.Format("{0};{1}", dayData[i][1], dayData[i][6]));                        
                    }
                    pageNo++;
                    GC.Collect();
                } while (uablecount <= 2);
                if (cityList.Count < 1)
                    cityList = tempcityList;
                else
                    cityList = (tempcityList.Count > cityList.Count ? tempcityList : cityList);
            }
            return cityList;
        }

        private void getDayData(DateTime start, DateTime end, string pathfile,List<string> city = null)
        {
            int pageNo = 1;
            using (StreamWriter sw = new StreamWriter(pathfile, true))
            {
                string filename = Path.GetFileNameWithoutExtension(pathfile);
                StreamWriter sw1 = new StreamWriter(Path.GetDirectoryName(pathfile) + "\\"+filename+"errHtml" + DateTime.Now.Millisecond.ToString() + ".txt", true);
                List<string[]> dayData = null; 
                if (city == null || city.Count < 1)
                {
                    int uablecount = 0;
                    do
                    {                        
                        dayData = EnvironmentalData.getDayData(pageNo, "", start, end);
                        if (dayData == null)
                        {
                            //页数；城市；开始时间；结束时间；
                            sw1.WriteLine(pageNo + ";" + city + ";" + start.ToString() + ";" + end.ToString());
                            uablecount++;
                            continue;
                        }
                        else if (dayData.Count == 0)
                        {
                            uablecount++;
                            continue;
                        }
                        for (int i = 0; i < dayData.Count; i++)
                        {
                            for (int j = 0; j < dayData[i].Length; j++)
                            {
                                sw.Write(dayData[i][j] + ";");
                            }
                            sw.WriteLine();
                        }
                        pageNo++;
                        GC.Collect();
                    } while (uablecount <= 5);
                }
                else
                {
                    foreach (var item in city)
                    {
                        int uablecount = 0;
                        do
                        {
                            dayData = EnvironmentalData.getDayData(pageNo, item, start, end);
                            if (dayData == null)
                            {
                                //页数；城市；开始时间；结束时间；
                                sw1.WriteLine(pageNo + ";" + city + ";" + start.ToString() + ";" + end.ToString());
                                uablecount++;
                                continue;
                            }
                            else if (dayData.Count == 0)
                            {
                                uablecount++;
                                continue;
                            }
                            for (int i = 0; i < dayData.Count; i++)
                            {
                                for (int j = 0; j < dayData[i].Length; j++)
                                {
                                    sw.Write(dayData[i][j] + ";");
                                }
                                sw.WriteLine();
                            }
                            pageNo++;
                            GC.Collect();
                        } while (uablecount <= 5);
                    }
                   
                }
                sw1.Close();          
                sw.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                string str = null;
                List<string> param = new List<string>();
                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    param.Add(str);
                }
                sr.Close();
                if (param.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "*.txt|*.txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Action<List<string>, string> getdata = new Action<List<string>, string>(getDayDataFromFile);
                        getdata.BeginInvoke(param, sfd.FileName,null,null);
                    }
                }              
            }
        }

        private void getDayDataFromFile(List<string> param,string pathfile)
        {
            //页数；城市；开始时间；结束时间；
            string filename = Path.GetFileNameWithoutExtension(pathfile);
            StreamWriter sw1 = new StreamWriter(Path.GetDirectoryName(pathfile) + "\\" + filename + "errHtmlBuchong" + DateTime.Now.Millisecond.ToString() + ".txt", true);
            StreamWriter sw = new StreamWriter(pathfile, true);
            foreach (var item in param)
            {
                string[] pCity = item.Split(new string[] { ";" }, StringSplitOptions.None);
                if (pCity.Length==4)
                {
                    DateTime start = Convert.ToDateTime(pCity[2]);
                    DateTime end = Convert.ToDateTime(pCity[3]);
                    int pageno=1;
                    if (start!=null & end!=null & int.TryParse(pCity[0],out pageno))
                    {
                        List<string[]> dayData = null;
                        dayData = EnvironmentalData.getDayData(pageno, pCity[1], start, end);
                        if (dayData == null)
                        {
                            //页数；城市；开始时间；结束时间；
                            sw1.WriteLine(pageno + ";" + pCity[1] + ";" + start.ToString() + ";" + end.ToString());
                            continue;
                        }
                           
                        for (int i = 0; i < dayData.Count; i++)
                        {
                            for (int j = 0; j < dayData[i].Length; j++)
                            {
                                sw.Write(dayData[i][j] + ";");
                            }
                            sw.WriteLine();
                        }                       
                    }                    
                }              
            }
            sw1.Close();
            sw.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "文本文件|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                string str = null;
                List<string> param = new List<string>();
                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    param.Add(str);
                }
                sr.Close();
                if (param.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "*.txt|*.txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Action<List<string>, string> getdata = new Action<List<string>, string>(getHourDataFromFile);
                        getdata.BeginInvoke(param, sfd.FileName, null, null);
                    }
                }
            }
        }

        private void getHourDataFromFile(List<string> param, string pathFile)
        {

        }

        private readonly object complete = new object();
        private void Form1_Load(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count<1 & Monitor.TryEnter(complete))
            {
                lock (complete)
                {
                    Action CitiesDisply = new Action(() =>
                    {
                        List<string> cities = getCityList(DateTime.Now.AddDays(-3));
                        if (cities.Count < 1)
                            MessageBox.Show("获取列表失败！");
                        updateCombox(cities, comboBox1);
                    });
                    CitiesDisply.BeginInvoke(null, null);
                }
            }
        }

        private void updateCombox(List<string> cities,ComboBox cbox)
        {
            if (cbox.InvokeRequired == true)
            {
                Action<List<string>, ComboBox> updateDelgete = new Action<List<string>, ComboBox>(updateCombox);
                this.BeginInvoke(updateDelgete,cities,cbox);
            }
            else
            {
                cbox.Items.Clear();
                foreach (var city in cities)
                {
                    cbox.Items.Add(city);
                }
            }
 
        }
    }
}
