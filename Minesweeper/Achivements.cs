using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace Minesweeper
{
    public partial class Achivements : Form
    {
        public Achivements()
        {
            InitializeComponent();
            richTextBox1.AppendText("Новичок" + "\n");
            richTextBox2.AppendText("Любитель" + "\n");
            richTextBox3.AppendText("Профессионал" + "\n");
            richTextBox4.AppendText("Особый (высота, ширина, мины)" + "\n");
            useDateBase(0);
            useDateBase(1);
            useDateBase(2);
            useDateBase(3);
        }

        private void Button1_Click(object sender, EventArgs e) //кнопка "в меню"
        {
            Main form = new Main();
            form.Show();
            Close();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                textBox1.Text = "Player";

        } //Если текстбокс пустой, задается автоматичесекое имя

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '_' || e.KeyChar == (char)Keys.Back)
            { }
            else
            {
                e.Handled = true;
            }
        }  //Запрет на ввод имени русским символами

        private void button2_Click(object sender, EventArgs e)
        {
            StartGame start = new StartGame(0,0,0,0, textBox1.Text);
            start.Show();
            Close();
        } //кнопка "играть"

        private void useDateBase(int lvl)
        {
            int i = 1;
            string query = null;
            string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.mdb;";
            OleDbConnection dbConnection = new OleDbConnection(connectString);
            dbConnection.Open();
            if (lvl == 0)
                query = "SELECT playerName, time FROM 0 ORDER BY time";
            else if (lvl == 1)
                query = "SELECT playerName, time FROM 1 ORDER BY time";
            else if (lvl == 2)
                query = "SELECT playerName, time FROM 2 ORDER BY time";
            if (query != null)
            {
                OleDbCommand command = new OleDbCommand(query, dbConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    (lvl == 0 ? richTextBox1 : lvl == 1 ? richTextBox2 : richTextBox3).AppendText(i + ". " + reader[0].ToString() + ": " + reader[1].ToString() + " сек." + "\n");
                    i++;
                }
                reader.Close();
            }
            //для пользовательского режима
            if (lvl == 3)
            {
                query = "SELECT playerName, time, x, y, mines FROM 3 ORDER BY time";
                OleDbCommand command = new OleDbCommand(query, dbConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    richTextBox4.AppendText(i + ". " + reader[0].ToString() + ": " + reader[1].ToString() + " сек. (" + reader[2].ToString() + ", " + reader[3].ToString() + ", " + reader[4].ToString() + ")" + "\n");
                    i++;
                }
                reader.Close();
            }
            dbConnection.Close();
        } //вывод данных из бд

    }
}
