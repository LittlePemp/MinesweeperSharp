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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)//кнопка "играть"
        {
            StartGame form = new StartGame(0,0,0,0,"Player");
            form.Show();
            Hide();

    } 

        private void Button2_Click(object sender, EventArgs e) //кнопка "настройки"
        {
            Set form = new Set();
            form.Show();
            Hide();
        }

        private void Button3_Click(object sender, EventArgs e) //кнопка "правила"
        {
            Rules form = new Rules();
            form.Show();
            Hide();
        }

        private void Button4_Click(object sender, EventArgs e)  //конпка "достижения"
        {
            Achivements form = new Achivements();
            form.Show();
            Hide();
        }

        private void Button5_Click(object sender, EventArgs e) //кнопка "выход"
        { 
            Environment.Exit(0);
        }

    }
}
