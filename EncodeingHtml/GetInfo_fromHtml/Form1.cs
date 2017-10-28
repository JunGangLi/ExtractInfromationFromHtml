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
                       // Action<DateTime, DateTime, string, string> getdata02 = new Action<DateTime, DateTime, string, string>(getDay);
                        string filePath = Path.GetDirectoryName(dataPath.Text.Trim());
                        if (!File.Exists(filePath + "\\cityList.txt"))
                        {
                            File.Delete(filePath + "\\cityList.txt");
                        }
                        //DateTime tempTime = DateTime.Now.AddDays(-7);
                        getCityList(DateTime.Now.AddDays(-3));
                       // getDay(tempTime, tempTime, filePath + "\\cityList.txt", "");
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
            EnvironmentalData.getCityHttp(cityList);
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
            //DateTime currenttime = DateTime.Now;
            //DateTime time = new DateTime(currenttime.Year, currenttime.Month, currenttime.Day, 23, 0, 0);
            //citylist.Text = string.Format("{0:g}", time);
            //time= time.AddHours(1);
            //citylist.Text += string.Format("{0:g}", time);

            List<string> cities = EnvironmentalData.getCity();

            //SaveFileDialog ofd = new SaveFileDialog();
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{

            //    pretime = DateTime.Now;
            //    citylist.Text = ofd.FileName;

            //}
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
            if (dateTimeStart.Enabled & dateTimeEnd.Enabled)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //ThreadStart  ts =new ThreadStart(
                    //Thread getdata1 = new System.Threading.Thread(new ParameterizedThreadStart(getHistoryAirData));
                    //getdata1.IsBackground = true;
                    dateTimeStart.Enabled = false;
                    dateTimeEnd.Enabled = false;
                    //var fromtime = dateTimeStart.Value;
                    //var endtime = dateTimeEnd.Value;
                    //getdata1.Start(ofd.FileName);
                    Action<string> getdata02 = new Action<string>(getHistoryAirData);
                    getdata02.BeginInvoke(ofd.FileName, recall, null);
                    //this.BeginInvoke(getdata, ofd.FileName);
                }      
            }
        }

        private void recall(IAsyncResult result)
        {
            //if (dateTimeEnd.InvokeRequired == true)
            //{
            //    Action set = new Action(() => { dateTimeEnd.Enabled = true; });
            //    this.BeginInvoke(set);
            //}
            //else
            //    dateTimeEnd.Enabled = true;

            //if (dateTimeStart.InvokeRequired == true)
            //{
            //    Action set = new Action(() => { dateTimeStart.Enabled = true; });
            //    this.BeginInvoke(set);
            //}
            //else
            //    dateTimeStart.Enabled = true;
            
        }

        private void getHistoryAirData(object opath)
        {
            DateTime start = dateTimeStart.Value;
            DateTime end = dateTimeEnd.Value;
            end = new DateTime(end.Year, end.Month, end.Day, 23, 0, 0);
            string path = opath.ToString();
            StreamWriter sw = new StreamWriter(path, true);
            List<string> cities = EnvironmentalData.getCity();
            DateTime currenttime = start;
            DateTime time = new DateTime(currenttime.Year, currenttime.Month, currenttime.Day, 0, 0, 0);
            int countNull = 0;
            while (countNull < 5 & time.CompareTo(end)<0 &time.CompareTo(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day))<0)
            {
                    for (int i = 0; i < cities.Count; i++)
                {
                   // List<string> dataAll = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(cities[i] + ";");
                    sb.Append(string.Format("{0:g}", time) + ";");
                   
                    string[] data = EnvironmentalData.getHoursData(cities[i], time);
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
            }
            //do
            //{               
            //    for (int i = 0; i < cities.Count; i++)
            //    {
            //       // List<string> dataAll = new List<string>();
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append(cities[i] + ";");
            //        sb.Append(string.Format("{0:g}", time) + ";");
                   
            //        string[] data = EnvironmentalData.getHoursData(cities[i], time);
            //        int dataNull = 0;
            //        for (int j = 0; j < data.Length; j++)
            //        {
            //            if (string.IsNullOrWhiteSpace(data[j]))
            //            {
            //                dataNull++;
            //            }
            //            else
            //            {
            //                sb.Append(data[j] + ";");
            //            }
            //        }
            //        if (dataNull == 8)
            //        {
            //            countNull++;
            //        }
            //        else
            //        {                       
            //            sw.WriteLine(sb.ToString());                       
            //        }
            //    }
            //    time=time.AddHours(1);               
            //} while (countNull < 5 & time.CompareTo(end)<0 &time.CompareTo(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day))<0);
            sw.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
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
                    Action<DateTime, DateTime, string, string> getdata02 = new Action<DateTime, DateTime, string, string>(getDay);
                    getdata02.BeginInvoke(dateTimeStart.Value, dateTimeEnd.Value, ofd.FileName, "", recall, null);                     
                }
            }
        }

        private List<string> getCityList(DateTime start)
        {
            List<string> cityList=new List<string>();
            List<string> tempcityList = new List<string>();            
            for (int j = 0; j < 3; j++)
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

        private void getDay(DateTime start, DateTime end, string pathfile, string city = "")
        {
            int pageNo = 1;
            using (StreamWriter sw = new StreamWriter(pathfile, true))
            {
                string filename = Path.GetFileNameWithoutExtension(pathfile);
                StreamWriter sw1 = new StreamWriter(Path.GetDirectoryName(pathfile) + "\\"+filename+"errHtml" + DateTime.Now.Millisecond.ToString() + ".txt", true);
                List<string[]> dayData = null;
                int uablecount = 0;
                do
                {                    
                    dayData = null;
                    dayData = EnvironmentalData.getDayData(pageNo, city, start, end);
                    if (dayData==null)
                    {
                        //页数；城市；开始时间；结束时间；
                        sw1.WriteLine(pageNo + ";" + city + ";" + start.ToString() + ";" + end.ToString());
                        uablecount++;
                        continue;
                    }                    
                    else if (dayData.Count==0)
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

    }
}
