using System;
using System.Windows.Forms;

namespace StressHeadset_TEST_UART.Viewform
{
    public partial class Nameform : Form
    {
        internal delegate void sendCommandDele(string text);
        internal event sendCommandDele sendCommand;

        public Nameform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label17.Text.Equals("헤드셋 연결 상태 : 연결 됨") && !String.IsNullOrWhiteSpace(cbPortName.Text))
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("성함을 입력해주세요.");
                }
                else
                {
                    sendCommand.Invoke("Create_Dir");
                    sendCommand.Invoke("mainform_uncheck");
                    sendCommand.Invoke("mainform");
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("헤드셋을 연결해주세요.");
            }
        }
    }
}
