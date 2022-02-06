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
using System.Text.RegularExpressions;

namespace YTDLP_UI
{
    public partial class Form1 : Form
    {
        
        public ProcessStartInfo processStartInfo = new ProcessStartInfo();
        public Form1()
        {
            InitializeComponent();
        }
        private void SetProcessInfo()
        {
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.FileName = Settings.YTDLP_Path;
        }

        private void ヘルプToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Help(this).Show();
        }
        private void ytdlpバージョンToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processStartInfo.Arguments = "--version";
            Process process = Process.Start(processStartInfo);
            MessageBox.Show("バージョン: " + process.StandardOutput.ReadToEnd());
        }

        private void 環境設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ReferenceeButton_Click(object sender, EventArgs e)
        {

        }

        List<string[]> formatDatas = new List<string[]>();
        int[] MatchPos = new int[10];
        bool Set = false;
        Regex splitRegex = new Regex(@"[\s|]+");
        private void ImportButton_Click(object sender, EventArgs e)
        {
            Set = false;
            listView1.Items.Clear();
            listView2.Items.Clear();
            formatDatas.Clear();
            processStartInfo.Arguments = URLText.Text + " --list-formats";
            Process process = Process.Start(processStartInfo);
            int line = -1;
            while (!process.StandardOutput.EndOfStream)
            {
                string data = process.StandardOutput.ReadLine();
                if (!Set)
                {
                    for (int i = 0; i < MatchPos.Length; i++)
                    {
                        string Name = Enum.GetName(typeof(FormatDataType), i);
                        if (data.Contains(Name))
                        {
                            Set = true;
                            switch(i)
                            {
                                //前から一致
                                case 0:
                                case 1:
                                case 2:
                                case 6:
                                case 7:
                                case 9:
                                    MatchPos[i] = data.IndexOf(Name);
                                    break;

                                //後ろから一致
                                default:
                                    MatchPos[i] = data.IndexOf(Name) + Name.Length;
                                    break;
                            }
                            
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if(data.Contains("-----------------------"))
                { }
                else
                {
                    ListViewItem listViewItem = new ListViewItem();
                    string[] Fdata = new string[MatchPos.Length];
                    for (int i = 0; i < MatchPos.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 6:
                            case 7:
                            case 9:
                                Fdata[i] = data.Substring(MatchPos[i]);
                                Fdata[i] = splitRegex.Split(Fdata[i]).First();
                                break;

                            default:
                                Fdata[i] = data.Substring(0, MatchPos[i]);
                                Fdata[i] = splitRegex.Split(Fdata[i]).Last();
                                break;
                        }

                        //video only と audio only のセット
                        if(Fdata[i] == "video")
                        {
                            Fdata[i] = "video only";
                        }else if (Fdata[i] == "audio")
                        {
                            Fdata[i] = "audio only";
                        }
                        //リストビューサブに追加
                        listViewItem.SubItems.Add(Fdata[i]);
                    }
                    formatDatas.Add(Fdata);
                    //リストビューに追加
                    listView1.Items.Add(listViewItem);
                }
                line++;
            }
        }

        private void ytdlpをupgradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processStartInfo.Arguments = "--update";
            Process process = Process.Start(processStartInfo);
            MessageBox.Show(process.StandardOutput.ReadToEnd());
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            listView2.Items.Add((ListViewItem)listView1.FocusedItem.Clone());
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            listView2.Items.Remove(listView2.SelectedItems[0]);
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            string Arguments = "-f ";
            for(int i = 0; i < listView2.Items.Count; i++)
            {
                Arguments += listView2.Items[i].SubItems[1].Text;

                if(i != listView2.Items.Count - 1)
                {
                    Arguments += "+";
                }
            }
            Arguments += " --merge-output-format " + comboBox1.Text + " " + URLText.Text;
            processStartInfo.Arguments = Arguments;

            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.UseShellExecute = true;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
            SetProcessInfo();
            MessageBox.Show("ダウンロードが完了しました");
        }

        private void 使い方ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HowTo().ShowDialog();
        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 環境設定ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new Settings(this).ShowDialog();
        }


        //データのロードと復元
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.YTDLP__Path = Settings.YTDLP_Path;
            Properties.Settings.Default.Save();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Settings.YTDLP_Path = Properties.Settings.Default.YTDLP__Path;

            SetProcessInfo();
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Sekiat/YTDLP_UI");
        }
    }

    enum FormatDataType
    {
        ID,
        EXT,
        RESOLUTION,
        FPS,
        FILESIZE,
        TBR,
        PROTO,
        VCODEC,
        VBR,
        ACODEC
    }
}
