using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Management;

namespace StressHeadset_TEST_UART
{
    public partial class Form1 : Form
    {
        WATUTF8 watUTF8 = new WATUTF8();

        public delegate void MethodInvoker();

        Viewform.Mainform mainform;
        Viewform.Nameform nameform;

        HIDDevice.interfaceDetails[] devices;
        HIDDevice device;

        private Boolean isAddedCalled = false;
        private Boolean isRemovedCalled = false;

        static bool logstate;

        string RecordFileName = "";

        Stream FS = null;
        StreamWriter objSaveFile = null;

        static string devicename = "VoicePro 575";

        // ============================ Log ===========================
        static int name_number = 0;
        static string DestinationDir;
        //============================================================

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //==========================================================
            mainform = new Viewform.Mainform();
            mainform.TopLevel = false;
            mainform.Dock = DockStyle.Fill;
            mainform.sendCommand += main_sendCommand;
            this.Controls.Add(mainform);
            //==========================================================
            nameform = new Viewform.Nameform();
            nameform.TopLevel = false;
            nameform.Dock = DockStyle.Fill;
            nameform.sendCommand += main_sendCommand;
            this.Controls.Add(nameform);
            //==========================================================

            UsbNotification.RegisterUsbDeviceNotification(this.Handle, "");

            Updategui(false);
            logstate = false;
            Usb_DeviceAdded();

            main_sendCommand("nameform");
            //main_sendCommand("mainform"); // 테스트 시 사용하는 페이지

            Get_com_port();
        }

