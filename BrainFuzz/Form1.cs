using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;
using BrainFuzzInterpreter;
using System.Runtime.InteropServices;

namespace BrainFuzz
{
    public partial class Form1 : Form
    {
        bool recol = false;
        bool progSelChange = false;
        bool undoBcol = false;
        bool saved = true;
        string fileName = "";
        string inputFileName = "";
        string outFileName = "";
        string prevText = "";
        bool useExtentions = true;
        [DllImport("user32.dll")] // import lockwindow to remove flashing
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        BFInt.EofSetting setting = BFInt.EofSetting.EOF0;

        public Form1()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string[] args = (string[])e.Argument;
            try
            {
                BFInt bfi = new BFInt(args[0], args[1], setting, useExtentions);
                while (bfi.next())
                {
                    string res = bfi.getOutput();
                    if (res != "")
                    {
                        res = res.Replace("\0", "\u2400");
                        this.Invoke((MethodInvoker)delegate
                        {
                            txtOutput.Text += res; // runs on UI thread
                        });
                    }
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                txtOutput.Text = "";
                string[] vals = { txtProg.Text, txtInput.Text };
                if (inputFromFileMenuItem.Checked)
                {
                    if (inputFileName != "")
                    {
                        string[] lines = File.ReadAllLines(inputFileName);
                        string temp = "";
                        for (int i = 0; i < lines.Length; i++)
                        {
                            temp += lines[i];
                        }
                        vals[1] = temp;
                    }
                }
                backgroundWorker1.RunWorkerAsync(vals);
                toolStripStatusLabel1.Text = "Running...";
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation)
                backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                toolStripStatusLabel1.Text = "Stopped";
            }
            else if (e.Error != null)
            {
                toolStripStatusLabel1.Text = e.Error.ToString();

            }
            else
            {
                toolStripStatusLabel1.Text = "Done";

            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtProg.Text = "";
            fileName = "";
            saved = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtProg.SelectionTabs = new int[] { 25, 50, 75, 100 };
            txtProg.AutoWordSelection = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName == "")
            {
                saveFileDialog1.ShowDialog();
                fileName = saveFileDialog1.FileName;
                saved = save();
            }
            else
            {
                saved = save();
            }
        }

        private bool save()
        {
            if (fileName != "")
            {
                File.WriteAllLines(fileName, txtProg.Lines);
                return true;
            }
            return false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                txtProg.Lines = File.ReadAllLines(fileName);
                saved = true;
                prevText = txtProg.Text;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                saved = save();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("Changes have not been saved. Do you want to save now?", "more", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (fileName == "")
                    {
                        dr = saveFileDialog1.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            fileName = saveFileDialog1.FileName;
                            saved = save();
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        saved = save();
                    }
                }
                else if (dr == DialogResult.No)
                {

                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void wordwrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wordwrapToolStripMenuItem.Checked = !wordwrapToolStripMenuItem.Checked;
            txtProg.WordWrap = !txtProg.WordWrap;
        }

        private void return0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting = BFInt.EofSetting.EOF0;
            return0ToolStripMenuItem.Checked = true;
            return1ToolStripMenuItem.Checked = false;
            unchangedToolStripMenuItem.Checked = false;
        }

