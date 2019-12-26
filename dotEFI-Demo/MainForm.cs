using dotEFI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace dotEFIDemo
{
    public partial class MainForm : Form
    {
        private DotEFI efi;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                efi = DotEFI.GetInstance();

                textBox1.Text = $"{string.Join(", ", efi.GetBootOrder().Select(item => "0x" + item.ToString("X4")).ToArray())}";
                textBox2.Text = $"0x{efi.GetBootCurrent().ToString("X4")}";
                textBox3.Text = efi.GetBootInformation(efi.GetBootCurrent()).description;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
