using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Threading;

namespace 检波器项目01
{
    public partial class TX : Form
    {
        //Thread T1 ;
        public TX()
        {
            InitializeComponent();
            //createPane(TB);
            //T1 = new Thread(GOMIG);
        }
        private void GOMIG()
        {
            try
            {
                SetTitles("数据折线图", "时间", "电压");

                Random ran = new Random();
                PointPairList list = new PointPairList();
                LineItem myCurve;
                //for (int i = 0; i <= 100; i++)
                //{
                //    double x = (double)new XDate(DateTime.Now.AddSeconds(-(100 - i)));
                //    double y = ran.NextDouble();
                //    list.Add(x, y);

                //}
                
                StreamReader sr = new StreamReader(@".\a.txt", Encoding.Default);
                String line;
                double i = 0.0000000;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(double.Parse(i.ToString("0.0000000")), double.Parse(line.ToString()));
                    i=i+ 0.0001024;
                   // Console.WriteLine(line.ToString());
                   if(i%2000==0)
                    {
                        Console.WriteLine(i.ToString());
                    }
                }
                DateTime dt = DateTime.Now;
                myCurve = TB.GraphPane.AddCurve("电压", list, Color.DarkGreen, SymbolType.UserDefined);
                this.TB.AxisChange();
                this.TB.Refresh();

                //T1.Abort();

                //T1.DisableComObjectEagerCleanup();
            }
            catch (Exception)
            {
                //T1.Abort();
                //T1.DisableComObjectEagerCleanup();
            }
           
        }
        //public void createPane(ZedGraphControl zgc)
        //{
        //    GraphPane myPane = zgc.GraphPane;

        //    //设置图标标题和x、y轴标题
        //    TB
        //    TB.Title.Text = "机票波动情况";
        //    TB.XAxis.Title.Text = "波动日期";
        //    TB.YAxis.Title.Text = "机票价格";

        //    //更改标题的字体
        //    FontSpec myFont = new FontSpec("Arial", 20, Color.Red, false, false, false);
        //    myPane.Title.FontSpec = myFont;
        //    myPane.XAxis.Title.FontSpec = myFont;
        //    myPane.YAxis.Title.FontSpec = myFont;

        //    // 造一些数据，PointPairList里有数据对x，y的数组
        //    Random y = new Random();
        //    PointPairList list1 = new PointPairList();
        //    for (int i = 0; i < 36; i++)
        //    {
        //        double x = i;
        //        //double y1 = 1.5 + Math.Sin((double)i * 0.2);
        //        double y1 = y.NextDouble() * 1000;
        //        list1.Add(x, y1); //添加一组数据
        //    }

        //    // 用list1生产一条曲线，标注是“东航”
        //    LineItem myCurve = myPane.AddCurve("东航", list1, Color.Red, SymbolType.Star);

        //    //填充图表颜色
        //    myPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);

        //    //以上生成的图标X轴为数字，下面将转换为日期的文本
        //    string[] labels = new string[36];
        //    for (int i = 0; i < 36; i++)
        //    {
        //        labels[i] = System.DateTime.Now.AddDays(i).ToShortDateString();
        //    }
        //    myPane.XAxis.Scale.TextLabels = labels; //X轴文本取值
        //    myPane.XAxis.Type = AxisType.Text;   //X轴类型

        //    //画到zedGraphControl1控件中，此句必加
        //    zgc.AxisChange();

        //    //重绘控件
        //    Refresh();
        //}

        private void TX_Load(object sender, EventArgs e)
        {
            //ThreadPool.QueueUserWorkItem(s =>
            //{
            //    GOMIG();
            //});

            try
            {
                SetTitles("数据折线图", "时间", "电压(V)");

                Random ran = new Random();
                PointPairList list = new PointPairList();
                LineItem myCurve;
                
                //for (int i = 0; i <= 100; i++)
                //{
                //    double x = (double)new XDate(DateTime.Now.AddSeconds(-(100 - i)));
                //    double y = ran.NextDouble();
                //    list.Add(x, y);

                //}

                StreamReader sr = new StreamReader(@".\a.txt", Encoding.Default);
                String line;
                double i = 0.0000001;
                int listLen = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    listLen++;
                    double X = double.Parse(i.ToString("0.0000000"));
                    double Y = double.Parse(line.ToString());
                    list.Add(X, Y, X.ToString() + "秒，" + Y.ToString() + "V，第" + listLen.ToString());
                    i = i + 0.0001024;
                    
                }
                //DateTime dt = DateTime.Now;
                TB.GraphPane.CurveList.Clear();
                
                
                myCurve = TB.GraphPane.AddCurve("电压", list, Color.Red, SymbolType.Triangle);
                myCurve.Symbol.Size = 3;

                myCurve.Line.Width = 2;
                this.TB.AxisChange();
                this.TB.Refresh();

                //T1.Abort();

                //T1.DisableComObjectEagerCleanup();
            }
            catch (Exception)
            {
                //T1.Abort();
                //T1.DisableComObjectEagerCleanup();
            }



        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="title">图标题</param>
        /// <param name="x_title">X轴标题</param>
        /// <param name="y_title">Y轴标题</param>
        /// <param name="x_type">X轴类型</param>
        /// <param name="y_type">Y轴类型</param>
        public void SetTitles(string title, string x_title, string y_title)
        {
            TB.GraphPane.Title.Text = title;
            //TB.GraphPane.X2Axis.Title.Text = x_title;
            //TB.GraphPane.Y2Axis.Title.Text = y_title;
            TB.GraphPane.XAxis.Title.Text = x_title;
            TB.GraphPane.YAxis.Title.Text = y_title;
            //TB.GraphPane.X2Axis.ti
        }
        /// <summary>
        /// 依据点集画线
        /// </summary>
        /// <param name="pointList">点集</param>
        public LineItem DrawLines(string label, PointPairList pointList, System.Drawing.Color color, SymbolType symbolType)
        {
            return TB.GraphPane.AddCurve(label, pointList, color, symbolType);
        }

    }
}
