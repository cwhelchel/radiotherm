using System;
using System.Windows.Forms;
using System.Net.Http;

namespace RadioTherm
{
    public partial class Form1 : Form
    {

        readonly HttpClient client = new HttpClient(new HttpClientHandler());

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var t = new Thermostat();

            await t.Update();

            textBox1.AppendText(t.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = new Thermostat();
            if (mtxtTempInput.MaskFull)
            {
                var newTemp = mtxtTempInput.Text;

                if (float.TryParse(newTemp, out float result))
                    t.SetCool(result);
                else
                    textBox1.AppendText("Invalid float");
            }
            else
                textBox1.AppendText("input full value eg 76.0\r\n");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var t = new Thermostat();
            string info = await t.GetSystemInfo();

            textBox1.AppendText(info);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var t = new Thermostat();
            string info = await t.GetRawJson("/sys/watchdog");

            textBox1.AppendText(info);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var t = new Thermostat();
            if (mtxtTempInput.MaskFull)
            {
                var newTemp = mtxtTempInput.Text;

                if (float.TryParse(newTemp, out float result))
                    t.SetHeat(result);
                else
                    textBox1.AppendText("Invalid float");
            }
            else
                textBox1.AppendText("input full value eg 76.0\r\n");

        }
    }
}
