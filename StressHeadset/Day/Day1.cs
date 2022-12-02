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
    public partial class Day1 : Form
    {
        internal delegate void sendCommandDele(string text);
        internal event sendCommandDele sendCommand;
        public Day1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendCommand.Invoke("mainform");
        }
    }
}
