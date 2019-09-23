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

        bool edytowane = false, czyOdPoczatkuPliku = true;

        public bool potwierdzZapis()
        {
            if (!edytowane) return true;
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
                        this.Text = "Easy Notepad";
                        return true;
                    }
                    else return false;
                }
                else if (result == DialogResult.No)
                {
                    edytowane = false;
                    this.Text = "Easy Notepad";
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void zakonczToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void otworzToolStripMenuItem_Click(object sender, EventArgs e)
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
                edytowane = false;
                this.Text = "Easy Notepad";
            }
        }

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(potwierdzZapis()) richTextBox1.Text = "";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            edytowane = true;
            this.Text = "* Easy Notepad";
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor: Gracjan Mika ( https://www.gmika.pl )","O Programie",MessageBoxButtons.OK);
        }

        private void wyszukajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool czyPoprawne = false;
            Form3 testDialog = new Form3();
            testDialog.checkBox1.Checked = czyOdPoczatkuPliku;
            if(richTextBox1.SelectedText.Length > 0)
            {
                testDialog.textBox1.Text = richTextBox1.SelectedText;
                testDialog.textBox1.SelectAll();
            }

            while (!czyPoprawne)
            {
                if (testDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string wyszukaj = testDialog.textBox1.Text;
                    czyOdPoczatkuPliku = testDialog.checkBox1.Checked;

                    if (wyszukaj.Length < 1) testDialog.label2.Text = "Wartość szukana nie może być pusta";
                    else
                    {
                        int poz = richTextBox1.Text.IndexOf(wyszukaj, (czyOdPoczatkuPliku ? 0 : (richTextBox1.SelectionStart + richTextBox1.SelectionLength)));
                        if (poz > -1)
                        {
                            richTextBox1.Select(poz,wyszukaj.Length);
                            czyPoprawne = true;
                        }
                        else testDialog.label2.Text = "Nie znaleziono szukanego wyrażenia";
                    }
                }
                else czyPoprawne = true;
            }

            testDialog.Dispose();
        }

        private void zamienToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool czyPoprawne = false;
            Form2 testDialog = new Form2();
            if (richTextBox1.SelectedText.Length > 0)
            {
                testDialog.textBox1.Text = richTextBox1.SelectedText;
                testDialog.textBox1.SelectAll();
            }

            while (!czyPoprawne)
            {
                if (testDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string zamien = testDialog.textBox1.Text;
                    string na = testDialog.textBox2.Text;
                    if (zamien.Length < 1 || na.Length < 1) testDialog.label3.Text = "Wartości nie mogą być puste";
                    else
                    {
                        int poz = richTextBox1.Text.IndexOf(zamien);
                        if (poz > -1)
                        {
                            while (poz > -1)
                            {
                                richTextBox1.Select(poz, zamien.Length);
                                richTextBox1.SelectedText = na;
                                poz = richTextBox1.Text.IndexOf(zamien);
                            }
                            testDialog.label3.Text = "Znaleziono podane wyrażenia i zamieniono";
                        }
                        else testDialog.label3.Text = "Nie znaleziono podanego wyrażenia";
                    }
                }
                else czyPoprawne = true;
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

        void DeleteAction(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        void SelectAllAction(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
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

        private void ponowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoAction(sender, e);
        }

        private void edycjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cofnijToolStripMenuItem.Enabled = richTextBox1.CanUndo;
            ponowToolStripMenuItem.Enabled = richTextBox1.CanRedo;

            if (richTextBox1.Text.Trim().Length > 0)
            {
                zamienToolStripMenuItem1.Enabled = true;
                wyszukajToolStripMenuItem.Enabled = true;
            }
            else
            {
                zamienToolStripMenuItem1.Enabled = false;
                wyszukajToolStripMenuItem.Enabled = false;
            }
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

            ToolStripMenuItem menuItemDelete = new ToolStripMenuItem("Usuń");
            menuItemDelete.Click += new EventHandler(DeleteAction);
            contextMenuStrip.Items.Add(menuItemDelete);

            contextMenuStrip.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem menuItemSelectAll = new ToolStripMenuItem("Zaznacz wszystko");
            menuItemSelectAll.Click += new EventHandler(SelectAllAction);
            contextMenuStrip.Items.Add(menuItemSelectAll);

            contextMenuStrip.Opening += (s, ev) =>
            {
                menuItemUndo.Enabled = richTextBox1.CanUndo;
                menuItemRedo.Enabled = richTextBox1.CanRedo;

                if (richTextBox1.SelectedText.Length > 0)
                {
                    menuItemDelete.Enabled = true;
                    menuItemCut.Enabled = true;
                    menuItemCopy.Enabled = true;
                }
                else
                {
                    menuItemDelete.Enabled = false;
                    menuItemCut.Enabled = false;
                    menuItemCopy.Enabled = false;
                }

                if (Clipboard.ContainsText()) menuItemPaste.Enabled = true;
                else menuItemPaste.Enabled = false;

                if (richTextBox1.Text.Length > 0) menuItemSelectAll.Enabled = true;
                else menuItemSelectAll.Enabled = false;
            };

            richTextBox1.ContextMenuStrip = contextMenuStrip;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !potwierdzZapis();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz otworzyć w przeglądarce podany link: "+e.LinkText, "Otwieranie linku", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
        }
    }
}