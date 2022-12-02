using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace StressHeadset
{
    public partial class Form1 : Form
    {
        private int pageNo = 0;

        Day.Mainform mainform;
        Day.Day1 day1;
        Day.Day2 day2;
        Day.Day3 day3;
        Day.Day4 day4;
        Day.Day5 day5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //==========================================================
            mainform = new Day.Mainform();
            mainform.TopLevel = false;
            mainform.Dock = DockStyle.Fill;
            mainform.sendCommand += main_sendCommand;
            this.Controls.Add(mainform);
            //==========================================================
            day1 = new Day.Day1();
            day1.TopLevel = false;
            day1.Dock = DockStyle.Fill;
            day1.sendCommand += main_sendCommand;
            this.Controls.Add(day1);
            //==========================================================
            day2 = new Day.Day2();
            day2.TopLevel = false;
            day2.Dock = DockStyle.Fill;
            day2.sendCommand += main_sendCommand;
            this.Controls.Add(day2);
            //==========================================================
            day3 = new Day.Day3();
            day3.TopLevel = false;
            day3.Dock = DockStyle.Fill;
            day3.sendCommand += main_sendCommand;
            this.Controls.Add(day3);
            //==========================================================
            day4 = new Day.Day4();
            day4.TopLevel = false;
            day4.Dock = DockStyle.Fill;
            day4.sendCommand += main_sendCommand;
            this.Controls.Add(day4);
            //==========================================================
            day5 = new Day.Day5();
            day5.TopLevel = false;
            day5.Dock = DockStyle.Fill;
            day5.sendCommand += main_sendCommand;
            this.Controls.Add(day5);
            //==========================================================

            mainform.Visible = true;
        }

        private void main_sendCommand(string text)
        {
            if (text.Equals("mainform"))
            {
                day1.Visible = false;
                day2.Visible = false;
                day3.Visible = false;
                day4.Visible = false;
                day5.Visible = false;
                mainform.Visible = true;
            }
            else if (text.Equals("day1"))
            {
                day2.Visible = false;
                day3.Visible = false;
                day4.Visible = false;
                day5.Visible = false;
                mainform.Visible = false;

                day1.Visible = true;
            }
            else if (text.Equals("day2"))
            {
                day1.Visible = false;
                day3.Visible = false;
                day4.Visible = false;
                day5.Visible = false;
                mainform.Visible = false;

                day2.Visible = true;
            }
            else if (text.Equals("day3"))
            {
                day1.Visible = false;
                day2.Visible = false;
                day4.Visible = false;
                day5.Visible = false;
                mainform.Visible = false;

                day3.Visible = true;
            }
            else if (text.Equals("day4"))
            {
                day1.Visible = false;
                day2.Visible = false;
                day3.Visible = false;
                day5.Visible = false;
                mainform.Visible = false;

                day4.Visible = true;
            }
            else if (text.Equals("day5"))
            {
                day1.Visible = false;
                day2.Visible = false;
                day3.Visible = false;
                day4.Visible = false;
                mainform.Visible = false;

                day5.Visible = true;
            }
        }
    }
}
