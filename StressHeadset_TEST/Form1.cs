using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace StressHeadset_TEST
{
    public partial class Form1 : Form
    {
        public delegate void MethodInvoker();

        private int pageNo = 0;

        Viewform.Mainform mainform;
        Viewform.Nameform nameform;

        HIDDevice.interfaceDetails[] devices;
        HIDDevice device;

        private Boolean isAddedCalled = false;
        private Boolean isRemovedCalled = false;

        static bool logstate;
        static string lognum;

        static string devicename = "DSU-2x";
        //static string devicename = "SEHC";

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
            //main_sendCommand("mainform");
        }

        private void main_sendCommand(string text)
        {
            if (text.Equals("mainform"))
            {
                mainform.Visible = true;
                nameform.Visible = false;
            }
            if (text.Equals("nameform"))
            {
                mainform.Visible = false;
                nameform.Visible = true;
            }
            if (text.Contains("logstate_true"))
            {
                lognum = text;
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
            Console.WriteLine(devices);
            if (devices.Length > 0)
            {
                for (int i = 0; i <= devices.Length - 1; i++)
                {
                    if (devices[i].product.Equals(devicename))
                    {
                        device = new HIDDevice(devices[i].devicePath, true);
                        device.dataReceived += new HIDDevice.dataReceivedEvent(device_dataReceived);

                        Updategui(true);
                    }
                }
            }
            else
            {
                Updategui(false);
            }
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
                if (usb == false)
                {
                    MediaControl.Stop();
                    Updategui(false);
                    main_sendCommand("nameform");
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

        private void device_dataReceived(byte[] message)
        {
            if (logstate == true && lognum.Equals("logstate_true_1"))
                Log_message.Add_log("01", message[7] + " " + message[8] + " " + message[9] + " " + message[10] + " " + message[11] + " " + message[12]);

            if (logstate == true && lognum.Equals("logstate_true_2"))
                Log_message.Add_log("02", message[7] + " " + message[8] + " " + message[9] + " " + message[10] + " " + message[11] + " " + message[12]);

            if (logstate == true && lognum.Equals("logstate_true_3"))
                Log_message.Add_log("03", message[7] + " " + message[8] + " " + message[9] + " " + message[10] + " " + message[11] + " " + message[12]);

            if (logstate == true && lognum.Equals("logstate_true_4"))
                Log_message.Add_log("04", message[7] + " " + message[8] + " " + message[9] + " " + message[10] + " " + message[11] + " " + message[12]);

            if (logstate == true && lognum.Equals("logstate_true_5"))
                Log_message.Add_log("05", message[7] + " " + message[8] + " " + message[9] + " " + message[10] + " " + message[11] + " " + message[12]);

            if (logstate == true && lognum.Equals("logstate_true_6"))
                Log_message.Add_log("06", message[7] + " " + message[8] + " " + message[9] + " " + message[10] + " " + message[11] + " " + message[12]);
        }
     }
}
