﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Web;
using HtmlAgilityPack;
using System.Xml;
using Add_in_PlacSetAttributeTool;

namespace GetInfo_fromHtml
{
    static class  EnvironmentalData
    {
        /// <summary>
        /// 获取城市列表（空气质量逐小时历史数据）(缺少一部分城市)
        /// </summary>
        /// <returns></returns>
        public static List<string> getCity()
        {
            try
            {
                List<string> result = new List<string>();
                HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create("http://datacenter.mep.gov.cn/index!environmentAirHourDetailCity.action");
                httprequest.KeepAlive = true;
                httprequest.UserAgent = " Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                httprequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httprequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httprequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string sb = sr.ReadToEnd();
                sr.Close();
                var doc = new HtmlDocument();
                doc.LoadHtml(sb.ToString());
                var root = doc.DocumentNode;
                if (root != null)
                {
                    var citynodes = root.SelectSingleNode("/html[1]/body[1]");
                    for (int i = 1; i < 62; i++)
                    {
                        var cityRow = citynodes.ChildNodes[i];//省份所在的表格
                        if (cityRow.ChildNodes.Count > 0)
                        {
                            List<string> tempProviece = new List<string>();
                            for (int j = 0; j < cityRow.ChildNodes.Count; j++)
                            {
                                string temp = cityRow.ChildNodes[j].InnerText.Trim();
                                temp.Replace("\r\n", "");
                                temp.Replace("\n", "");
                                temp.Replace("\r", "");
                                temp.Replace("\t", "");
                                temp.Replace(" ", "");
                                if (!string.IsNullOrWhiteSpace(temp))
                                {
                                    tempProviece.Add(cityRow.ChildNodes[j].InnerText.Trim());
                                    if (temp.Contains("津"))
                                    {
                                    }
                                }
                            }
                            if (tempProviece.Count > 1)//去掉省份
                                tempProviece.RemoveAt(0);
                            result.AddRange(tempProviece);
                        }
                        else if (!string.IsNullOrWhiteSpace(cityRow.InnerText.Trim().Replace("\r\n","").Replace("\r","").Replace("\n","")))
                        {
                            result.Add(cityRow.InnerText.Trim());   
                        }                    
                    }
                }
                return result;
            }
            catch (Exception err)
            {
                LogHelp.WriteLog(err, "EnvironmentalData");
                return null;
            }
        }


        /// <summary>
        /// 传入待查询城市、查询时间返回一个字符串数组分别为：AQI 等级 PM2.5 PM10 SO2 NO2 CO 
        /// O3(不区分站点)
        /// </summary>
        /// <param name="name">城市名称</param>
        /// <param name="date">时间（一般为整点，否则返回为空）</param>
        /// <returns></returns>
        public static string[] getHoursData(string name, DateTime date)
        {
            try
            {
                string[] airDataHours = new string[8];   //AQI 等级 PM2.5 PM10 SO2 NO2 CO O3
                StringBuilder sb = new StringBuilder();               
                byte[] time = System.Text.Encoding.UTF8.GetBytes(string.Format("{0:g}", date));
                string formWeb = "cityName=" + System.Web.HttpUtility.UrlEncode(name) + "&searchTime=" + System.Web.HttpUtility.UrlEncode(time);
                string url = "http://datacenter.mep.gov.cn/index!environmentAirHourDetail.action";
                    //string.Format("http://datacenter.mep.gov.cn/index!environmentAirHourDetail.action?{0}", formWeb);
                HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(url);
                httprequest.KeepAlive = true;
                httprequest.Accept = " gzip, deflate";
                httprequest.Method = "POST";
                httprequest.ContentType = "application/x-www-form-urlencoded";
                StreamWriter sw = new StreamWriter(httprequest.GetRequestStream());
                sw.WriteLine(formWeb);
                sw.Close();
                try
                {
                    HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();
                    Stream s = httpresponse.GetResponseStream();
                    StreamReader sr = new StreamReader(s);
                    while (!sr.EndOfStream)
                    {
                        string tempstr = sr.ReadLine();
                        if (tempstr != " " & tempstr != "\r\n" & tempstr != "\r\n\r\n" & tempstr != "\r\n\r\n")
                        {
                            sb.AppendLine(tempstr);
                        }
                    }
                    sr.Close();
                }
                catch (Exception)
                {
                    return null;
                }
               
                var doc = new HtmlDocument();
                doc.LoadHtml(sb.ToString());
                HtmlNode docNode = doc.DocumentNode;
                var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]");//逐小时数据所在的部位
                if (res != null)
                {
                    var qai = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[1]/div[1]/div[1]");
                    foreach (var item in qai.ChildNodes)
                    {
                        airDataHours[0] = (item.OuterHtml.Trim());
                    }

                    var level = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[1]/div[1]/p[3]");
                    airDataHours[1] = level.InnerHtml.Trim();
                    var pm2d5 = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[4]/p[2]");
                    foreach (var item in pm2d5.ChildNodes)
                    {
                        airDataHours[2] = (item.OuterHtml.Trim());
                    }

                    var pm10 = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[5]/p[2]");
                    foreach (var item in pm10.ChildNodes)
                    {
                        airDataHours[3] = (item.OuterHtml.Trim());
                    }

                    var SO2 = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[6]/p[2]");
                    foreach (var item in SO2.ChildNodes)
                    {
                        airDataHours[4] = (item.OuterHtml.Trim());
                    }

                    var NO2 = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[7]/p[2]");
                    foreach (var item in NO2.ChildNodes)
                    {
                        airDataHours[5] = (item.OuterHtml.Trim());
                    }

                    var CO = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[8]/p[2]");
                    foreach (var item in CO.ChildNodes)
                    {
                        airDataHours[6] = (item.OuterHtml.Trim());
                    }

                    var O3 = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[2]/div[3]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[1]/div[1]/div[9]/p[2]");
                    foreach (var item in O3.ChildNodes)
                    {
                        airDataHours[7] = (item.OuterHtml.Trim());
                    }
                }
                return airDataHours;
            }
            catch (Exception err)
            {
                LogHelp.WriteLog(err, "EnvironmentalData");
                return null;               
            }
         
        }


