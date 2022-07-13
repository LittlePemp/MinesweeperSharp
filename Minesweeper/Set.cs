using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{

    public partial class Set : Form
    {
        int lvl, x, y, mines;

        public Set()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        public void radioButtons_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                lvl = 0;
            else if (radioButton2.Checked)
                lvl = 1;
            else if (radioButton3.Checked)
                lvl = 2;
            else if (radioButton4.Checked)
                lvl = 3;
            
        }

        private void Button1_Click(object sender, EventArgs e) //Выход в главное меню
        {
            Main form = new Main();
            form.StartPosition = FormStartPosition.Manual;
            form.Show();
            Close();
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)Keys.Back)
            { }
            else
            {
                e.Handled = true;
            }
        } //Запись только чисел

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)Keys.Back )
            { }
            else
            {
                e.Handled = true;
            }
        } //Запись только чисел

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)Keys.Back )
            { }
            else
            {
                e.Handled = true;
            }
        } //Запись только чисел

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true; lvl = 3;
            try
            {
                if (Convert.ToInt32(textBox1.Text) >= 30)
                    textBox1.Text = "30";
                x = Convert.ToInt32(textBox1.Text);
            }
            catch { textBox1.Text = "10"; }

        } //Ввод высоты не больше 50

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true; lvl = 3;
            try
            {
                if (Convert.ToInt32(textBox2.Text) >= 30)
                    textBox2.Text = "30";
                y = Convert.ToInt32(textBox2.Text);
            }
            catch { textBox2.Text = "10"; }
        } //Ввод ширины не больше 50

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true; lvl = 3;
            try
            {
                if (Convert.ToInt32(textBox1.Text) * Convert.ToInt32(textBox2.Text) < Convert.ToInt32(textBox3.Text))
                    textBox3.Text = Convert.ToString(Convert.ToInt32(textBox1.Text) * Convert.ToInt32(textBox2.Text));
                mines = Convert.ToInt32(textBox3.Text);
            }
            catch { textBox3.Text = "10"; }
        } //Ввод мин не больше 900

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBox1.Text) <= 5)
                x = 10;
            if (Convert.ToInt32(textBox2.Text) <= 5)
                y = 10;
            mines = 10;
            StartGame form = new StartGame(lvl,x,y,mines,"Player");
            form.Show();
            Close();
        }
    }
}
