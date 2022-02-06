using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTDLP_UI
{
    public partial class Settings : Form
    {
        public static string YTDLP_Path = "";
        public Settings(Form1 main)
        {
            InitializeComponent();

            ytdlp_path.Text = YTDLP_Path;
        }

        private void ReferenceButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "実行ファイル(*.exe)|*.exe";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ytdlp_path.Text = openFileDialog1.FileName;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            YTDLP_Path = ytdlp_path.Text;
            this.Close();
        }
    }
}