        /// <summary>
        /// 逐天获取所有城市的综合评价和相应的指标(通过string获取返回页面，string的最大长度有限，
        /// 不能一次查询太多)
        /// </summary>
        /// <param name="PageNo">页数</param>
        /// <param name="city">可以为空，则返回所有城市的列表</param>
        /// <param name="fromDate">查询的起始时间</param>
        /// <param name="endDate">查询的结束时间</param>
        /// <returns>返回的列表中字符串数字共有8个元素，依次为：ID:{0}城市：{1}AQI指数为：{2}首要污染物:
        /// {3}等级：{4}日期：{5}CityCode:{6}空气质量级别{7}</returns>
        public static List<string[]> getDayData(int PageNo, string city, DateTime fromDate, DateTime endDate)
        {
            List<string[]> dayDataList = new List<string[]>();
            byte[] citybyte = System.Text.Encoding.UTF8.GetBytes(city);
            city = System.Web.HttpUtility.UrlEncode(citybyte);
            byte[] fromtimebyte = System.Text.Encoding.UTF8.GetBytes(fromDate.ToString("yyyy-MM-dd"));
            string vtime = System.Web.HttpUtility.UrlEncode(fromtimebyte);
            byte[] endtimebyte = System.Text.Encoding.UTF8.GetBytes(endDate.ToString("yyyy-MM-dd"));
            string etime = System.Web.HttpUtility.UrlEncode(endtimebyte);
            //string webform=string.Format("page.pageNo=1&page.orderBy=&page.order=&orderby=&ordertype=&xmlname=1462259560614&queryflag=close&isdesignpatterns=false&CITY=衡水市&V_DATE=2016-11-10&E_DATE=2017-11-23"
            string webform = string.Format("page.pageNo={0}&page.orderBy=&page.order=&orderby=&ordertype=&xmlname=1462259560614&queryflag=close&gisDataJson=&isdesignpatterns=false&CITY={1}&V_DATE={2}&E_DATE={3}", PageNo, city, vtime, etime);
            byte[] webformByte = System.Text.Encoding.UTF8.GetBytes(webform);
            HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create("http://datacenter.mep.gov.cn:8099/ths-report/report!list.action");
            
                httprequest.KeepAlive = false;
                httprequest.UserAgent = " Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                httprequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httprequest.ContentType = " application/x-www-form-urlencoded";
                httprequest.Method = "POST";
                StreamWriter sw = new StreamWriter(httprequest.GetRequestStream());
                sw.Write(webform);
                sw.Close();
                try
                {
                    HttpWebResponse response = (HttpWebResponse)httprequest.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader sr = new StreamReader(response.GetResponseStream(),Encoding.UTF8);
                        string sb = sr.ReadToEnd();
                        sr.Close();
                        response.Close();
                        response = null;
                        var doc = new HtmlDocument();
                        doc.LoadHtml(sb.ToString());
                        HtmlNode docNode = doc.DocumentNode;
                        if (docNode != null)
                        {
                            try
                            {
                                for (int i = 3; i < 63; i++)
                                {
                                    string[] cityDayData = new string[8];
                                    cityDayData[0] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[2].InnerText;
                                    cityDayData[1] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[4].InnerText;
                                    cityDayData[2] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[6].InnerText;
                                    cityDayData[3] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[8].InnerText;
                                    cityDayData[4] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[10].InnerText;
                                    cityDayData[5] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[12].InnerText;
                                    cityDayData[6] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[14].InnerText;
                                    cityDayData[7] = docNode.ChildNodes[3].ChildNodes[3].ChildNodes[25].ChildNodes[25].ChildNodes[1].ChildNodes[1].ChildNodes[i].ChildNodes[16].InnerText;

                                    dayDataList.Add(cityDayData);
                                    i++;
                                }
                            }
                            catch (Exception)
                            {
                                return dayDataList;
                            }
                        }
                    }
                    else
                        return null;  
                }
                catch (Exception)
                {

                    return null;
                }
            return dayDataList;
        }

        /// <summary>
        /// 获取所有城市的页面请求连接（http://www.pm25.in/），并写入文本
        /// </summary>
        /// <param name="savePath">保存路径</param>
        public static List<string> getCityHttp()
        {           
            try
            {
                List<string> urls = new List<string>();
                for (int kk = 0; kk < 3; kk++)
                {
                    List<string> tempurls = new List<string>();
                    try
                    {
                        HttpWebRequest pm25currentTime = (HttpWebRequest)WebRequest.Create("http://www.pm25.in/");
                        HttpWebResponse currenttimeResponse = (HttpWebResponse)pm25currentTime.GetResponse();
                        StreamReader sr = new StreamReader(currenttimeResponse.GetResponseStream());
                        string sb = sr.ReadToEnd();
                        sr.Close();
                        var doc = new HtmlDocument();
                        doc.LoadHtml(sb);
                        HtmlNode docnode = doc.DocumentNode;
                        for (int i = 1; i < 45; i++)
                        {
                            int count = docnode.ChildNodes[2].ChildNodes[3].ChildNodes[11].ChildNodes[5].ChildNodes[5].ChildNodes[3].ChildNodes[i].ChildNodes.Count;
                            var tempnode = docnode.ChildNodes[2].ChildNodes[3].ChildNodes[11].ChildNodes[5].ChildNodes[5].ChildNodes[3].ChildNodes[i].ChildNodes[3];
                            for (int j = 1; j < tempnode.ChildNodes.Count; j++)
                            {
                                string cityName = tempnode.ChildNodes[j].ChildNodes[1].InnerHtml;
                                string cityURL = string.Format("http://www.pm25.in{0}", tempnode.ChildNodes[j].ChildNodes[1].Attributes[0].Value);
                                tempurls.Add(string.Format("{0};{1}", cityName, cityURL));
                                j++;
                            }
                            i++;
                        }
                        if (urls == null)
                            urls = tempurls;
                        else
                            urls = tempurls.Count > urls.Count ? tempurls : urls;
                        tempurls = null;
                    }
                    catch (Exception) { }                   
                }
                return urls;
            }
            catch (Exception)
            {
                return null;                
            }
           
        }

        /// <summary>
        /// 实时的获取当前的空气质量数据
        /// </summary>
        /// <param name="cityUrl"></param>
        /// <returns>返回的列表依次对应该市的监测站点，每个字符串数组包括：数据更新时间 监测点 AQI 
        /// 空气质量指数类别 首要污染物 PM25 PM10 CO CO2 O3(1小时平均值) O3(8小时平均值) SO2  </returns>
        public static List<string[]> getRealtimeData(string cityUrl)
        {
            try
            {
                List<string[]> jiancezhan = new List<string[]>();
                HttpWebRequest pm25currentTime = (HttpWebRequest)WebRequest.Create(cityUrl);
                HttpWebResponse currenttimeResponse = (HttpWebResponse)pm25currentTime.GetResponse();
                StreamReader sr = new StreamReader(currenttimeResponse.GetResponseStream());
                string sb = sr.ReadToEnd();
                sr.Close();
                var doc = new HtmlDocument();
                doc.LoadHtml(sb);
                HtmlNode docnode = doc.DocumentNode;
                var dirtyFirst = docnode.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/div[4]/div[1]/p[1]");//该市首要污染物           
                var updateDataTime = docnode.SelectSingleNode("/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/p[1]");
                var tdinfo = docnode.SelectSingleNode("html[1]/body[1]/div[2]/div[3]/table[1]/tbody[1]");//获取数据所在的表
                for (int i = 1; i < tdinfo.ChildNodes.Count; i++)
                {
                    string[] hourdata = new string[12];
                    hourdata[0] = updateDataTime.InnerText.Trim().Split(new string[] { "间：" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    int index = 1;
                    for (int j = 1; j < tdinfo.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        //依次打出：监测点 AQI 空气质量指数类别 首要污染物 PM25 PM10 CO CO2 O3(1小时平均值) O3(8小时平均值) SO2                   
                        hourdata[index++] = tdinfo.ChildNodes[i].ChildNodes[j].InnerText.Trim();
                        j++;
                    }
                    jiancezhan.Add(hourdata);
                    i++;
                }
                return jiancezhan;
            }
            catch (Exception)
            {
                return null;              
            }
           
        }

    }
}
