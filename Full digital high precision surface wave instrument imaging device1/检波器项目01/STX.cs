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

namespace 检波器项目01
{
    public partial class STX : Form
    {
        public Button[] butt = new Button[24];
        public STX()
        {
            InitializeComponent();
        }

        string file;

        private void STX_Load(object sender, EventArgs e)
        {
            this.Size = new Size(550, 200);
            STB.Visible = false;
            for (int i = 0; i < 24; i++)
            {
                butt[i] = new Button();
                butt[i].Size = new Size(40, 23);
                butt[i].Text = (i+1).ToString();
                butt[i].Click += new EventHandler(buutclick);
                if (i < 12)
                {
                    butt[i].Location = new Point(12 + 43 * i, 40 + (45 * (i / 12)));
                }
                else
                {
                    butt[i].Location = new Point((12 + 43 * i) - 516, 40 + (45 * (i / 12)));
                }
                this.Controls.Add(butt[i]);
                butt[i].Visible = true;
                butt[i].Enabled = true;
            }
        }
        private void buutclick(object sender, EventArgs e)
        {

            Button lbl = (Button)(sender); //这么一转换就知道是哪个label点击了

            this.Size = new Size(960, 440);
            BT.Enabled = false;
            for(int i=0;i<24;i++)
            {
                butt[i].Visible = false;
                butt[i].Enabled = false;
            }
            STB.Visible = true;


            try
            {
                SetTitles("数据折线图", "时间", "电压(V)");
                //string[] B = Directory.GetFiles(@"D:\AAAAAA");


                Random ran = new Random();
                PointPairList list = new PointPairList();
                LineItem myCurve;
                //for (int i = 0; i <= 100; i++)
                //{
                //    double x = (double)new XDate(DateTime.Now.AddSeconds(-(100 - i)));
                //    double y = ran.NextDouble();
                //    list.Add(x, y);

                //}

                string PD = lbl.Text;
                if(PD.Length==1)
                {
                    PD = "0" + PD;
                }

                StreamReader sr = new StreamReader(@".\txt\"+ PD + ".txt", Encoding.Default);
                String line;
                double i = 0.0000001;
                int listLen = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    listLen++;
                    double X = double.Parse(i.ToString("0.0000000"));
                    double Y = double.Parse(line.ToString());
                    list.Add(X, Y, X.ToString() + "秒，" + Y.ToString() + "V，第" + listLen.ToString());
                    list.Add(double.Parse(i.ToString("0.0000000")), double.Parse(line.ToString()));
                    i = i + 0.0001024;
                    // Console.WriteLine(line.ToString());
                }
                DateTime dt = DateTime.Now;
                STB.GraphPane.CurveList.Clear();
                myCurve = STB.GraphPane.AddCurve("电压", list, Color.Red, SymbolType.Triangle);
                myCurve.Symbol.Size = 3;

                myCurve.Line.Width = 2;
                this.STB.AxisChange();
                this.STB.Refresh();

                //T1.Abort();

                //T1.DisableComObjectEagerCleanup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            STB.GraphPane.Title.Text = title;
            STB.GraphPane.XAxis.Title.Text = x_title;
            STB.GraphPane.XAxis.Title.Text = y_title;
        }
        /// <summary>
        /// 依据点集画线
        /// </summary>
        /// <param name="pointList">点集</param>
        public LineItem DrawLines(string label, PointPairList pointList, System.Drawing.Color color, SymbolType symbolType)
        {
            return STB.GraphPane.AddCurve(label, pointList, color, symbolType);
        }
    }
}
