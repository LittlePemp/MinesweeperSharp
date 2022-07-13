using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Minesweeper
{
    public partial class StartGame : Form
    {
        int flags = 0;
        int time = 0;
        int xClick, yClick, lvl;
        Button[,] buttons = new Button[xCount, yCount];

        int ySide, xSide, minesCount, openedCellsCount;
        string verdict, playerName;
        List<List<int>> neighboursField, minesField, flagsField, openedCellsField;


        static int xCount, yCount, mines, buttonSize;
        bool firstLeft = true;


        private void button1_Click(object sender, EventArgs e) //кнопка рестарта игры и выход в меню 
        {
            Close();
            if (verdict == "Lose")
            {
                StartGame newGame = new StartGame(lvl,xSide,ySide,minesCount,playerName);
                newGame.Show();
            }
            else
            {
                Main main = new Main();
                main.Show();
            }
        }


        private void button_MouseMove(object sender, MouseEventArgs e) //фокус при наведении на кнопку
        {
            (sender as Button).Focus();
        }


        private void button_MouseClick(object sender, MouseEventArgs e) //событие при нажатии на кнопку с методом вскрытия
        {
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    if (buttons[i, j].Focused)
                    {
                        xClick = i;
                        yClick = j;
                        openningMinesOrFlagCell(i, j, ((e.Button == MouseButtons.Left || firstLeft) ? 'l' : 'r'), buttons);
                        break;
                    }
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            if (time < 10)
                label2.Text = "000" + time.ToString();
            else if (time < 100)
                label2.Text = "00" + time.ToString();
            else if (time < 1000)
                label2.Text = "0" + time.ToString();
            else
                label2.Text = time.ToString();
        } //таймер

        public StartGame(int level,int x,int y, int mine, string name)
        {
            InitializeComponent();
            playerName = name;
            if (level == 0)
            {
                xCount = 9; yCount = 9; mines = 10; buttonSize = 50; lvl = 0;
            }
            else if (level == 1)
            {
                xCount = 16; yCount = 16; mines = 40; buttonSize = 35; lvl = 1;
            }
            else if (level == 2)
            {
                xCount = 16; yCount = 30; mines = 99; buttonSize = 25; lvl = 2;
            }
            else
            {
                xCount = x; yCount = y; mines = mine; buttonSize = (x > 16 || y > 16 ? 20 : (x < 10 && y < 10) ? 60 : 35) ; lvl = 3;
            }
            buttons = CreateButtons(xCount, yCount);

            //добавление шрифта  
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("3574.ttf"); // файл шрифта
            FontFamily family = fontCollection.Families[0];
            label1.Font = new Font(family, 26);
            label2.Font = new Font(family, 26);
            label1.Text = Convert.ToString(mines);

            //
            this.xSide = xCount;
            this.ySide = yCount;
            this.minesCount = mines;
            this.verdict = null;

            this.flagsField = getField(ySide, xSide);
            this.openedCellsField = getField(ySide, xSide);

            this.minesField = minesFieldGeneration(ySide, xSide, yClick, xClick, minesCount);
            this.neighboursField = getNeighboursField();

            this.openedCellsCount = 0;
            //
        }


        public Button[,] CreateButtons(int n, int m)
        {
            var buttons_1 = new Button[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Button button = new Button();
                    button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_MouseClick);
                    button.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button_MouseMove);
                    button.TabStop = false;
                    button.Size = new Size(buttonSize, buttonSize);
                    button.Location = new Point(15 + j * buttonSize, 70 + i * buttonSize);
                    button.BackgroundImage = global::Minesweeper.Properties.Resources.button;
                    button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    this.Controls.Add(button);
                    buttons_1[i, j] = button;
                }
            }
            this.Size = new Size(yCount * buttonSize + 30, xCount * buttonSize + 90);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.label1.Location = new Point(20, 25);
            this.label2.Location = new Point(yCount*buttonSize-80, 25);
            this.button1.Location = new Point((yCount * buttonSize) / 2, 25);
            return buttons_1;
        } //создание поля из кнопок


        private List<List<int>> getField(int widthField, int heightField)
        /*Метод генерации нулевого двумерного списка*/
        {
            var field = new List<List<int>>();

            for (int i = 0; i < widthField; i++)
            {
                var newFieldRow = new List<int>();
                field.Add(newFieldRow);

                int additionalValue = 0;
                for (int j = 0; j < heightField; j++)
                {
                    field[i].Add(additionalValue);
                }
            }

            return field;
        }



        private List<List<int>> minesFieldGeneration(int ySide,
                                                           int xSide,
                                                           int yClickCoord,
                                                           int xClickCoord,
                                                           int minesCount)
        /* Метод заполнения минного поля*/
        {
            var minesField = new List<List<int>>();
            minesField = getField(ySide, xSide);

            var potentialCoord = new Random();

            int yPotential, xPotential;
            int fieldFillCheck = minesCount;
            while (fieldFillCheck != 0)
            {
                yPotential = potentialCoord.Next(0, ySide);
                xPotential = potentialCoord.Next(0, xSide);

                if ((minesField[yPotential][xPotential] == 0) && !((yPotential == yClickCoord) && (xPotential == xClickCoord)))
                {
                    minesField[yPotential][xPotential] = 1;
                    fieldFillCheck--;
                }
            }

            return minesField;
        }



        private List<List<int>> getNeighboursField()
        /*Метод заполненея матрицы значениями, равными кол-ву мин вокруг*/
        {
            var field = new List<List<int>>();
            var neighboursField = new List<List<int>>();

            field = getField(xSide, ySide);
            neighboursField = getField(xSide, ySide);

            int xCurrent, yCurrent;

            // Прохождение для каждой клетки
            for (int xStateCoord = 0; xStateCoord < xSide; xStateCoord++)
            {
                for (int yStateCoord = 0; yStateCoord < ySide; yStateCoord++)
                {
                    if (minesField[yStateCoord][xStateCoord] == 0) // Проверка "Это не мина"
                    {
                        // Счет вокруг каждой клетки
                        for (int xDelta = -1; xDelta < 2; xDelta++)
                        {
                            for (int yDelta = -1; yDelta < 2; yDelta++)
                            {
                                xCurrent = xStateCoord + xDelta;
                                yCurrent = yStateCoord + yDelta;

                                if ((inField(field, yCurrent, xCurrent)) && (minesField[yCurrent][xCurrent] == 1))
                                {
                                    neighboursField[xStateCoord][yStateCoord]++;
                                }
                            }
                        }
                    }

                    else
                    {
                        continue;
                    }
                }
            }

            return neighboursField;
        }



        private static bool inField(List<List<int>> field, int yCurrent, int xCurrent)
        /*Метод для проверки нахождения координаты внутри поля*/
        {
            int ySide;
            int xSide;
            xSide = field.Count;
            ySide = field[0].Count;


            if (((xCurrent > -1) && (xCurrent < xSide)) &&
                ((yCurrent > -1) && (yCurrent < ySide)))
            {
                return true;

            }
            else
            {
                return false;
            }
        }



        public Button[,] openningMinesOrFlagCell(int xClickCoord, int yClickCoord, char clickSide, Button[,] buttons)
        /*Метод вскрытия и пометки флажком ячейки*/
        {

            if (buttons[xClickCoord, yClickCoord].Enabled == true)
            {
                if (clickSide == 'l' && flagsField[yClickCoord][xClickCoord] != 1)
                {
                    buttons[xClickCoord, yClickCoord].BackgroundImage = null;
                    buttons[xClickCoord, yClickCoord].Enabled = false;
                    if (neighboursField[xClickCoord][yClickCoord] > 0)
                    {
                        buttons[xClickCoord, yClickCoord].Text = Convert.ToString(neighboursField[xClickCoord][yClickCoord]);
                    }
                    this.openedCellsCount++;

                    firstLeft = false;

                    gameVerdict(xClickCoord, yClickCoord);

                    buttons = openningAround(xClickCoord, yClickCoord, buttons);
                }
                if (clickSide == 'r')
                {
                    if (this.flagsField[xClickCoord][yClickCoord] == 1)
                    {
                        flags--;
                        mines++;
                        label1.Text = Convert.ToString(mines);
                        buttons[xClickCoord, yClickCoord].BackgroundImage = global::Minesweeper.Properties.Resources.button;
                        this.flagsField[xClickCoord][yClickCoord] = 0;
                    }
                    else if (flags != minesCount)
                    {
                        flags++;
                        mines--;
                        label1.Text = Convert.ToString(mines);
                        buttons[xClickCoord, yClickCoord].BackgroundImage = Image.FromFile(@".\flag.png");
                        this.flagsField[xClickCoord][yClickCoord] = 1;
                    }
                }
            }
            return buttons;
        }



        private Button[,] openningAround(int xCoord, int yCoord, Button[,] buttons)
        /*Обращение к клеткам вокруг пустой*/
        {
            if ((neighboursField[xCoord][yCoord] == 0) && (minesField[yCoord][xCoord] == 0))
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        int x = xCoord + i;
                        int y = yCoord + j;
                        if (inField(neighboursField, y, x) && (buttons[xCoord, yCoord].Enabled == false))
                        {
                            buttons = openningMinesOrFlagCell(x, y, 'l', buttons);

                        }
                    }
                }
            }
            return buttons;
        }



        private void gameVerdict(int xCoord, int yCoord)
        /*Метод определения конца игры: присвоение verdict "Win"/"Lose"*/
        {
            if (minesField[yCoord][xCoord] == 1)
            {
                minesField[yCoord][xCoord] = -1;
                this.verdict = "Lose";
                buttons[xCoord, yCoord].BackgroundImage = Image.FromFile(@".\mine.jpg");
                button1.BackgroundImage = Image.FromFile(@".\sad_smile.png");
                openMines();
                timer1.Stop();
            }
            else if (openedCellsCount == (xSide * ySide - minesCount))
            {
                this.verdict = "Win";
                openMines();
                timer1.Stop();
                useDateBase();
            }
        }



        private void openMines()
        /*Вскрытие мин для конца игры*/
        {
            for (int x = 0; x < ySide; x++)
            {
                for (int y = 0; y < xSide; y++)
                {
                    buttons[y, x].Enabled = false;
                    if (minesField[x][y] == 1)
                    {
                        buttons[y, x].BackgroundImage = Image.FromFile(@".\mines.jpg");
                    }
                }
            }
        }

        private void useDateBase()
        {
            string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.mdb;";
            // создаем экземпляр класса OleDbConnection
            OleDbConnection dbConnection = new OleDbConnection(connectString);
            // открываем соединение с БД
            dbConnection.Open();

            string query = null;
            if(lvl == 0)
                query = "INSERT INTO [0] ([playerName], [time]) VALUES ('"+playerName+"', "+time+")";
             else if(lvl == 2)
                query = "INSERT INTO [1] ([playerName], [time]) VALUES ('" + playerName + "', " + time + ")";
            else if (lvl == 2)
                query = "INSERT INTO [2] ([playerName], [time], [x], [y], [mines]) VALUES ('" + playerName + "', " + time + ", "+ xCount +", "+ yCount +", "+ minesCount +")";
            OleDbCommand command = new OleDbCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();
        } 
        
    }
    
}