        private void return1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting = BFInt.EofSetting.EOFNEG1;
            return0ToolStripMenuItem.Checked = false;
            return1ToolStripMenuItem.Checked = true;
            unchangedToolStripMenuItem.Checked = false;
        }

        private void unchangedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting = BFInt.EofSetting.EOFNOCHANGE;
            return0ToolStripMenuItem.Checked = false;
            return1ToolStripMenuItem.Checked = false;
            unchangedToolStripMenuItem.Checked = true;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtProg.SelectAll();
        }

        private void clearOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtOutput.Text = "";
            toolStripStatusLabel1.Text = "Ready";
        }

        private bool continueUnsaved()
        {
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("This will clear your program. Continue?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        private void catToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "";
            if (continueUnsaved())
            {
                switch (setting)
                {
                    case BFInt.EofSetting.EOF0:
                        s = BrainFuzz.Properties.Resources.cat0;
                        break;
                    case BFInt.EofSetting.EOFNEG1:
                        s = BrainFuzz.Properties.Resources.cat1;
                        break;
                    case BFInt.EofSetting.EOFNOCHANGE:
                        s = BrainFuzz.Properties.Resources.catunchanged;
                        break;
                    default:
                        s = txtProg.Text;
                        break;
                }
                txtProg.Text = s;
            }
        }

        private void helloWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (continueUnsaved())
            {
                txtProg.Text = BrainFuzz.Properties.Resources.helloWorld;
            }
        }

        private void useBrainFuzzExtentionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useBrainFuzzExtentionsToolStripMenuItem.Checked = !useBrainFuzzExtentionsToolStripMenuItem.Checked;
            useExtentions = !useExtentions;
        }

        private void numberCatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (continueUnsaved())
            {
                txtProg.Text = Regex.Unescape(BrainFuzz.Properties.Resources.numcat);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.ShowDialog();
        }

        private void txtProg_TextChanged(object sender, EventArgs e)
        {
            if (saved)
            {
                saved = false;
            }
            if (txtProg.Text != prevText)
            {
                prevText = txtProg.Text;
                recol = true;
                int selPos = txtProg.SelectionStart;
                int selLeng = txtProg.SelectionLength;
                //if (tprogupdated)
                {
                    try
                    {
                        if (recol)
                        {
                            progSelChange = true;
                            LockWindowUpdate(txtProg.Handle);


                            txtProg.SelectAll();
                            txtProg.SelectionColor = Color.Black;
                            txtProg.SelectionBackColor = Color.White;



                            Regex quotes = new Regex(@"""(?:[^""\\]|\\.)*""");
                            //For each match from the regex, highlight the word.
                            foreach (Match keyWordMatch in quotes.Matches(txtProg.Text))
                            {

                                txtProg.Select(keyWordMatch.Index, keyWordMatch.Length);
                                txtProg.SelectionColor = Color.Red;
                            }
                            Regex comments = new Regex(@"\/\/.*");

                            //For each match from the regex, highlight the word.
                            foreach (Match keyWordMatch in comments.Matches(txtProg.Text))
                            {

                                txtProg.Select(keyWordMatch.Index, keyWordMatch.Length);
                                txtProg.SelectionColor = Color.Green;

                            }
                            txtProg.SelectionStart = selPos;
                            txtProg.Select(selPos, selLeng);
                            txtProg.SelectionColor = Color.Black;
                            recol = false;
                        }
                        progSelChange = false;

                    }
                    finally { LockWindowUpdate(IntPtr.Zero); }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void txtProg_SelectionChanged(object sender, EventArgs e)
        {

            if (bracketMatchingToolStripMenuItem.Checked)
            {

                if (!progSelChange)
                {
                    int selPos = txtProg.SelectionStart;

                    int selLeng = txtProg.SelectionLength;
                    progSelChange = true;
                    //if (selLeng == 0)
                    {
                        try
                        {
                            if (txtProg.Text.Length > 0 && selPos >= 1)
                            {

                                char c = txtProg.Text[selPos - 1];
                                if (c == ')' || c == ']')
                                {
                                    txtProg.SelectAll();
                                    txtProg.SelectionBackColor = Color.White;
                                    txtProg.Select(selPos, selLeng);
                                    txtProg.SelectionColor = Color.Black;
                                    int ind = selPos - 1;
                                    int dist = 0;
                                    for (int i = selPos - 1; i >= 0; i--)
                                    {
                                        char cur = txtProg.Text[i];

                                        if (cur == c)
                                        {
                                            dist++;
                                        }
                                        else if (cur == '(' && c == ')')
                                        {
                                            dist--;
                                        }
                                        else if (cur == '[' && c == ']')
                                        {
                                            dist--;
                                        }
                                        if (dist == 0)
                                        {
                                            ind = i;
                                            break;
                                        }
                                    }
                                    if (ind != selPos)
                                    {
                                        txtProg.Select(selPos - 1, 1);

                                        txtProg.SelectionBackColor = Color.LightCyan;
                                        txtProg.Select(ind, 1);
                                        txtProg.SelectionBackColor = Color.LightCyan;
                                        txtProg.Select(selPos, selLeng);
                                        txtProg.SelectionColor = Color.Black;
                                    }
                                    undoBcol = true;
                                }
                                else
                                {
                                    if (undoBcol)
                                    {
                                        undoBcol = false;

                                    }
                                }
                            }
                            progSelChange = false;
                        }
                        finally { LockWindowUpdate(IntPtr.Zero); }
                    }
                }
            }
        }

        private void stripCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BFInt bfi = new BFInt(txtProg.Text, "", setting, useExtentions);
            txtProg.Text = bfi.getProg();
        }

        private void bracketMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bracketMatchingToolStripMenuItem.Checked = !bracketMatchingToolStripMenuItem.Checked;
        }

        private void btnFileIn_Click(object sender, EventArgs e)
        {
            if (inputFromFileMenuItem.Checked)
            {
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    inputFileName = openFileDialog1.FileName;
                    txtInput.Text = inputFileName;
                }
            }
        }

        private void inputFromFileMenuItem_Click(object sender, EventArgs e)
        {
            inputFromFileMenuItem.Checked = !inputFromFileMenuItem.Checked;
            if (inputFromFileMenuItem.Checked)
            {
                txtInput.ReadOnly = true;
                txtInput.Text = fileName;
            }
            else
            {
                txtInput.ReadOnly = false;
                txtInput.Text = "";
            }
            
        }

        private void exportOutputMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, txtOutput.Text);
            }
        }

    }
}
