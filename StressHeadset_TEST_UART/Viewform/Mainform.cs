using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace StressHeadset_TEST_UART.Viewform
{
    public partial class Mainform : Form
    {
        internal delegate void sendCommandDele(string text);
        internal event sendCommandDele sendCommand;

        string curFileFullPath;
        string curMediaFileName;

        List<FileInfo> curMusicList = new List<FileInfo>();

        int curIndex = -1;

        static int Idletime = 900;
        static int Reset_idletime = 900;

        public Mainform()
        {
            InitializeComponent();
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("1일차");
            comboBox1.Items.Add("2일차");
            comboBox1.Items.Add("3일차");
            comboBox1.Items.Add("4일차");
            comboBox1.Items.Add("5일차");
            comboBox1.SelectedIndex = 0;
            Initial();
            Select_path("1");
        }

        private void Initial()
        {
            button16.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button4.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button13.Enabled = false;
            button14.Enabled = false;
            button20.Enabled = false;
        }

        private void Start_btn_enable(bool enable)
        {
            if (enable == true)
            {
                button17.Enabled = true;
                button1.Enabled = true;
                button6.Enabled = true;
                button9.Enabled = true;
                button12.Enabled = true;
                button15.Enabled = true;
                button18.Enabled = true;
                button19.Enabled = true;
            }
            else if (enable == false)
            {
                button17.Enabled = false;
                button1.Enabled = false;
                button6.Enabled = false;
                button9.Enabled = false;
                button12.Enabled = false;
                button15.Enabled = false;
                button18.Enabled = false;
                button19.Enabled = false;
            }
        }

        private void Select_path(string num)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            string path = Directory.GetCurrentDirectory() + "\\mp3\\" + num;
            dialog.SelectedPath = path;

            string printFolderText = dialog.SelectedPath;
            const int limitLength = 15;
            if (printFolderText.Length > limitLength)
            {
                printFolderText = printFolderText.Remove(0, printFolderText.Length - limitLength);
                printFolderText = "..." + printFolderText;
            }
            label16.Text = "폴더 경로 : " + printFolderText;

            DirectoryInfo di = new DirectoryInfo(dialog.SelectedPath);
            List<FileInfo> musicItemList = new List<FileInfo>();
            FindMP3Files(di, musicItemList);

            SetMusicList(musicItemList);
        }

        public void SetMusicList(List<FileInfo> musicList)
        {
            curMusicList.Clear();

            foreach (FileInfo item in musicList)
            {
                curMusicList.Add(item);
            }

            curIndex = 0;
            curMediaFileName = curMusicList[curIndex].Name;
            curFileFullPath = curMusicList[curIndex].FullName;
        }

        public void setMusicList(int index)
        {
            curMediaFileName = curMusicList[index].Name;
            curFileFullPath = curMusicList[index].FullName;
        }

        private string PrintMusicName(int index)
        {
            curMediaFileName = curMusicList[index].Name;
            curFileFullPath = curMusicList[index].FullName;

            string printFolderText = curMediaFileName;
            const int limitLength = 15;
            if (printFolderText.Length > limitLength)
            {
                printFolderText = printFolderText.Remove(limitLength, printFolderText.Length - limitLength);
                printFolderText = printFolderText + "...";
            }

            return printFolderText;
        }

        private void FindMP3Files(DirectoryInfo dirInfo, List<FileInfo> list)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = dirInfo.GetFiles("*.mp3");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    list.Add(fi);
                }

                subDirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo di in subDirs)
                {
                    FindMP3Files(di, list);
                }
            }
        }

        private void PlayMusic(string musicFile, bool loop)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                MediaControl.Close();
            }

            MediaControl.Open(musicFile);

            MediaControl.Play(loop);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Idletime--;
            TimeSpan time = TimeSpan.FromSeconds(Idletime);
            label9.Text = "남은 시간 : " + time.ToString(@"mm\:ss");
            if (Idletime == 0)
            {
                Idletime = Reset_idletime;
                Start_btn_enable(true);
                button16.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(1);
                label9.Text = "남은 시간: 00:00";
                timer1.Dispose();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                TimeSpan time = TimeSpan.FromMilliseconds(MediaControl.Length() - MediaControl.Position());
                label10.Text = "남은 시간 : " + string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            if (MediaControl.Status() == "stopped")
            {
                MediaControl.Close();
                Start_btn_enable(true);
                button2.Enabled = false;
                button3.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(2);
                label10.Text = "남은 시간: 00:00";
                timer2.Dispose();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                TimeSpan time = TimeSpan.FromMilliseconds(MediaControl.Length() - MediaControl.Position());
                label11.Text = "남은 시간 : " + string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            if (MediaControl.Status() == "stopped")
            {
                MediaControl.Close();
                Start_btn_enable(true);
                button4.Enabled = false;
                button5.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(3);
                label11.Text = "남은 시간: 00:00";
                timer3.Dispose();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                TimeSpan time = TimeSpan.FromMilliseconds(MediaControl.Length() - MediaControl.Position());
                label12.Text = "남은 시간 : " + string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            if (MediaControl.Status() == "stopped")
            {
                MediaControl.Close();
                Start_btn_enable(true);
                button7.Enabled = false;
                button8.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(4);
                label12.Text = "남은 시간: 00:00";
                timer4.Dispose();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                TimeSpan time = TimeSpan.FromMilliseconds(MediaControl.Length() - MediaControl.Position());
                label13.Text = "남은 시간 : " + string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            if (MediaControl.Status() == "stopped")
            {
                MediaControl.Close();
                Start_btn_enable(true);
                button10.Enabled = false;
                button11.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(5);
                label13.Text = "남은 시간: 00:00";
                timer5.Dispose();
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                TimeSpan time = TimeSpan.FromMilliseconds(MediaControl.Length() - MediaControl.Position());
                label14.Text = "남은 시간 : " + string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            if (MediaControl.Status() == "stopped")
            {
                MediaControl.Close();
                Start_btn_enable(true);
                button13.Enabled = false;
                button14.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(6);
                label14.Text = "남은 시간: 00:00";
                timer6.Dispose();
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            Idletime--;
            TimeSpan time = TimeSpan.FromSeconds(Idletime);
            label19.Text = "남은 시간 : " + time.ToString(@"mm\:ss");
            if (Idletime == 0)
            {
                Idletime = Reset_idletime;
                Start_btn_enable(true);
                button20.Enabled = false;
                comboBox1.Enabled = true;
                sendCommand("Stop_log");
                Check_complete(7);
                label19.Text = "남은 시간: 00:00";
                timer7.Dispose();
            }
        }

        private void Check_complete(int num)
        {
            if (num == 1)
                checkBox1.Checked = true;
            else if (num == 2)
                checkBox2.Checked = true;
            else if (num == 3)
                checkBox3.Checked = true;
            else if (num == 4)
                checkBox4.Checked = true;
            else if (num == 5)
                checkBox5.Checked = true;
            else if (num == 6)
                checkBox6.Checked = true;
            else if (num == 7)
                checkBox7.Checked = true;
        }

        public void Unchecked()
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
        }

        // ========================= 임상실험 ===========================
        // ======================== 실험전 측정 ==========================
        private void button17_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button16.Enabled = true;
            comboBox1.Enabled = false;
            timer1.Start();
            sendCommand("Record_log_01");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Idletime = Reset_idletime;
            Start_btn_enable(true);
            button16.Enabled = false;
            Check_complete(1);
            timer1.Dispose();
            comboBox1.Enabled = true;
            label9.Text = "남은 시간: 00:00";
            sendCommand("Stop_log");
        }
        // ======================== 실험후 측정 ==========================
        private void button19_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button20.Enabled = true;
            comboBox1.Enabled = false;
            timer7.Start();
            sendCommand("Record_log_07");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Idletime = Reset_idletime;
            Start_btn_enable(true);
            button20.Enabled = false;
            Check_complete(7);
            timer7.Dispose();
            comboBox1.Enabled = true;
            label19.Text = "남은 시간: 00:00";
            sendCommand("Stop_log");
        }
        // =============================================================
        // ========================= 1번 항목 ===========================
        private void button1_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button2.Enabled = true;
            button3.Enabled = true;
            comboBox1.Enabled = false;
            timer2.Start();

            setMusicList(0);
            if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
            else
            {
                if (curIndex >= 0)
                {
                    PlayMusic(curFileFullPath, false);
                }
                else
                {
                    MediaControl.Close();
                }
            }
            sendCommand("Record_log_02");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            MediaControl.Pause();
            sendCommand("Stop_log");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
            label10.Text = "남은 시간 : 00:00";
            sendCommand("Stop_log");
        }
        // ========================= 2번 항목 ===========================
        private void button6_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button4.Enabled = true;
            button5.Enabled = true;
            comboBox1.Enabled = false;
            timer3.Start();

            setMusicList(1);
            if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
            else
            {
                if (curIndex >= 0)
                {
                    PlayMusic(curFileFullPath, false);
                }
                else
                {
                    MediaControl.Close();
                }
            }
            sendCommand("Record_log_03");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            button5.Enabled = false;
            MediaControl.Pause();
            sendCommand("Stop_log");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
            label11.Text = "남은 시간 : 00:00";
            sendCommand("Stop_log");
        }
        // ========================= 3번 항목 ===========================
        private void button9_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button7.Enabled = true;
            button8.Enabled = true;
            comboBox1.Enabled = false;
            timer4.Start();

            setMusicList(2);
            if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
            else
            {
                if (curIndex >= 0)
                {
                    PlayMusic(curFileFullPath, false);
                }
                else
                {
                    MediaControl.Close();
                }
            }
            sendCommand("Record_log_04");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button9.Enabled = true;
            button8.Enabled = false;
            MediaControl.Pause();
            sendCommand("Stop_log");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
            label12.Text = "남은 시간 : 00:00";
            sendCommand("Stop_log");
        }
        // ========================= 4번 항목 ===========================
        private void button12_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button10.Enabled = true;
            button11.Enabled = true;
            comboBox1.Enabled = false;
            timer5.Start();

            setMusicList(3);
            if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
            else
            {
                if (curIndex >= 0)
                {
                    PlayMusic(curFileFullPath, false);
                }
                else
                {
                    MediaControl.Close();
                }
            }
            sendCommand("Record_log_05");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            button12.Enabled = true;
            button11.Enabled = false;
            MediaControl.Pause();
            sendCommand("Stop_log");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
            label13.Text = "남은 시간 : 00:00";
            sendCommand("Stop_log");
        }

        // ========================= 5번 항목 ===========================
        private void button15_Click(object sender, EventArgs e)
        {
            Start_btn_enable(false);
            button13.Enabled = true;
            button14.Enabled = true;
            comboBox1.Enabled = false;
            timer6.Start();

            setMusicList(4);
            if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
            else
            {
                if (curIndex >= 0)
                {
                    PlayMusic(curFileFullPath, false);
                }
                else
                {
                    MediaControl.Close();
                }
            }
            sendCommand("Record_log_06");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            button15.Enabled = true;
            button14.Enabled = false;
            MediaControl.Pause();
            sendCommand("Stop_log");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
            label14.Text = "남은 시간 : 00:00";
            sendCommand("Stop_log");
        }

        // =============================================================

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Select_path("1");
                label2.Text = "1번 오디오 클립 (" + PrintMusicName(0) + ")";
                label3.Text = "2번 오디오 클립 (" + PrintMusicName(1) + ")";
                label4.Text = "3번 오디오 클립 (" + PrintMusicName(2) + ")";
                label5.Text = "4번 오디오 클립 (" + PrintMusicName(3) + ")";
                label6.Text = "5번 오디오 클립 (" + PrintMusicName(4) + ")";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Select_path("2");
                label2.Text = "1번 오디오 클립 (" + PrintMusicName(0) + ")";
                label3.Text = "2번 오디오 클립 (" + PrintMusicName(1) + ")";
                label4.Text = "3번 오디오 클립 (" + PrintMusicName(2) + ")";
                label5.Text = "4번 오디오 클립 (" + PrintMusicName(3) + ")";
                label6.Text = "5번 오디오 클립 (" + PrintMusicName(4) + ")";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                Select_path("3");
                label2.Text = "1번 오디오 클립 (" + PrintMusicName(0) + ")";
                label3.Text = "2번 오디오 클립 (" + PrintMusicName(1) + ")";
                label4.Text = "3번 오디오 클립 (" + PrintMusicName(2) + ")";
                label5.Text = "4번 오디오 클립 (" + PrintMusicName(3) + ")";
                label6.Text = "5번 오디오 클립 (" + PrintMusicName(4) + ")";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                Select_path("4");
                label2.Text = "1번 오디오 클립 (" + PrintMusicName(0) + ")";
                label3.Text = "2번 오디오 클립 (" + PrintMusicName(1) + ")";
                label4.Text = "3번 오디오 클립 (" + PrintMusicName(2) + ")";
                label5.Text = "4번 오디오 클립 (" + PrintMusicName(3) + ")";
                label6.Text = "5번 오디오 클립 (" + PrintMusicName(4) + ")";
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                Select_path("5");
                label2.Text = "1번 오디오 클립 (" + PrintMusicName(0) + ")";
                label3.Text = "2번 오디오 클립 (" + PrintMusicName(1) + ")";
                label4.Text = "3번 오디오 클립 (" + PrintMusicName(2) + ")";
                label5.Text = "4번 오디오 클립 (" + PrintMusicName(3) + ")";
                label6.Text = "5번 오디오 클립 (" + PrintMusicName(4) + ")";
            }

            Unchecked();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Unchecked();
            sendCommand("nameform");
        }
    }
}
