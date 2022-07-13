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
    public partial class Rules : Form
    {
        public Rules()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Main form = new Main();
            form.Show();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartGame start = new StartGame(0, 0, 0, 0, "Player");
            start.Show();
            Close();
        }
    }
}