        private void main_sendCommand(string text)
        {
            if (text.Equals("mainform"))
            {
                mainform.Visible = true;
                nameform.Visible = false;
                OpenComm();
                mainform.cbPortName.Text = nameform.cbPortName.Text;
            }
            if (text.Equals("nameform"))
            {
                mainform.Visible = false;
                nameform.Visible = true;
                CloseComm();
            }
            if (text.Contains("logstate_true"))
            {
                logstate = true;
            }
            if (text.Equals("logstate_false"))
            {
                logstate = false;
            }
            if (text.Equals("mainform_uncheck"))
            {
                mainform.Unchecked();
            }
            if (text.Equals("Create_Dir"))
            {
                DirCreate(nameform.textBox1.Text);
            }
            if (text.Equals("Record_log_01"))
            {
                Record_log("01");
            }
            if (text.Equals("Record_log_02"))
            {
                Record_log("02");
            }
            if (text.Equals("Record_log_03"))
            {
                Record_log("03");
            }
            if (text.Equals("Record_log_04"))
            {
                Record_log("04");
            }
            if (text.Equals("Record_log_05"))
            {
                Record_log("05");
            }
            if (text.Equals("Record_log_06"))
            {
                Record_log("06");
            }
            if (text.Equals("Record_log_07"))
            {
                Record_log("07");
            }
            if (text.Equals("Stop_log"))
            {
                Stop_log();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == UsbNotification.WmDevicechange)
            {
                switch ((int)m.WParam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        if (!isRemovedCalled)
                        {
                            Usb_DeviceRemoved();

                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                Thread.Sleep(500);
                                isRemovedCalled = false;

                            }).Start();
                        }

                        break;

                    case UsbNotification.DbtDevicearrival:
                        if (!isAddedCalled)
                        {
                            isAddedCalled = true;
                            Thread.Sleep(500);
                            Usb_DeviceAdded();

                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                Thread.Sleep(500);
                                isAddedCalled = false;

                            }).Start();
                        }
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void Usb_DeviceAdded()
        {
            devices = HIDDevice.getConnectedDevices();
            if (devices.Length > 0)
            {
                for (int i = 0; i <= devices.Length - 1; i++)
                {
                    if (devices[i].product.Equals(devicename))
                    {
                        //device = new HIDDevice(devices[i].devicePath, true);
                        //device.dataReceived += new HIDDevice.dataReceivedEvent(device_dataReceived);

                        Updategui(true);
                    }
                }
            }
            else
            {
                Updategui(false);
            }

            nameform.cbPortName.Items.Clear();
            MethodInvoker gdi = new MethodInvoker(Get_com_port); IAsyncResult result = gdi.BeginInvoke(null, "");
        }

        private void Usb_DeviceRemoved()
        {
            devices = HIDDevice.getConnectedDevices();
            Console.WriteLine(devices);
            bool usb = false;
            if (devices.Length > 0)
            {
                for (int i = 0; i <= devices.Length - 1; i++)
                {
                    if (devices[i].product.Equals(devicename))
                    {
                        usb = true;
                    }
                }
            }

            nameform.cbPortName.Text = "";
            nameform.cbPortName.Items.Clear();
            MethodInvoker gdi = new MethodInvoker(Get_com_port); IAsyncResult result = gdi.BeginInvoke(null, "");

            if (String.IsNullOrWhiteSpace(nameform.cbPortName.Text))
            {
                MediaControl.Stop();
                //Updategui(false);
                main_sendCommand("nameform");
            }

            if (usb == false)
            {
                MediaControl.Stop();
                Updategui(false);
                main_sendCommand("nameform");
            }
        }

        private void Get_com_port()
        {
            using (var searcher = new ManagementObjectSearcher ("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                if (portnames.Length > 0)
                {
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                    var tList = (from n in portnames
                                 join p in ports on n equals p["DeviceID"].ToString()
                                 select n + " - " + p["Caption"]).ToList();

                    for (int i = 0; i <= ports.Count - 1; i++)
                    {
                        if (tList[i].Contains("CP210x"))
                        {
                            Invoke((MethodInvoker)delegate ()
                            {
                                nameform.cbPortName.Text = portnames[i];
                            });
                        }
                    }
                }
            }
        }

        private void Updategui(bool state)
        {
            if (state == true)
            {
                nameform.label17.Text = "헤드셋 연결 상태 : 연결 됨";
                nameform.label17.ForeColor = Color.Red;
                mainform.label17.Text = "헤드셋 연결 상태 : 연결 됨";
                mainform.label17.ForeColor = Color.Red;
            }
            else
            {
                nameform.label17.Text = "헤드셋 연결 상태 : 연결 안됨";
                nameform.label17.ForeColor = Color.Black;
                mainform.label17.Text = "헤드셋 연결 상태 : 연결 안됨";
                mainform.label17.ForeColor = Color.Black;
            }
        }

        private void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (logstate == true)
            {
                try
                {
                    int iRecSize = sp1.BytesToRead; // 수신된 데이터 갯수

                    if (iRecSize != 0) // 수신된 데이터의 수가 0이 아닐때만 처리하자
                    {
                        byte[] buff = new byte[iRecSize];
                        try
                        {
                            sp1.Read(buff, 0, iRecSize);

                            string strTemp = this.watUTF8.AddBytes(buff.ToList()).Replace("\r", "").Replace("\n", Environment.NewLine);
                            if (objSaveFile != null)
                                objSaveFile.Write(strTemp);
                        }
                        catch { }
                    }
                }
                catch (System.Exception)
                {
                }
            }
        }

        private void OpenComm()
        {
            try
            {
                if (!sp1.IsOpen)
                {
                    sp1.PortName = nameform.cbPortName.Text;
                    sp1.Open();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseComm()
        {
            if (null != sp1)
            {
                if (sp1.IsOpen)
                {
                    sp1.Close();
                }
            }
        }

        private static void DirCreate(string name)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Log");
            DirectoryInfo destinatinoDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + " " + name);
            if (!dir.Exists)
            {
                dir.Create();
            }
            while (destinatinoDir.Exists)
            {
                name_number++;
                destinatinoDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + " " + name + name_number);
            }

            try
            {
                destinatinoDir.Create();
                DestinationDir = destinatinoDir.ToString();
            }
            catch (System.IO.IOException ex)
            {

            }
            Console.WriteLine(DestinationDir);
        }

        private void Record_log(string num)
        {
            logstate = true;
            try
            {
                RecordFileName = DestinationDir + "\\" + num + ".txt";
                FS = new FileStream(RecordFileName, FileMode.Create, FileAccess.Write);
                objSaveFile = new System.IO.StreamWriter(FS, System.Text.Encoding.Default);

                //objSaveFile.WriteLine("start: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffffff" + "\n")); //저장 시작시간 기록
            }
            catch { }
        }

        private void Stop_log()
        {
            try
            {
                //objSaveFile.WriteLine("\n" + "end: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffffff")); //저장 종료시간 기록

                objSaveFile.Close();
                objSaveFile.Dispose();
            }
            catch { }
            logstate = false;

            //var lineCount = File.ReadLines(RecordFileName).Count(); // 총 text file의 줄 수 파악 시 사용
            //Console.WriteLine("Total Text line count is : " + lineCount);
        }
    }
}
