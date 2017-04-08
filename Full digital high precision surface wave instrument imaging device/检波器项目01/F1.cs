using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace 检波器项目01
{
    public partial class F1 : Form
    {
        
        SerialPort SP = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
        //Thread T1;
        //Thread T2;
        //TX TX = new TX();
        //STX STX = new STX();
        CJ CJ = new CJ();
        

        public F1()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            // T1 = new Thread(GO);
            // T2 = new Thread(SETUP);
        }

        public void SETUP()
        {
            try
            {
                SP.Open();
                List<byte> gobyte = new List<byte>();
                gobyte.AddRange(new byte[] { 0xFC, 0xFD });
                gobyte.AddRange(new byte[] { 0x00, 0x31 });
                gobyte.AddRange(new byte[] { 0x12 });
                gobyte.AddRange(new byte[] { 0x00, 0x00 });
                for (int i = 0; i < 6; i++)
                {
                    gobyte.AddRange(new byte[] { 0x00 });
                }

                gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                gobyte.AddRange(new byte[] { 0xFE, 0xFF });

                SP.Write(gobyte.ToArray(), 0, gobyte.ToArray().Length);
                listBox1.Items.Add("发送启动采集:" + STB(gobyte.ToArray(), 16));
                List<byte> inbyte = new List<byte>();
                byte[] LSbyte = new byte[1];
                for (int i = 0; i < 16; i++)
                {
                    SP.Read(LSbyte, 0, 1);
                    inbyte.AddRange(LSbyte);
                }
                byte[] Rbyte = inbyte.ToArray();

                listBox1.Items.Add("启动采集返回值:" + STB(Rbyte, 16));
                SP.Close();
                //T2.DisableComObjectEagerCleanup();
            }
            catch (Exception EX)
            {
                //T2.DisableComObjectEagerCleanup();
                SP.Close();
                MessageBox.Show(EX.ToString());
            }
        }
        /// <summary>
        /// byte数组转显示字符串
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public string STB(byte[] A, int len)
        {
            string AA = "";
            for (int i = 0; i < len; i++)
            {
                string BB = A[i].ToString("X");
                if (BB.Length == 1)
                {
                    BB = "0" + BB;
                }
                AA = AA + " " + BB;
            }

            return AA;
        }
        /// <summary>
        /// 从新打开串口
        /// </summary>
        private void SPR()
        {
            SP.Close();
            Thread.Sleep(50);
            SP.Open();
        }

        /// <summary>
        /// 查询检波器状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen == false)
            {
                MessageBox.Show("串口未连接", "错误");
                return;
            }
            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {
                    //SP.Open();
                    List<byte> gobyte = new List<byte>();
                    gobyte.AddRange(new byte[] { 0xFC, 0xFD });
                    gobyte.AddRange(new byte[] { 0x00, 0x11 });
                    gobyte.AddRange(new byte[] { 0x12 });
                    for (int i = 0; i < 8; i++)
                    {
                        gobyte.AddRange(new byte[] { 0x01 });
                    }
                    gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                    gobyte.AddRange(new byte[] { 0xFE, 0xFF });

                    SP.Write(gobyte.ToArray(), 0, gobyte.ToArray().Length);
                    listBox1.Items.Add("发送查询状态:" + STB(gobyte.ToArray(), 16));
                    List<byte> inbyte = new List<byte>();
                    byte[] Rbyte = new byte[16];
                    byte[] LSbyte = new byte[1];
                    for (int i = 0; i < 16; i++)
                    {
                        SP.Read(LSbyte, 0, 1);
                        inbyte.AddRange(LSbyte);
                    }
                    byte[] OUTA = inbyte.ToArray();
                    listBox1.Items.Add("查询检波器状态:" + STB(OUTA, 16));
                    SPR();                    //SP.Close();
                }
                catch (Exception EX)
                {
                    SPR();
                    MessageBox.Show(EX.ToString());
                }
            });



        }

        /// <summary>
        /// 累加和校验【每字节相加（16进）取后末两位】
        /// </summary>
        /// <param name="bytes">需要计算的byte数组</param>
        /// <returns></returns>
        public byte GetVerifyCodeByte(byte[] bytes)
        {

            int result = 0;
            foreach (byte b in bytes)
            {
                result += b;
            }
            return (byte)(result % 256);

        }


        /// <summary>
        /// 读取上传数据的子线程
        /// </summary>
        //private void GO()
        //{
        //    try
        //    {
        //        string path = @".\a.txt";//文件存放路径，保证文件存在。
        //        StreamWriter sw = new StreamWriter(path, true);
        //        SP.Open();
        //        listBox2.Items.Add("开始采集");
        //        for (int i=0;i<int.Parse(up1.Text);i++)
        //        {
        //            if (i == 299)
        //            {
        //                ;
        //            }
        //            byte[] AA = BitConverter.GetBytes(i+1);
        //            List<byte> gobyte = new List<byte>();
        //            gobyte.AddRange(new byte[] { 0xFC, 0xFD });
        //            gobyte.AddRange(new byte[] { 0x00, 0x32 });
        //            gobyte.AddRange(new byte[] { 0x12 });
        //            gobyte.AddRange(new byte[] { AA[1], AA[0] });
        //            for (int z = 0; z < 6; z++)
        //            {
        //                gobyte.AddRange(new byte[] { 0x00 });
        //            }
        //            //gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
        //            gobyte.AddRange(new byte[] { 0x01 });
        //            gobyte.AddRange(new byte[] { 0xFE, 0xFF });

        //            SP.Write(gobyte.ToArray(), 0, gobyte.ToArray().Length);
        //            List<byte> inbyte = new List<byte>();
        //            byte[] Rbyte = new byte[1015];

        //            byte[] LSbyte = new byte[1];
        //            for (int x = 0; x < 1015; x++)
        //            {
        //                SP.Read(LSbyte, 0, 1);
        //                inbyte.AddRange(LSbyte);
        //            }
        //            byte[] OUTA = inbyte.ToArray();

        //            string KOSTR = STB(OUTA, 1015);
        //            string[] BOKSTR = KOSTR.Split(' ');

        //            //listBox2.Items.Add("第"+(i+1).ToString()+"帧:" + KOSTR);

        //            for (int z = 13; z < 1009; z = z + 4)
        //            {
        //                string byte3tostr = BOKSTR[z] + BOKSTR[z + 1] + BOKSTR[z + 2];
        //                int temp_num = Convert.ToInt32(byte3tostr, 16);
        //                if (Convert.ToInt32(BOKSTR[z], 16)>=0x80)
        //                {
        //                    temp_num = temp_num - 0xFFFFFF;
        //                }
        //                double jieguo = (Convert.ToDouble(((temp_num / Math.Pow(2, 24)) * 4.809).ToString("0.000")));
        //               sw.WriteLine(jieguo.ToString()); 
        //            }
        //            upmess.Text = "第" + (i + 1).ToString() + "帧接收完成";
        //        }
        //        listBox2.Items.Add("接收完成");
        //        sw.Close();
        //        SP.Close();
        //        //T1.DisableComObjectEagerCleanup();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        //T1.DisableComObjectEagerCleanup();
        //    }
        //}
        /// <summary>
        /// 设置检波器参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen == false)
            {
                MessageBox.Show("串口未连接", "错误");
                return;
            }
            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {

                    List<byte> gobyte = new List<byte>();
                    gobyte.AddRange(new byte[] { 0xFC, 0xFD });
                    gobyte.AddRange(new byte[] { 0x00, 0x21 });
                    gobyte.AddRange(new byte[] { 0x12 });
                    gobyte.AddRange(new byte[] { 0x00, BitConverter.GetBytes(int.Parse(set1.Text))[0] });
                    gobyte.AddRange(new byte[] { BitConverter.GetBytes(int.Parse(set3.Text))[1], BitConverter.GetBytes(int.Parse(set3.Text))[0] });
                    gobyte.AddRange(new byte[] { 0x00, BitConverter.GetBytes(int.Parse(set2.Text))[0] });
                    gobyte.AddRange(new byte[] { 0x00, BitConverter.GetBytes(int.Parse(set4.Text))[0] });


                    gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                    gobyte.AddRange(new byte[] { 0xFE, 0xFF });

                    SP.Write(gobyte.ToArray(), 0, gobyte.ToArray().Length);
                    listBox1.Items.Add("发送设置参数:" + STB(gobyte.ToArray(), 16));
                    List<byte> inbyte = new List<byte>();
                    byte[] LSbyte = new byte[1];
                    for (int i = 0; i < 16; i++)
                    {
                        SP.Read(LSbyte, 0, 1);
                        inbyte.AddRange(LSbyte);
                    }
                    byte[] Rbyte = inbyte.ToArray();
                    listBox1.Items.Add("设置参数返回值:" + STB(Rbyte, 16));
                    SPR();
                }
                catch (Exception EX)
                {
                    SPR();
                    MessageBox.Show(EX.ToString());
                }
            });

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(@".\config.ini", Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                string A = line.ToString();
                if (A.Length != (A.Replace("#", "")).Length)
                {
                    string[] B = A.Split('#');
                    IP.Text = B[0];
                    Parse.Text = B[1];
                }
            }
            sr.Close();
        }
        /// <summary>
        /// 启动采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen == false)
            {
                MessageBox.Show("串口未连接", "错误");
                return;
            }
            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {
                    //SP.Open();
                    List<byte> gobyte = new List<byte>();
                    gobyte.AddRange(new byte[] { 0xFC, 0xFD });
                    gobyte.AddRange(new byte[] { 0x00, 0x31 });
                    gobyte.AddRange(new byte[] { 0x12 });
                    gobyte.AddRange(new byte[] { 0x00, 0x00 });
                    for (int i = 0; i < 6; i++)
                    {
                        gobyte.AddRange(new byte[] { 0x00 });
                    }

                    gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                    gobyte.AddRange(new byte[] { 0xFE, 0xFF });

                    SP.Write(gobyte.ToArray(), 0, gobyte.ToArray().Length);
                    listBox1.Items.Add("发送启动采集:" + STB(gobyte.ToArray(), 16));
                    List<byte> inbyte = new List<byte>();
                    byte[] LSbyte = new byte[1];
                    for (int i = 0; i < 16; i++)
                    {
                        SP.Read(LSbyte, 0, 1);
                        inbyte.AddRange(LSbyte);
                    }
                    byte[] Rbyte = inbyte.ToArray();

                    listBox1.Items.Add("启动采集返回值:" + STB(Rbyte, 16));
                    SPR();
                }
                catch (Exception EX)
                {
                    SPR();
                    MessageBox.Show(EX.ToString());
                }
            });



            //  T2.Start();
        }
        /// <summary>
        /// 启动读取上传数据的子线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (SP.IsOpen == false)
            {
                MessageBox.Show("串口未连接", "错误");
                return;
            }
            ThreadPool.QueueUserWorkItem(s =>
            {
                string path = @".\a.txt";//文件存放路径，保证文件存在。
                StreamWriter sw = new StreamWriter(path, true);
                try
                {

                    File.Delete(path);


                    //SP.Open();
                    listBox2.Items.Add("开始采集");
                    for (int i = 0; i < int.Parse(set3.Text); i++)
                    {
                        byte[] AA = BitConverter.GetBytes(i + 1);
                        List<byte> gobyte = new List<byte>();
                        gobyte.AddRange(new byte[] { 0xFC, 0xFD });
                        gobyte.AddRange(new byte[] { 0x00, 0x32 });
                        gobyte.AddRange(new byte[] { 0x12 });
                        gobyte.AddRange(new byte[] { AA[1], AA[0] });
                        for (int z = 0; z < 6; z++)
                        {
                            gobyte.AddRange(new byte[] { 0x00 });
                        }
                        //gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                        gobyte.AddRange(new byte[] { 0x01 });
                        gobyte.AddRange(new byte[] { 0xFE, 0xFF });

                        SP.Write(gobyte.ToArray(), 0, gobyte.ToArray().Length);
                        List<byte> inbyte = new List<byte>();
                        byte[] Rbyte = new byte[1015];

                        byte[] LSbyte = new byte[1];
                        for (int x = 0; x < 1015; x++)
                        {
                            SP.Read(LSbyte, 0, 1);
                            inbyte.AddRange(LSbyte);
                        }
                        byte[] OUTA = inbyte.ToArray();

                        string KOSTR = STB(OUTA, 1015);
                        string[] BOKSTR = KOSTR.Split(' ');

                        //listBox2.Items.Add("第"+(i+1).ToString()+"帧:" + KOSTR);

                        for (int z = 13; z < 1009; z = z + 4)
                        {
                            string byte3tostr = BOKSTR[z] + BOKSTR[z + 1] + BOKSTR[z + 2];
                            int temp_num = Convert.ToInt32(byte3tostr, 16);
                            if (Convert.ToInt32(BOKSTR[z], 16) >= 0x80)
                            {
                                temp_num = temp_num - 0xFFFFFF;
                            }
                            double jieguo = (Convert.ToDouble(((temp_num / Math.Pow(2, 24)) * 4.809).ToString("0.000")));
                            sw.WriteLine(jieguo.ToString());
                        }
                        upmess.Text = "第" + (i + 1).ToString() + "帧接收完成";
                    }
                    listBox2.Items.Add("接收完成");
                    sw.Close();
                    SPR();
                }
                catch (Exception ex)
                {
                    SPR();
                    sw.Close();
                    MessageBox.Show(ex.ToString());

                }
            });

        }

        private void button6_Click(object sender, EventArgs e)
        {
            TX TX = new TX();
            TX.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
        
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (button7.Text == "打开串口")
                {
                    if (SP.IsOpen == true)
                    {
                        button7.Text = "关闭串口";
                    }
                    else
                    {
                        SP.Open();
                        button7.Text = "关闭串口";
                    }
                }
                else if (button7.Text == "关闭串口")
                {
                    if (SP.IsOpen == false)
                    {
                        button7.Text = "打开串口";
                    }
                    else
                    {
                        SP.Close();
                        button7.Text = "打开串口";
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("串口错误");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }





        //串口部分
        //*************************************************************************************************************************
        //*************************************************************************************************************************
        //*************************************************************************************************************************
        public void setimg()
        {
            P1.Image = Properties.Resources.空白;
            P2.Image = Properties.Resources.空白;
            P3.Image = Properties.Resources.空白;
            P4.Image = Properties.Resources.空白;
            P5.Image = Properties.Resources.空白;
            P6.Image = Properties.Resources.空白;
            P7.Image = Properties.Resources.空白;
            P8.Image = Properties.Resources.空白;
            P9.Image = Properties.Resources.空白;
            P10.Image = Properties.Resources.空白;
            P11.Image = Properties.Resources.空白;
            P12.Image = Properties.Resources.空白;
            P13.Image = Properties.Resources.空白;
            P14.Image = Properties.Resources.空白;
            P15.Image = Properties.Resources.空白;
            P16.Image = Properties.Resources.空白;
            P17.Image = Properties.Resources.空白;
            P18.Image = Properties.Resources.空白;
            P19.Image = Properties.Resources.空白;
            P20.Image = Properties.Resources.空白;
            P21.Image = Properties.Resources.空白;
            P22.Image = Properties.Resources.空白;
            P23.Image = Properties.Resources.空白;
            P24.Image = Properties.Resources.空白;
        }

        public void setimgONORNOT(byte[] A)
        {
            if (A[0] == 0x01)
            {
                P1.Image = Properties.Resources.绿;
            }
            if (A[1] == 0x01)
            {
                P2.Image = Properties.Resources.绿;
            }
            if (A[2] == 0x01)
            {
                P3.Image = Properties.Resources.绿;
            }

            if (A[3] == 0x01)
            {
                P4.Image = Properties.Resources.绿;
            }

            if (A[4] == 0x01)
            {
                P5.Image = Properties.Resources.绿;
            }

            if (A[5] == 0x01)
            {
                P6.Image = Properties.Resources.绿;
            }

            if (A[6] == 0x01)
            {
                P7.Image = Properties.Resources.绿;
            }

            if (A[7] == 0x01)
            {
                P8.Image = Properties.Resources.绿;
            }

            if (A[8] == 0x01)
            {
                P9.Image = Properties.Resources.绿;
            }

            if (A[9] == 0x01)
            {
                P10.Image = Properties.Resources.绿;
            }

            if (A[10] == 0x01)
            {
                P11.Image = Properties.Resources.绿;
            }

            if (A[11] == 0x01)
            {
                P12.Image = Properties.Resources.绿;
            }

            if (A[12] == 0x01)
            {
                P13.Image = Properties.Resources.绿;
            }

            if (A[13] == 0x01)
            {
                P14.Image = Properties.Resources.绿;
            }

            if (A[14] == 0x01)
            {
                P15.Image = Properties.Resources.绿;
            }

            if (A[15] == 0x01)
            {
                P16.Image = Properties.Resources.绿;
            }

            if (A[16] == 0x01)
            {
                P17.Image = Properties.Resources.绿;
            }

            if (A[17] == 0x01)
            {
                P18.Image = Properties.Resources.绿;
            }

            if (A[18] == 0x01)
            {
                P19.Image = Properties.Resources.绿;
            }

            if (A[19] == 0x01)
            {
                P20.Image = Properties.Resources.绿;
            }

            if (A[20] == 0x01)
            {
                P21.Image = Properties.Resources.绿;
            }

            if (A[21] == 0x01)
            {
                P22.Image = Properties.Resources.绿;
            }

            if (A[22] == 0x01)
            {
                P23.Image = Properties.Resources.绿;
            }

            if (A[23] == 0x01)
            {
                P24.Image = Properties.Resources.绿;
            }

        }
        //*************************************************************************************************************************
        //*************************************************************************************************************************
        //*************************************************************************************************************************
        //网络部分


        IPEndPoint IPE;
        Socket SK;
        public static byte[] IS = new byte[24];
        public static byte[] ISSET = new byte[24];

        void jiesoushuju()
        {
            try
            {
                if (SK == null)
                {
                    SK = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                if (clientSckIsConnected(SK))
                {

                }
                else
                {
                    SK.Connect(IPAddress.Parse(IP.Text), int.Parse(Parse.Text));
                    SETMESS("服务端连接成功");
                    TTCP.Enabled = true;
                    //ReceiveTh.Start();
                    Thread ReceiveTh = new Thread(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                byte[] ReceiveBts = new byte[1100];
                                int len = SK.Receive(ReceiveBts);
                                if (len == 0)
                                {
                                    throw new Exception(SK.RemoteEndPoint.ToString() + ":断开连接！ ");
                                    button8.Enabled = true;
                                }
                                else
                                {
                                    button8.Enabled = true;
                                    if (len < 100)
                                    {
                                        SETMESS("收到数据:" + STB(ReceiveBts, len));
                                    }

                                    if (ReceiveBts[0] == 0xFC && ReceiveBts[1] == 0xFD)
                                    {
                                        //switch (ReceiveBts[4])
                                        {
                                            //case 0x11:
                                            if (ReceiveBts[4] == 0x11)
                                            {
                                                byte[] K = new byte[24];
                                                for (int i = 0; i < 24; i++)
                                                {
                                                    K[i] = ReceiveBts[46 + i];
                                                    IS[i] = ReceiveBts[46 + i];
                                                    ISSET[i] = ReceiveBts[46 + i];
                                                }
                                                setimgONORNOT(K);
                                            }
                                            //break;
                                            // case 0x21:
                                            else if (ReceiveBts[4] == 0x21)
                                            {
                                                byte[] K = new byte[24];
                                                for (int i = 0; i < 24; i++)
                                                {
                                                    K[i] = ReceiveBts[46 + i];
                                                    IS[i] = ReceiveBts[46 + i];
                                                    ISSET[i] = ReceiveBts[46 + i];
                                                }
                                                setimgONORNOT(K);
                                            }
                                            // break;
                                            // case 0x31:
                                            else if (ReceiveBts[4] == 0x31)
                                            {
                                                byte[] K = new byte[24];
                                                for (int i = 0; i < 24; i++)
                                                {
                                                    K[i] = ReceiveBts[46 + i];
                                                    IS[i] = ReceiveBts[46 + i];
                                                    ISSET[i] = ReceiveBts[46 + i];
                                                }
                                                setimgONORNOT(K);
                                                timer1.Stop();
                                            }
                                            //  break;
                                            //  case 0x32:
                                            else if (ReceiveBts[4] == 0x32)
                                            {
                                                string KOSTR = STB(ReceiveBts, 1017);
                                                string[] BOKSTR = KOSTR.Split(' ');
                                                zheng.Text = "通道"+ Convert.ToInt32(BOKSTR[8]) + "已处理第:" + Convert.ToInt32(BOKSTR[11] + BOKSTR[12].ToString(), 16) + "帧/"+ TTCPSD.Text;
                                                for (int t = 13+4; t < 1011; t = t + 4)
                                                {
                                                    string byte3tostr = BOKSTR[t] + BOKSTR[t + 1] + BOKSTR[t + 2];
                                                    int temp_num = Convert.ToInt32(byte3tostr, 16);
                                                    if (Convert.ToInt32(BOKSTR[t], 16) >= 0x80)
                                                    {
                                                        temp_num = temp_num - 0xFFFFFF;
                                                    }
                                                    double Voltage = (Convert.ToDouble(((temp_num / Math.Pow(2, 24)) * 4.809).ToString("0.000")));
                                                    sw.WriteLine(Voltage.ToString());
                                                    sw1.WriteLine(Convert.ToInt32(BOKSTR[11] + BOKSTR[12].ToString(), 16) + "#" + byte3tostr); 
                                                    
                                                }
                                                TCPB = false;
                                            }
                                        }
                                    }
                                }
                                TTCP.Enabled = true;
                            }
                            catch (Exception ex)
                            {
                                TTCP.Enabled = true;
                                //MessageBox.Show(ex.ToString());
                            }
                        }
                    });
                    ReceiveTh.IsBackground = true;
                    ReceiveTh.Start();
                }
            }
            catch (Exception EX)
            {
                TTCP.Enabled = true;
                //MessageBox.Show(EX.ToString());
            }
        }

        private void SETMESS(string A)
        {
            //tcpmessBox
            tcpmessBox.Text = tcpmessBox.Text + "\n" + A;
        }

        /// <summary>
        /// 检测socket连接状态
        /// </summary>
        /// <param name="clisck">Socket对象</param>
        /// <returns></returns>
        bool clientSckIsConnected(Socket clisck)
        {
            bool result = false;
            if (clisck != null && clisck.Connected)
            {
                try
                {
                    clisck.Send(new byte[] { });
                    result = true;
                }
                catch (Exception)
                {
                }
            }
            return result;
        }


        /// <summary>
        /// 用于创建TCP连接请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click_1(object sender, EventArgs e)
        {
            button8.Enabled = false;
            SETMESS("寻找网络设备中");
            new Task(jiesoushuju).Start();
            

            FileStream fs = new FileStream(@".\config.ini", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(IP.Text + "#" + Parse.Text);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();

            return;

            ThreadPool.QueueUserWorkItem(s =>
            {
                
                
                setimg();
                Thread.Sleep(100);
                try
                {
                    if (button8.Text == "打开连接")
                    {
                        IPE = new IPEndPoint(IPAddress.Parse(IP.Text), int.Parse(Parse.Text));


                        SK = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //建立连接
                        SK.Connect(IPE);
                        //button8.Text = "关闭连接";
                        tcpmessBox.Text = tcpmessBox.Text + "\n" + "网络连接成功";
                        TTCP.Enabled = true;
                        
                        button8.Enabled = true;
                    }
                    //else if (button8.Text == "关闭连接")
                    //{
                    //    if (SK.IsBound == true)
                    //    {
                    //        tcpmessBox.Text = tcpmessBox.Text + "\n" + "关闭网络连接中";

                    //        TTCP.Enabled = false;
                    //        //关闭socket连接
                    //        SK.Close();
                    //        SK.Dispose();
                    //        button8.Text = "打开连接";
                    //        tcpmessBox.Text = tcpmessBox.Text + "\n" + "网络关闭成功";
                    //    }
                    //}

                }
                catch (Exception ex)
                {
                    button8.Enabled = true;
                    // MessageBox.Show(ex.ToString());
                }

            });
        }

     

        private void button9_Click(object sender, EventArgs e)
        {




        }

        /// <summary>
        /// 网络部分,查询网络状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                button8.Enabled = false;
                ThreadPool.QueueUserWorkItem(s =>
                {
                    setimg();
                    Thread.Sleep(100);

                    TTCP.Enabled = false;

                    List<byte> gobyte = new List<byte>();
                    gobyte.AddRange(new byte[] { 0xFA, 0xFB });
                    gobyte.AddRange(new byte[] { 0x00, 0x01 });
                    gobyte.AddRange(new byte[] { 0x11 });
                    gobyte.AddRange(new byte[] { 0x40 });
                    for (int i = 0; i < 40; i++)
                    {
                        gobyte.AddRange(new byte[] { 0x00 });
                    }
                    for (int i = 0; i < 24; i++)
                    {
                        gobyte.AddRange(new byte[] { 0x01 });
                    }

                    gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                    gobyte.AddRange(new byte[] { 0xFE });

                    byte[] outbyte = gobyte.ToArray();

                    //发送数据

                    SK.Send(outbyte, outbyte.Length, 0);
                    tcpmessBox.Text = tcpmessBox.Text + "\n" + "发送命令;" + STB(outbyte, outbyte.Length);



                    //byte[] intbyte = new byte[72];
                    //int len = SK.Receive(intbyte);

                    //tcpmessBox.Text = tcpmessBox.Text + "\n" + "返回命令;" + STB(intbyte, intbyte.Length);
                    //if (len == 72)
                    //{
                    //    if (intbyte[0] == 0xFC && intbyte[1] == 0xFD && intbyte[71] == 0xFE)
                    //    {
                    //        //46
                    //        byte[] K = new byte[24];
                    //        for (int i = 0; i < 24; i++)
                    //        {
                    //            K[i] = intbyte[46 + i];
                    //            IS[i] = intbyte[46 + i];
                    //            ISSET[i] = intbyte[46 + i];
                    //            setimgONORNOT(K);
                    //        }
                    //    }
                    //}
                    //button8.Enabled = true;
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                button8.Enabled = true;
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                button8.Enabled = false;
                setimg();
                Thread.Sleep(100);
                TTCP.Enabled = false;
                ThreadPool.QueueUserWorkItem(s =>
                {
                    List<byte> gobyte = new List<byte>();
                    gobyte.AddRange(new byte[] { 0xFA, 0xFB });
                    gobyte.AddRange(new byte[] { 0x00, 0x01 });
                    gobyte.AddRange(new byte[] { 0x21 });
                    gobyte.AddRange(new byte[] { 0x40 });
                    for (int i = 0; i < 32; i++)
                    {
                        gobyte.AddRange(new byte[] { 0x00 });
                    }
                    gobyte.AddRange(new byte[] { 0x00, BitConverter.GetBytes(int.Parse(TTCPBS.Text))[0] });

                    gobyte.AddRange(new byte[] { BitConverter.GetBytes(int.Parse(TTCPSD.Text))[1], BitConverter.GetBytes(int.Parse(TTCPSD.Text))[0] });
                    gobyte.AddRange(new byte[] { 0x00, BitConverter.GetBytes(int.Parse(TTCPCD.Text))[0] });
                    gobyte.AddRange(new byte[] { 0x00, BitConverter.GetBytes(int.Parse(TTCPJG.Text))[0] });
                    gobyte.AddRange(IS);
                    gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                    gobyte.AddRange(new byte[] { 0xFE });

                    byte[] outbyte = gobyte.ToArray();

                    //发送数据
                    SK.Send(outbyte, outbyte.Length, 0);
                    tcpmessBox.Text = tcpmessBox.Text + "\n" + "发送命令;" + STB(outbyte, outbyte.Length);



                    //byte[] intbyte = new byte[72];
                    //int len = SK.Receive(intbyte);
                    //tcpmessBox.Text = tcpmessBox.Text + "\n" + "返回命令;" + STB(intbyte, intbyte.Length);
                    //if (len == 72)
                    //{
                    //    if (intbyte[0] == 0xFC && intbyte[1] == 0xFD && intbyte[71] == 0xFE)
                    //    {
                    //        //46
                    //        byte[] K = new byte[24];
                    //        for (int i = 0; i < 24; i++)
                    //        {
                    //            K[i] = intbyte[46 + i];
                    //            IS[i] = intbyte[46 + i];
                    //            ISSET[i] = intbyte[46 + i];
                    //            setimgONORNOT(K);
                    //        }
                    //    }
                    //}
                    //button8.Enabled = true;
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                button8.Enabled = true;
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                button8.Enabled = false;
                timer1.Start();
                TTCP.Enabled = false;
                ThreadPool.QueueUserWorkItem(s =>
                {
                    tcpmessBox.Text = tcpmessBox.Text + "\n" + "调试信息:线程\"启动采集\"已建立";
                    List<byte> gobyte = new List<byte>();
                    gobyte.AddRange(new byte[] { 0xFA, 0xFB });
                    gobyte.AddRange(new byte[] { 0x00, 0x01 });
                    gobyte.AddRange(new byte[] { 0x31 });
                    gobyte.AddRange(new byte[] { 0x40 });
                    for (int i = 0; i < 40; i++)
                    {
                        gobyte.AddRange(new byte[] { 0x00 });
                    }
                    //byte[] ASDASDAS = ISSET;
                    //ASDASDAS[0] = 0x00;
                    //ASDASDAS[23] = 0x01;
                    //gobyte.AddRange(ASDASDAS);
                    gobyte.AddRange(ISSET);
                    gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                    gobyte.AddRange(new byte[] { 0xFE });

                    byte[] outbyte = gobyte.ToArray();
                    tcpmessBox.Text = tcpmessBox.Text + "\n" + "调试信息:控制数组已建立,长度:" + outbyte.Length.ToString();
                    //发送数据


                    SK.Send(outbyte, outbyte.Length, 0);
                    tcpmessBox.Text = tcpmessBox.Text + "\n" + "发送命令;" + STB(outbyte, outbyte.Length);



                    //byte[] intbyte = new byte[72];
                    //tcpmessBox.Text = tcpmessBox.Text + "\n" + "调试信息:等待返回数据";
                    //int len = SK.Receive(intbyte);
                    //tcpmessBox.Text = tcpmessBox.Text + "\n" + "返回命令;" + STB(intbyte, intbyte.Length);
                    //if (len == 72)
                    //{
                    //    if (intbyte[0] == 0xFC && intbyte[1] == 0xFD && intbyte[71] == 0xFE)
                    //    {
                    //        //46
                    //        byte[] K = new byte[24];
                    //        for (int i = 0; i < 24; i++)
                    //        {
                    //            K[i] = intbyte[46 + i];
                    //            IS[i] = intbyte[46 + i];
                    //            ISSET[i] = intbyte[46 + i];
                    //            setimgONORNOT(K);
                    //        }
                    //    }
                    //}
                    //button8.Enabled = true;
                    //tcpmessBox.Text = tcpmessBox.Text + "\n" + "调试信息:线程\"启动采集\"已结束";
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                button8.Enabled = true;
            }
        }

        string path = "";
        string YSpath = "";
        StreamWriter sw;
        StreamWriter sw1;

        bool TCPB = true;

        public static bool UpDateBool = true;

        public void upDate()
        {
            setimgONORNOT(CJ.setByte);
            TCPB = false;
            try
            {
                cjtime = 0;
                timer2.Start();
                button8.Enabled = false;
                TTCP.Enabled = false;
                ThreadPool.QueueUserWorkItem(s =>
                {
                    int ch = 0;
                    for (byte i = 0x00; i < 0x18; i++)
                    {
                        if (ISSET[i] == 0x00)
                        {
                            ch++;
                        }
                    }
                    if (ch >= 24)
                    {
                        MessageBox.Show("所有通道全部为空", "错误");
                        return;
                    }
                    for (byte x = 0x01; x < 0x19; x++)
                    {
                        if (ISSET[x - 1] != 0x00)
                        {
                            string pathon = STB(new byte[] { x }, 1);

                            path = @".\txt\" + pathon.Replace(" ", "") + ".txt";//文件存放路径，保证文件存在。
                            YSpath = @".\txt\YS" + pathon.Replace(" ", "") + ".txt";//文件存放路径，保证文件存在。
                            File.Delete(path);
                            File.Delete(YSpath);
                            sw = new StreamWriter(path, true);
                            sw1 = new StreamWriter(YSpath, true);

                            for (int z = 0; z < int.Parse(TTCPSD.Text); z++)
                            {
                                while (TCPB)
                                {
                                    ;
                                }
                                byte[] AA = BitConverter.GetBytes(z + 1);
                                List<byte> gobyte = new List<byte>();
                                gobyte.AddRange(new byte[] { 0xFA, 0xFB });
                                gobyte.AddRange(new byte[] { 0x00, 0x01 });
                                gobyte.AddRange(new byte[] { 0x32 });
                                gobyte.AddRange(new byte[] { 0x40 });
                                for (int i = 0; i < 61; i++)
                                {
                                    gobyte.AddRange(new byte[] { 0x00 });
                                }
                                gobyte.AddRange(new byte[] { x });
                                gobyte.AddRange(new byte[] { AA[1], AA[0] });
                                gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                                gobyte.AddRange(new byte[] { 0xFE });

                                byte[] outbyte = gobyte.ToArray();


                                //发送数据
                                SK.Send(outbyte, outbyte.Length, 0);
                                TCPB = true;
                                //tcpmessBox.Text = tcpmessBox.Text + "\n" + "发送命令;" + STB(outbyte, outbyte.Length);
                                //byte[] intbyte = new byte[1017];
                                //int len = SK.Receive(intbyte);
                                //if (len == 1017)
                                //{
                                //    if (intbyte[0] == 0xFC && intbyte[1] == 0xFD && intbyte[1017] == 0xFE)
                                //    {


                                //        string KOSTR = STB(intbyte, 1017);
                                //        string[] BOKSTR = KOSTR.Split(' ');

                                //        //listBox2.Items.Add("第"+(i+1).ToString()+"帧:" + KOSTR);

                                //        for (int t = 13; t < 1011; t = t + 4)
                                //        {
                                //            string byte3tostr = BOKSTR[t] + BOKSTR[t + 1] + BOKSTR[t + 2];
                                //            int temp_num = Convert.ToInt32(byte3tostr, 16);
                                //            if (Convert.ToInt32(BOKSTR[t], 16) >= 0x80)
                                //            {
                                //                temp_num = temp_num - 0xFFFFFF;
                                //            }
                                //            double jieguo = (Convert.ToDouble(((temp_num / Math.Pow(2, 24)) * 4.809).ToString("0.000")));
                                //            sw.WriteLine(jieguo.ToString());
                                //        }
                                //    }
                                //}
                            }
                            Thread.Sleep(300);
                            sw.Close();
                            sw1.Close();
                            SETMESS("通道" + pathon + "上传完成" + UPTIME.Text);
                            
                            cjtime = 0;
                        }
                    }
                    timer2.Stop();
                    button8.Enabled = true;
                    TCPB = true;
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                button8.Enabled = true;
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            UpDateBool = true;
            CJ.ShowDialog();
            while(UpDateBool)
            {
                ;
            }
            upDate();
        }

        private void P1_Click(object sender, EventArgs e)
        {
            if (IS[0] == 0x01)
            {
                if (ISSET[0] == 0x01)
                {
                    P1.Image = Properties.Resources.红;
                    ISSET[0] = 0x00;
                }
                else if (ISSET[0] == 0x00)
                {
                    P1.Image = Properties.Resources.绿;
                    ISSET[0] = 0x01;
                }
            }
        }

        private void P2_Click(object sender, EventArgs e)
        {
            if (IS[1] == 0x01)
            {
                if (ISSET[1] == 0x01)
                {
                    P2.Image = Properties.Resources.红;
                    ISSET[1] = 0x00;
                }
                else if (ISSET[1] == 0x00)
                {
                    P2.Image = Properties.Resources.绿;
                    ISSET[1] = 0x01;
                }
            }
        }

        private void P3_Click(object sender, EventArgs e)
        {
            if (IS[2] == 0x01)
            {
                if (ISSET[2] == 0x01)
                {
                    P3.Image = Properties.Resources.红;
                    ISSET[2] = 0x00;
                }
                else if (ISSET[2] == 0x00)
                {
                    P3.Image = Properties.Resources.绿;
                    ISSET[2] = 0x01;
                }
            }
        }

        private void P4_Click(object sender, EventArgs e)
        {
            if (IS[3] == 0x01)
            {
                if (ISSET[3] == 0x01)
                {
                    P4.Image = Properties.Resources.红;
                    ISSET[3] = 0x00;
                }
                else if (ISSET[3] == 0x00)
                {
                    P4.Image = Properties.Resources.绿;
                    ISSET[3] = 0x01;
                }
            }
        }

        private void P5_Click(object sender, EventArgs e)
        {
            if (IS[4] == 0x01)
            {
                if (ISSET[4] == 0x01)
                {
                    P5.Image = Properties.Resources.红;
                    ISSET[4] = 0x00;
                }
                else if (ISSET[4] == 0x00)
                {
                    P5.Image = Properties.Resources.绿;
                    ISSET[4] = 0x01;
                }
            }
        }

        private void P6_Click(object sender, EventArgs e)
        {
            if (IS[5] == 0x01)
            {
                if (ISSET[5] == 0x01)
                {
                    P6.Image = Properties.Resources.红;
                    ISSET[5] = 0x00;
                }
                else if (ISSET[5] == 0x00)
                {
                    P6.Image = Properties.Resources.绿;
                    ISSET[5] = 0x01;
                }
            }
        }

        private void P7_Click(object sender, EventArgs e)
        {
            if (IS[6] == 0x01)
            {
                if (ISSET[6] == 0x01)
                {
                    P7.Image = Properties.Resources.红;
                    ISSET[6] = 0x00;
                }
                else if (ISSET[6] == 0x00)
                {
                    P7.Image = Properties.Resources.绿;
                    ISSET[6] = 0x01;
                }
            }
        }

        private void P8_Click(object sender, EventArgs e)
        {
            if (IS[7] == 0x01)
            {
                if (ISSET[7] == 0x01)
                {
                    P8.Image = Properties.Resources.红;
                    ISSET[7] = 0x00;
                }
                else if (ISSET[7] == 0x00)
                {
                    P8.Image = Properties.Resources.绿;
                    ISSET[7] = 0x01;
                }
            }
        }

        private void P9_Click(object sender, EventArgs e)
        {
            if (IS[8] == 0x01)
            {
                if (ISSET[8] == 0x01)
                {
                    P9.Image = Properties.Resources.红;
                    ISSET[8] = 0x00;
                }
                else if (ISSET[8] == 0x00)
                {
                    P9.Image = Properties.Resources.绿;
                    ISSET[8] = 0x01;
                }
            }
        }

        private void P10_Click(object sender, EventArgs e)
        {
            if (IS[9] == 0x01)
            {
                if (ISSET[9] == 0x01)
                {
                    P10.Image = Properties.Resources.红;
                    ISSET[9] = 0x00;
                }
                else if (ISSET[9] == 0x00)
                {
                    P10.Image = Properties.Resources.绿;
                    ISSET[9] = 0x01;
                }
            }
        }

        private void P11_Click(object sender, EventArgs e)
        {
            if (IS[10] == 0x01)
            {
                if (ISSET[10] == 0x01)
                {
                    P11.Image = Properties.Resources.红;
                    ISSET[10] = 0x00;
                }
                else if (ISSET[10] == 0x00)
                {
                    P11.Image = Properties.Resources.绿;
                    ISSET[10] = 0x01;
                }
            }
        }

        private void P12_Click(object sender, EventArgs e)
        {
            if (IS[11] == 0x01)
            {
                if (ISSET[11] == 0x01)
                {
                    P12.Image = Properties.Resources.红;
                    ISSET[11] = 0x00;
                }
                else if (ISSET[11] == 0x00)
                {
                    P12.Image = Properties.Resources.绿;
                    ISSET[11] = 0x01;
                }
            }
        }

        private void P13_Click(object sender, EventArgs e)
        {
            if (IS[12] == 0x01)
            {
                if (ISSET[12] == 0x01)
                {
                    P13.Image = Properties.Resources.红;
                    ISSET[12] = 0x00;
                }
                else if (ISSET[12] == 0x00)
                {
                    P13.Image = Properties.Resources.绿;
                    ISSET[12] = 0x01;
                }
            }
        }

        private void P14_Click(object sender, EventArgs e)
        {
            if (IS[13] == 0x01)
            {
                if (ISSET[13] == 0x01)
                {
                    P14.Image = Properties.Resources.红;
                    ISSET[13] = 0x00;
                }
                else if (ISSET[13] == 0x00)
                {
                    P14.Image = Properties.Resources.绿;
                    ISSET[13] = 0x01;
                }
            }
        }

        private void P15_Click(object sender, EventArgs e)
        {
            if (IS[14] == 0x01)
            {
                if (ISSET[14] == 0x01)
                {
                    P15.Image = Properties.Resources.红;
                    ISSET[14] = 0x00;
                }
                else if (ISSET[14] == 0x00)
                {
                    P15.Image = Properties.Resources.绿;
                    ISSET[14] = 0x01;
                }
            }
        }

        private void P16_Click(object sender, EventArgs e)
        {
            if (IS[15] == 0x01)
            {
                if (ISSET[15] == 0x01)
                {
                    P16.Image = Properties.Resources.红;
                    ISSET[15] = 0x00;
                }
                else if (ISSET[15] == 0x00)
                {
                    P16.Image = Properties.Resources.绿;
                    ISSET[15] = 0x01;
                }
            }
        }

        private void P17_Click(object sender, EventArgs e)
        {
            if (IS[16] == 0x01)
            {
                if (ISSET[16] == 0x01)
                {
                    P17.Image = Properties.Resources.红;
                    ISSET[16] = 0x00;
                }
                else if (ISSET[16] == 0x00)
                {
                    P17.Image = Properties.Resources.绿;
                    ISSET[16] = 0x01;
                }
            }
        }

        private void P18_Click(object sender, EventArgs e)
        {
            if (IS[17] == 0x01)
            {
                if (ISSET[17] == 0x01)
                {
                    P18.Image = Properties.Resources.红;
                    ISSET[17] = 0x00;
                }
                else if (ISSET[17] == 0x00)
                {
                    P18.Image = Properties.Resources.绿;
                    ISSET[17] = 0x01;
                }
            }
        }

        private void P19_Click(object sender, EventArgs e)
        {
            if (IS[18] == 0x01)
            {
                if (ISSET[18] == 0x01)
                {
                    P19.Image = Properties.Resources.红;
                    ISSET[18] = 0x00;
                }
                else if (ISSET[18] == 0x00)
                {
                    P19.Image = Properties.Resources.绿;
                    ISSET[18] = 0x01;
                }
            }
        }

        private void P20_Click(object sender, EventArgs e)
        {
            if (IS[19] == 0x01)
            {
                if (ISSET[19] == 0x01)
                {
                    P20.Image = Properties.Resources.红;
                    ISSET[19] = 0x00;
                }
                else if (ISSET[19] == 0x00)
                {
                    P20.Image = Properties.Resources.绿;
                    ISSET[19] = 0x01;
                }
            }
        }

        private void P21_Click(object sender, EventArgs e)
        {
            if (IS[20] == 0x01)
            {
                if (ISSET[20] == 0x01)
                {
                    P21.Image = Properties.Resources.红;
                    ISSET[20] = 0x00;
                }
                else if (ISSET[20] == 0x00)
                {
                    P21.Image = Properties.Resources.绿;
                    ISSET[20] = 0x01;
                }
            }
        }

        private void P22_Click(object sender, EventArgs e)
        {
            if (IS[21] == 0x01)
            {
                if (ISSET[21] == 0x01)
                {
                    P22.Image = Properties.Resources.红;
                    ISSET[21] = 0x00;
                }
                else if (ISSET[21] == 0x00)
                {
                    P22.Image = Properties.Resources.绿;
                    ISSET[21] = 0x01;
                }
            }
        }

        private void P23_Click(object sender, EventArgs e)
        {
            if (IS[22] == 0x01)
            {
                if (ISSET[22] == 0x01)
                {
                    P23.Image = Properties.Resources.红;
                    ISSET[22] = 0x00;
                }
                else if (ISSET[22] == 0x00)
                {
                    P23.Image = Properties.Resources.绿;
                    ISSET[22] = 0x01;
                }
            }
        }

        private void P24_Click(object sender, EventArgs e)
        {
            if (IS[23] == 0x01)
            {
                if (ISSET[23] == 0x01)
                {
                    P24.Image = Properties.Resources.红;
                    ISSET[23] = 0x00;
                }
                else if (ISSET[23] == 0x00)
                {
                    P24.Image = Properties.Resources.绿;
                    ISSET[23] = 0x01;
                }
            }
        }



        private void set2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (set2.Text == "5")
            {
                set3.Text = "200";
            }
            if (set2.Text == "8")
            {
                set3.Text = "300";
            }
            if (set2.Text == "13")
            {
                set3.Text = "500";
            }
            if (set2.Text == "20")
            {
                set3.Text = "800";
            }
            if (set2.Text == "25")
            {
                set3.Text = "1000";
            }
            if (set2.Text == "51")
            {
                set3.Text = "2000";
            }
        }

        private void set3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (set3.Text == "200")
            {
                set2.Text = "5";
            }
            if (set3.Text == "300")
            {
                set2.Text = "8";
            }
            if (set3.Text == "500")
            {
                set2.Text = "13";
            }
            if (set3.Text == "800")
            {
                set2.Text = "20";
            }
            if (set3.Text == "1000")
            {
                set2.Text = "25";
            }
            if (set3.Text == "2000")
            {
                set2.Text = "51";
            }


        }
        private void TTCPSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TTCPSD.Text == "200")
            {
                TTCPCD.Text = "5";
            }
            if (TTCPSD.Text == "300")
            {
                TTCPCD.Text = "8";
            }
            if (TTCPSD.Text == "500")
            {
                TTCPCD.Text = "13";
            }
            if (TTCPSD.Text == "800")
            {
                TTCPCD.Text = "20";
            }
            if (TTCPSD.Text == "1000")
            {
                TTCPCD.Text = "25";
            }
            if (TTCPSD.Text == "2000")
            {
                TTCPCD.Text = "51";
            }
        }
        private void TTCPCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TTCPCD.Text == "5")
            {
                TTCPSD.Text = "200";
            }
            if (TTCPCD.Text == "8")
            {
                TTCPSD.Text = "300";
            }
            if (TTCPCD.Text == "13")
            {
                TTCPSD.Text = "500";
            }
            if (TTCPCD.Text == "20")
            {
                TTCPSD.Text = "800";
            }
            if (TTCPCD.Text == "25")
            {
                TTCPSD.Text = "1000";
            }
            if (TTCPCD.Text == "51")
            {
                TTCPSD.Text = "2000";
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            STX STX = new STX();
            //path = @"D:\AAAAAA\ 01.txt";//文件存放路径，保证文件存在。

            //sw = new StreamWriter(path, true);

            //sw.WriteLine("01");

            //sw.Close();
            STX.Show();
        }

        private void TTCP_Enter(object sender, EventArgs e)
        {

        }


        double cjtime = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            cjsc.Text = "采集时长:" + cjtime.ToString("0.0") + "秒";
            cjtime = cjtime + 0.1;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            UPTIME.Text = "上传用时:" + cjtime.ToString("0.0") + "秒";
            cjtime = cjtime + 0.1;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            TCPB = false;
            for (byte x = 0x01; x < 0x19; x++)
            {
                //if (ISSET[x - 1] != 0x00)
                {
                    string pathon = STB(new byte[] { x }, 1);

                    path = @".\txt\" + pathon.Replace(" ", "") + ".txt";//文件存放路径，保证文件存在。
                    YSpath = @".\txt\YS" + pathon.Replace(" ", "") + ".txt";//文件存放路径，保证文件存在。
                    File.Delete(path);
                    File.Delete(YSpath);
                    sw = new StreamWriter(path, true);
                    sw1 = new StreamWriter(YSpath, true);

                    for (int z = 0; z < int.Parse(TTCPSD.Text); z++)
                    {
                        
                        while (TCPB)
                        {
                            ;
                        }
                        byte[] AA = BitConverter.GetBytes(z + 1);
                        List<byte> gobyte = new List<byte>();
                        gobyte.AddRange(new byte[] { 0xFA, 0xFB });
                        gobyte.AddRange(new byte[] { 0x00, 0x01 });
                        gobyte.AddRange(new byte[] { 0x32 });
                        gobyte.AddRange(new byte[] { 0x40 });
                        for (int i = 0; i < 61; i++)
                        {
                            gobyte.AddRange(new byte[] { 0x00 });
                        }
                        gobyte.AddRange(new byte[] { x });
                        gobyte.AddRange(new byte[] { AA[1], AA[0] });
                        gobyte.AddRange(new byte[] { GetVerifyCodeByte(gobyte.ToArray()) });
                        gobyte.AddRange(new byte[] { 0xFE });

                        byte[] outbyte = gobyte.ToArray();


                        //发送数据
                        SK.Send(outbyte, outbyte.Length, 0);
                        TCPB = true;
                        //tcpmessBox.Text = tcpmessBox.Text + "\n" + "发送命令;" + STB(outbyte, outbyte.Length);
                        //byte[] intbyte = new byte[1017];
                        //int len = SK.Receive(intbyte);
                        //if (len == 1017)
                        //{
                        //    if (intbyte[0] == 0xFC && intbyte[1] == 0xFD && intbyte[1017] == 0xFE)
                        //    {


                        //        string KOSTR = STB(intbyte, 1017);
                        //        string[] BOKSTR = KOSTR.Split(' ');

                        //        //listBox2.Items.Add("第"+(i+1).ToString()+"帧:" + KOSTR);

                        //        for (int t = 13; t < 1011; t = t + 4)
                        //        {
                        //            string byte3tostr = BOKSTR[t] + BOKSTR[t + 1] + BOKSTR[t + 2];
                        //            int temp_num = Convert.ToInt32(byte3tostr, 16);
                        //            if (Convert.ToInt32(BOKSTR[t], 16) >= 0x80)
                        //            {
                        //                temp_num = temp_num - 0xFFFFFF;
                        //            }
                        //            double jieguo = (Convert.ToDouble(((temp_num / Math.Pow(2, 24)) * 4.809).ToString("0.000")));
                        //            sw.WriteLine(jieguo.ToString());
                        //        }
                        //    }
                        //}
                    }
                    Thread.Sleep(300);
                    sw.Close();
                    sw1.Close();
                    SETMESS("通道" + pathon + "上传完成" + UPTIME.Text);
                    cjtime = 0;
                }
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            

            string byte3tostr = "040306";
            int temp_num = Convert.ToInt32(byte3tostr, 16);
          
            double Voltage = (Convert.ToDouble(((temp_num / Math.Pow(2, 24)) * 4.809).ToString("0.000")));
            
            
        }
    }
}
