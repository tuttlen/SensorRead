using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using SpPerfChart;
using System.Threading;

namespace AccessCom3
{
    public partial class Form1 : Form
    {
        List<SensorData> sensorDataList = new List<SensorData>();
        SpPerfChart.PerfChart chart = new PerfChart();

        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
           
            //chart1.DataSource = sensorDataList;

            //chart1.DataBind();

            serialPort1.Open();
            timer1.Start();
            RunTimer();
           // serialPort1.Close();
            this.Refresh();
            

        }
        private void RunTimer()
        {
            backgroundWorker1.RunWorkerAsync(15);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //test theory
            /*
            [{ sensor :  262},{ output : 65}]
            [{ sensor :  254},{ output : 63}]
            [{ sensor :  240},{ output : 59}]
             * */
            var jsonString = "{\"sensor\":361,\"output\" :89}";// [{ \"sensor\" :  254},{ \"output\" : 63}],[{ \"sensor\" :  240},{ \"output\" : 59}]";

            var json2 ="[{\"Id\": 1,\"Name\": \"HP Up\"},{\"Id\": 2,\"Name\": \"Regeneration\"}]";

            var jsonPerson = "{\"age\":42,\"name\":\"John\"}";


            var test = jsonHelper.From<SensorData>(jsonString);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            
            
            //this.Refresh();

            perfChart1.TimerMode = TimerMode.Disabled;
            perfChart1.ScaleMode = ScaleMode.Relative;
            

            //chart1.ChartAreas.Add("SesorData");
            //chart1.ChartAreas.First().AxisX.Minimum = 0;
            //chart1.ChartAreas.First().AxisX.Maximum = 255;
            //chart1.ChartAreas.First().AxisX.Interval = 1;

            //chart1.ChartAreas.First().AxisY.Minimum = 0;
            //chart1.ChartAreas.First().AxisY.Maximum = 100;
            //chart1.ChartAreas.First().AxisY.Interval = 1;
            //chart1.Series.Add("output");
            //chart1.Series.Add("snesor");
            //chart1.Series["output"].Color = Color.Gray;
            //chart1.Series["snesor"].Color = Color.Red;

            //chart1.Series["output"].Points.AddXY(0, 0);
            //chart1.Series["snesor"].Points.AddXY(0, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int samplecounter = 0;
           
            {                
                jsonHelper jsonHelper = new jsonHelper();
                

              //  do
              //  {
                    string json = serialPort1.ReadLine();
                    try
                    {
                       // Console.WriteLine(json);
                        var sensorData = jsonHelper.From<SensorData>(json);
                        samplecounter++;
                        sensorDataList.Add(sensorData);

                    }
                    catch (Exception) 
                    {
                        Console.WriteLine(string.Format(@"Could not interpret |{0}|",json));
                    }

             //   } while (samplecounter <= Convert.ToInt16(textBox1.Text));
            }
           

            samplecounter = 0;
            //RunTimer();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(100);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //perfChart1.Clear();
            foreach (var sData in sensorDataList)
            {
                //chart1.Series["output"].Points.AddXY(samplecounter, sData.output);
                //chart1.Series["snesor"].Points.AddXY(samplecounter, sData.sensor);\
                //perfChart1.AddValue( sData.sensor);                
                //chart.AddValue( sData.output);
                System.Collections.BitArray bitarray = new System.Collections.BitArray(
                    new byte[] 
                        {
                            (byte)sData.sensor 
                        });



                perfChart1.AddValue(sData.sensor);    
                //foreach (bool bit in bitarray)
                //{
                //    var holdUp = bit ? 1 : 0;

                //    perfChart1.AddValue(holdUp);    
                //}
                
                

                //Console.WriteLine(sData.sensor);
                //samplecounter++;

            }
            
            sensorDataList.Clear();

            RunTimer();
        }



    }

    [DataContract]
    public class Person
    {
        public Person() { }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class SensorData
    {
        public SensorData() { }

        [DataMember]
        public int sensor { get; set; }

        [DataMember]
        public int output { get; set; }
    }

    [DataContract]
    class Person2
    {
        [DataMember]
        internal string name;

        [DataMember]
        internal int age;
    }


    public class jsonHelper
    {
        public static string To<T>(T obj)
        {
            string retVal = string.Empty;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                retVal = Encoding.Default.GetString(ms.ToArray());
            }
            return retVal;
        }

        public static T From<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
            }

            return obj;
        }
    }
}
