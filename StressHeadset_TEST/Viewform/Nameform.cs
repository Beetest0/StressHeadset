using System;
using System.Windows.Forms;

namespace StressHeadset_TEST.Viewform
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
            if (label17.Text.Equals("헤드셋 연결 상태 : 연결 됨"))
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("성함을 입력해주세요.");
                }
                else
                {
                    Log_message.DirCreate(textBox1.Text);
                    Log_message.LogWrite();
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
