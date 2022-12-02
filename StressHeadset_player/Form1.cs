using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StressHeadset_player
{
    public partial class Form1 : Form
    {
        string curFileFullPath;
        string curMediaFileName;

        List<FileInfo> curMusicList = new List<FileInfo>();

        int curIndex = -1;
        bool isTrackBarScroling = false;

        public Form1()
        {
            InitializeComponent();

            timer1.Enabled = true;
            trackBar2.Value = 1000;
            progressBar1.Value = 0;
        }

        public void ChangeMode()
        {
            List<FileInfo> musicItemList = new List<FileInfo>();
            musicItemList = getMusicList();

            if (musicItemList.Count > 0)
                SetMusicList(musicItemList);
        }

        public List<FileInfo> getMusicList()
        {
            return curMusicList;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Select_path("1");
        }

        private void Select_path(string num)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + num;
            dialog.SelectedPath = path;

            string printFolderText = dialog.SelectedPath;
            const int limitLength = 15;
            if (printFolderText.Length > limitLength)
            {
                printFolderText = printFolderText.Remove(0, printFolderText.Length - limitLength);
                printFolderText = "..." + printFolderText;
            }
            label1.Text = printFolderText;

            DirectoryInfo di = new DirectoryInfo(dialog.SelectedPath);
            List<FileInfo> musicItemList = new List<FileInfo>();
            FindMP3Files(di, musicItemList);
            SetMusicList(musicItemList);

            setMusicList(musicItemList);
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

            PrintMusicName();
        }
        public void setMusicList(List<FileInfo> musicList)
        {
            curMusicList.Clear();
            listBox1.Items.Clear();

            foreach (FileInfo item in musicList)
            {
                curMusicList.Add(item);
                listBox1.Items.Add(item);
            }
        }

        private void PrintMusicName()
        {
            string printFolderText = curMediaFileName;
            const int limitLength = 22;
            if (printFolderText.Length > limitLength)
            {
                printFolderText = printFolderText.Remove(limitLength, printFolderText.Length - limitLength);
                printFolderText = printFolderText + "...";
            }

            label4.Text = printFolderText;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Select_path("2");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Select_path("3");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Select_path("4");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Select_path("5");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != curIndex)
                MediaControl.Close();

            if (curIndex >= 0)
            {
                PlayMusic(curFileFullPath, true);
            }
            else
            {
                MediaControl.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing")
            {
                MediaControl.Pause();
            }
            else if (MediaControl.Status() == "paused")
            {
                MediaControl.Resume();
            }
        }

        private void PlayMusic(string musicFile, bool loop)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                MediaControl.Close();
            }

            MediaControl.Open(musicFile);
            trackBar1.Maximum = MediaControl.Length();

            MediaControl.Play(loop);

            PrintMusicName();
        }

        private void playMusic(string musicFile, bool loop, int seekTime)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                MediaControl.Close();
            }

            MediaControl.Open(musicFile);
            trackBar1.Maximum = MediaControl.Length();

            MediaControl.Play(loop, seekTime);

            PrintMusicName();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MediaControl.Stop();
            label6.Text = "00:00";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            curIndex -= 1;

            if (curIndex <= 0)
            {
                curIndex = 0;
            }

            curMediaFileName = curMusicList[curIndex].Name;
            curFileFullPath = curMusicList[curIndex].FullName;

            MediaControl.Close();
            PlayMusic(curFileFullPath, true);
            //PlayMusic(curMediaFileName, true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            curIndex += 1;

            if (curIndex >= curMusicList.Count - 1)
            {
                curIndex = curMusicList.Count - 1;
            }

            curMediaFileName = curMusicList[curIndex].Name;
            curFileFullPath = curMusicList[curIndex].FullName;

            MediaControl.Close();
            PlayMusic(curFileFullPath, true);
            //PlayMusic(curMediaFileName, true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                //Console.WriteLine("length : {0} ~ {1}", trackBar1.Minimum, trackBar1.Maximum);

                if (isTrackBarScroling != true)
                    trackBar1.Value = MediaControl.Position();
                int lengthtime = MediaControl.Length() - MediaControl.Position();
                //label6.Text =  trackBar1.Value.ToString();
                TimeSpan time = TimeSpan.FromMilliseconds(MediaControl.Length() - MediaControl.Position());
                label6.Text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            else if (MediaControl.Status() == "stopped")
            {
                trackBar1.Value = 0;
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            isTrackBarScroling = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            isTrackBarScroling = false;
            playMusic(curFileFullPath, true, trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            MediaControl.MasterVolume(trackBar2.Value);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            curIndex = listBox1.SelectedIndex;

            curMediaFileName = curMusicList[curIndex].Name;
            curFileFullPath = curMusicList[curIndex].FullName;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (MediaControl.Status() == "playing" || MediaControl.Status() == "paused")
            {
                if (isTrackBarScroling != true)
                {
                }
            }
        }
    }
}
