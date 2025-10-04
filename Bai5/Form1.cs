using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private string currentFile = "";

        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.Text = "Soạn Thảo Văn Bản";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // --- Nạp font ---
            var fontAuto = new AutoCompleteStringCollection();
            toolStripComboBox1.Items.Clear();
            foreach (FontFamily font in new InstalledFontCollection().Families)
            {
                toolStripComboBox1.Items.Add(font.Name);
                fontAuto.Add(font.Name);
            }
            toolStripComboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            toolStripComboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            toolStripComboBox1.AutoCompleteCustomSource = fontAuto;

            // --- Nạp size ---
            List<int> listSize = new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            toolStripComboBox2.Items.Clear();
            foreach (var s in listSize)
            {
                toolStripComboBox2.Items.Add(s.ToString());
            }

            toolStripComboBox1.Text = "Tahoma";
            toolStripComboBox2.Text = "14";
        }

        // ---------------- FILE ----------------
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.MdiParent = this;
            f3.Show();
            currentFile = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3;

            // Nếu chưa có child nào => tạo mới
            if (this.ActiveMdiChild is Form3)
            {
                f3 = (Form3)this.ActiveMdiChild;
            }
            else
            {
                f3 = new Form3();
                f3.MdiParent = this;
                f3.Show();
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rich Text Format|*.rtf|Text Files|*.txt|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(ofd.FileName);
                if (ext.ToLower() == ".rtf")
                    f3.Editor.LoadFile(ofd.FileName, RichTextBoxStreamType.RichText);
                else
                    f3.Editor.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);

                currentFile = ofd.FileName;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3)
            {
                if (string.IsNullOrEmpty(currentFile))
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Rich Text Format|*.rtf|Text Files|*.txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string ext = Path.GetExtension(sfd.FileName);
                        if (ext.ToLower() == ".rtf")
                            f3.Editor.SaveFile(sfd.FileName, RichTextBoxStreamType.RichText);
                        else
                            f3.Editor.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);

                        currentFile = sfd.FileName;
                        MessageBox.Show("Lưu thành công!", "Thông báo");
                    }
                }
                else
                {
                    string ext = Path.GetExtension(currentFile);
                    if (ext.ToLower() == ".rtf")
                        f3.Editor.SaveFile(currentFile, RichTextBoxStreamType.RichText);
                    else
                        f3.Editor.SaveFile(currentFile, RichTextBoxStreamType.PlainText);

                    MessageBox.Show("Lưu thành công!", "Thông báo");
                }
            }
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3)
            {
                FontDialog fontDlg = new FontDialog();
                fontDlg.ShowColor = true;
                fontDlg.ShowEffects = true;

                if (fontDlg.ShowDialog() == DialogResult.OK)
                {
                    f3.Editor.SelectionFont = fontDlg.Font;
                    f3.Editor.SelectionColor = fontDlg.Color;
                }
            }
            else
            {
                MessageBox.Show("Hãy mở hoặc tạo văn bản trước!", "Thông báo");
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3)
            {
                string fontName = toolStripComboBox1.SelectedItem?.ToString() ?? "Tahoma";
                float fontSize = f3.Editor.SelectionFont?.Size ?? 14;
                f3.Editor.SelectionFont = new Font(fontName, fontSize);
            }
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3)
            {
                if (float.TryParse(toolStripComboBox2.SelectedItem?.ToString(), out float fontSize))
                {
                    string fontName = f3.Editor.SelectionFont?.FontFamily.Name ?? "Tahoma";
                    f3.Editor.SelectionFont = new Font(fontName, fontSize);
                }
            }
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3 && f3.Editor.SelectionFont != null)
            {
                Font currentFont = f3.Editor.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Bold;
                f3.Editor.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3 && f3.Editor.SelectionFont != null)
            {
                Font currentFont = f3.Editor.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Italic;
                f3.Editor.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is Form3 f3 && f3.Editor.SelectionFont != null)
            {
                Font currentFont = f3.Editor.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Underline;
                f3.Editor.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click( sender, e);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click( sender, e);
        }
    }
}
