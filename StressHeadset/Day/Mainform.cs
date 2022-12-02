using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StressHeadset.Day
{
    public partial class Mainform : Form
    {
        internal delegate void sendCommandDele(string text);
        internal event sendCommandDele sendCommand;

        public Mainform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendCommand.Invoke("day1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendCommand.Invoke("day2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendCommand.Invoke("day3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sendCommand.Invoke("day4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendCommand.Invoke("day5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string fileFullPath = @"C\imsi.txt";
            string key = "abc";

            UtilModel.UpdateFileSkipRow(fileFullPath, key);

            List<string> keys = new List<string>();
            keys.Add("abc");
            keys.Add("123");
            keys.Add("eee");

            UtilModel.UpdateFileSkipRow(fileFullPath, keys);
        }
    }
}
