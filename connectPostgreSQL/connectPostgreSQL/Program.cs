using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace connectPostgreSQL
{
    class Sample
    {
        private byte[] tifFile;

        public byte[] TifFile
        {
            get {
                
                return tifFile=tifFile; 
            }
            set { tifFile = value; }
        }
        private string standardVerion;

        public string StandardVerion
        {
            get { return standardVerion??""; }
            set { standardVerion = value; }
        }
        private string imageType;

        public string ImageType
        {
            get { return imageType??""; }
            set { imageType = value; }
        }
        private string imageSrc;

        public string ImageSrc
        {
            get { return imageSrc??""; }
            set { imageSrc = value; }
        }
        private DateTime imagePhase;

        public DateTime ImagePhase
        {
            get { return imagePhase; }
            set { imagePhase = value; }
        }
        private string author;

        public string Author
        {
            get { return author??""; }
            set { author = value; }
        }
        private string checker;

        public string Checker
        {
            get { return checker ?? ""; }
            set { checker = value; }
        }
        private DateTime produceTime;

        public DateTime ProduceTime
        {
            get { return produceTime; }
            set { produceTime = value; }
        }
        private string province;

        public string Province
        {
            get { return province??""; }
            set { province = value; }
        }
        private string city;

        public string City
        {
            get { return city??""; }
            set { city = value; }
        }
        private string district;

        public string District
        {
            get { return district ?? ""; }
            set { district = value; }
        }
        private int sampleWidtHeight;

        public int SampleWidtHeight
        {
            get { return sampleWidtHeight; }
            set { sampleWidtHeight = value; }
        }
        private double centerX;

        public double CenterX
        {
            get { return centerX; }
            set { centerX = value; }
        }
        private double centerY;

        public double CenterY
        {
            get { return centerY; }
            set { centerY = value; }
        }
        private string geometryData;

        public string GeometryData
        {
            get { return geometryData ?? ""; }
            set { geometryData = value; }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string connection = "Server=localhost;Port=5432;User Id=postgres;Password=1;Database=SampleDataBase;";
            NpgsqlConnection conn = new NpgsqlConnection(connection);
            conn.Open();
            if (conn.State==System.Data.ConnectionState.Open)
            {
                Console.WriteLine("open!!");
            }
            Console.ReadKey();
            //using (NpgsqlDataAdapter cmd = new NpgsqlDataAdapter("select id,name from test_table;", conn))
            //{
            //    DataSet ds = new DataSet();
            //    cmd.Fill(ds);
            //}
            Sample s=new Sample();
            string cmdstr = string.Format("insert into {0}(tiffile,standardversion,imagetype,imagesrc,imagephase,author,checker,producetime,province,city,district,samplewidthheight,centerx,centery) values({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14});", "t01e01p00100", s.TifFile, s.StandardVerion, s.ImageType, s.ImageSrc, s.ImagePhase, s.Author, s.Checker, s.ProduceTime, s.Province, s.City, s.District, s.SampleWidtHeight, s.CenterX, s.CenterY);
            //string cmdtxt = "insert into test_table(id,name) values (11,\'李磊\'),(12,\'韩梅梅\');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(cmdstr, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
