using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool edytowane = false;

        public bool potwierdzZapis()
        {
            if (!edytowane)
            {
                edytowane = false;
                return true;
            }
            else
            {
                DialogResult result = MessageBox.Show("Niezapisane zmiany zostaną usunięte. Zapisać plik?", "Ostrzeżenie", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveFileDialog x = new SaveFileDialog();
                    x.Title = "Zapisz plik";
                    x.Filter = "Pliki tekstowe|*.txt";
                    x.FilterIndex = 1;
                    x.DefaultExt = "txt";
                    if (x.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter w = new StreamWriter(x.FileName);
                        w.Write(richTextBox1.Text);
                        w.Close();
                        edytowane = false;
                        return true;
                    }
                    else return false;
                }
                else if (result == DialogResult.No)
                {
                    edytowane = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (potwierdzZapis())
            {
                OpenFileDialog x = new OpenFileDialog();
                x.Title = "Otwórz plik";
                x.Filter = "Pliki tekstowe|*.txt|Wszystkie|*.*";
                x.FilterIndex = 1;
                if (x.ShowDialog() == DialogResult.OK)
                {
                    StreamReader r = new StreamReader(x.FileName);
                    richTextBox1.Text = r.ReadToEnd();
                    r.Close();
                }
            }
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog x = new SaveFileDialog();
            x.Title = "Zapisz plik";
            x.Filter = "Pliki tekstowe|*.txt|Wszystkie pliki|*.*";
            x.FilterIndex = 1;
            x.DefaultExt = "txt";
            if (x.ShowDialog() == DialogResult.OK)
            {
                StreamWriter w = new StreamWriter(x.FileName);
                w.Write(richTextBox1.Text);
                w.Close();
            }
        }

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(potwierdzZapis()) richTextBox1.Text = "";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            edytowane = true;
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor: Gracjan Mika ( https://www.gmika.pl )","O Programie",MessageBoxButtons.OK);
        }

        private void wyszukajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 testDialog = new Form3();

            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                string wyszukaj = testDialog.textBox1.Text;
                int poz = -1, pocz = 0;
                string pozycje = "";
                do
                {
                    if (pocz == 0)
                    {
                        poz = richTextBox1.Text.IndexOf(wyszukaj);
                        if (poz > -1) pozycje = poz.ToString();
                        pocz = poz + 1;
                    }
                    else
                    {
                        if (pocz >= richTextBox1.Text.Length) poz = -1;
                        else
                        {
                            poz = richTextBox1.Text.IndexOf(wyszukaj, pocz);
                            if (poz > -1)
                            {
                                pozycje += ", " + poz.ToString();
                                pocz = poz + 1;
                            }
                        }
                    }
                } while (poz != -1);
                if (pozycje.Length > 0) MessageBox.Show("Znaleziono wyrażenie \"" + wyszukaj + "\" na pozycjach: " + pozycje);
            }

            testDialog.Dispose();
        }

        private void zamieńToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 testDialog = new Form2();

            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                string zamien = testDialog.textBox1.Text;
                string na = testDialog.textBox2.Text;
                richTextBox1.Text = richTextBox1.Text.Replace(zamien, na);
            }

            testDialog.Dispose();
        }

        private void zawijajWierszeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (zawijajWierszeToolStripMenuItem1.Checked) richTextBox1.WordWrap = true;
            else richTextBox1.WordWrap = false;
        }
        
        void CutAction(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }
        
        void CopyAction(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }

        void PasteAction(object sender, EventArgs e)
        {
            richTextBox1.Paste(DataFormats.GetFormat("Text"));
        }

        void UndoAction(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo) richTextBox1.Undo();
        }

        void RedoAction(object sender, EventArgs e)
        {
            if (richTextBox1.CanRedo) richTextBox1.Redo();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteAction(sender, e);
                e.Handled = true;
            }

            if (e.Control && e.Shift && e.KeyCode == Keys.Z)
            {
                RedoAction(sender, e);
                e.Handled = true;
            }
        }

        private void cofnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoAction(sender, e);
        }

        private void ponówToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoAction(sender, e);
        }

        private void edycjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cofnijToolStripMenuItem.Enabled = richTextBox1.CanUndo;
            ponówToolStripMenuItem.Enabled = richTextBox1.CanRedo;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            contextMenuStrip.ShowImageMargin = false;

            ToolStripMenuItem menuItemUndo = new ToolStripMenuItem("Cofnij");
            menuItemUndo.Click += new EventHandler(UndoAction);
            contextMenuStrip.Items.Add(menuItemUndo);

            ToolStripMenuItem menuItemRedo = new ToolStripMenuItem("Ponów");
            menuItemRedo.Click += new EventHandler(RedoAction);
            contextMenuStrip.Items.Add(menuItemRedo);

            contextMenuStrip.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem menuItemCopy = new ToolStripMenuItem("Kopiuj");
            menuItemCopy.Click += new EventHandler(CopyAction);
            contextMenuStrip.Items.Add(menuItemCopy);

            ToolStripMenuItem menuItemCut = new ToolStripMenuItem("Wytnij");
            if (richTextBox1.SelectedText.Length > 0) menuItemCut.Enabled = false;
            menuItemCut.Click += new EventHandler(CutAction);
            contextMenuStrip.Items.Add(menuItemCut);

            ToolStripMenuItem menuItemPaste = new ToolStripMenuItem("Wklej");
            menuItemPaste.Click += new EventHandler(PasteAction);
            contextMenuStrip.Items.Add(menuItemPaste);

            contextMenuStrip.Opening += (s, ev) =>
            {
                menuItemUndo.Enabled = richTextBox1.CanUndo;
                menuItemRedo.Enabled = richTextBox1.CanRedo;

                if (richTextBox1.SelectedText.Length > 0)
                {
                    menuItemCut.Enabled = true;
                    menuItemCopy.Enabled = true;
                }
                else
                {
                    menuItemCut.Enabled = false;
                    menuItemCopy.Enabled = false;
                }

                if (Clipboard.ContainsText()) menuItemPaste.Enabled = true;
                else menuItemPaste.Enabled = false;
            };

            richTextBox1.ContextMenuStrip = contextMenuStrip;
        }
    }
}