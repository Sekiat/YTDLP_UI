using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace YTDLP_UI
{
    public partial class Help : Form
    {
        public Help(Form1 main)
        {
            InitializeComponent();
            main.processStartInfo.Arguments = "--help";
            Process process = Process.Start(main.processStartInfo);
            richTextBox1.Text = process.StandardOutput.ReadToEnd();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
