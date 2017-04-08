using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 检波器项目01
{
    public partial class CJ : Form
    {
       
        public CJ()
        {
            
            InitializeComponent();
        }

        CheckBox[] cBox = new CheckBox[24];
        

        private void CJ_Load(object sender, EventArgs e)
        {
            this.Size = new Size(550, 200);
            for (int i = 0; i < 24; i++)
            {
                cBox[i] = new CheckBox();
                cBox[i].Size = new Size(40, 23);
                cBox[i].Text = (i + 1).ToString();
                cBox[i].Click += new EventHandler(buutclick);
                if (i < 12)
                {
                    cBox[i].Location = new Point(12 + 43 * i, 40 + (45 * (i / 12)));
                }
                else
                {
                    cBox[i].Location = new Point((12 + 43 * i) - 516, 40 + (45 * (i / 12)));
                }
                this.Controls.Add(cBox[i]);
                cBox[i].Visible = true;
                cBox[i].Enabled = true;
            }

            for (int i = 0; i < 24; i++)
            {
                if(F1.IS[i]==0x00)
                {
                    cBox[i].Enabled = false;
                }
                if (F1.IS[i] == 0x01)
                {
                    cBox[i].Enabled = true;
                }
            }
        }

        public static string file = "";
        public static byte[] setByte = new byte[24];
        private void buutclick(object sender, EventArgs e)
        {

            //CheckBox lbl = (CheckBox)(sender); //这么一转换就知道是哪个label点击了
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                for (int i = 0; i < 24; i++)
                {
                    cBox[i].Checked = false;
                }
            }
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < 24; i++)
                {
                    cBox[i].Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 24; i++)
            {
                if(cBox[i].Checked==true)
                {
                    F1.ISSET[i] = 0x01;
                    setByte[i] = 0x01;
                }
                if (cBox[i].Checked == false)
                {
                    F1.ISSET[i] = 0x00;
                    setByte[i] = 0x00;
                }
            }
            F1.UpDateBool = false;
            CJ.ActiveForm.Close();
        }
    }
}
