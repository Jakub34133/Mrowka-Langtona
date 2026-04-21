using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangtonAnt_Simulation
{
    public partial class Form1 : Form
    {
        // Rozmiar komórek.
        int cellWidth = 30;
        int cellHeight = 30;

        // Ilość komórek.
        int cellRows = 30;
        int cellColumns = 20;

        // Lista komórek
        Label[,] cells;

        // Prawdopodobieństwo białych komórek
        double whiteCells = 0.95;

        // Pozycja mrówki
        int antX;
        int antY;
        Direction antDirection;

        private enum Direction
        {
            TOP,
            BOTTOM,
            RIGHT,
            LEFT
        }

        private void SetAnt(int x, int y, Direction antDirection)
        {
            antLabel.Location = new Point(x * cellWidth, y * cellHeight);
            this.antX = x;
            this.antY = y;
            this.antDirection = antDirection;
        }

        public Form1()
        {
            InitializeComponent();

            // Ustawienie rozmiaru okna.
            int formWidth = cellWidth * cellRows + 20;
            int formHeight = cellHeight * cellColumns + 40;

            this.Width = formWidth;
            this.Height = formHeight;

            // Ustawienie mrówki na środku macierzy, początkowo idzie w góre.
            antLabel.BackColor = Color.Red;
            antLabel.Size = new Size(cellWidth, cellHeight);
            SetAnt(cellRows / 2, cellColumns / 2, Direction.TOP);

            // Wygenerowanie komórek.
            GenerateCells();

            // Wylosowanie kolorów komórek
            RandomizeColor(whiteCells);

            // Rozpoczęcie symulacji.
            Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Start()
        {
            antLabel.Location = new Point(antX * cellWidth, antY * cellHeight);

            timer1.Start();
        }

        private void GenerateCells()
        {
            // Ustawienie macierzy.
            cells = new Label[cellWidth, cellHeight];

            // Dodanie w pętli siatki komórek.
            for (int i = 0; i < cellRows; i++)
            {
                for(int j = 0; j < cellColumns; j++)
                {

                    // Ustawienie pozycji komórki.
                    int posX = i * cellWidth;
                    int posY = j * cellHeight;

                    // Ustawienie właściwości komórki.
                    Label label = new Label();
                    label.Width = cellWidth;
                    label.Height = cellHeight;
                    label.BackColor = Color.White;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Location = new Point(posX, posY);

                    // Dodanie komórki.
                    this.Controls.Add(label);
                    cells[i,j] = label;
                }
            } // pętla dodawania siatki komórek
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (antX < 0 || antX > cellRows || 
                antY < 0 || antY > cellColumns)
            {
                timer1.Stop();
                return; 
            }

            // Pobierz obecną komórke
            Label currentCell = cells[antX, antY];

            if(currentCell == null)
            {
                timer1.Stop();
                return;
            }

            // Zrób krok.
            // (1) Obróć mrówkę.
            rotateAnt(currentCell.BackColor);

            // (2) Przejdź dalej.
            if (antDirection == Direction.TOP)
                antY -= 1;
            else if (antDirection == Direction.BOTTOM)
                antY += 1;
            else if (antDirection == Direction.RIGHT)
                antX += 1;
            else if (antDirection == Direction.LEFT)
                antX -= 1;

            SetAnt(antX, antY, antDirection);

            // (3) Zmień kolor komórki pod sobą.
            if (currentCell.BackColor == Color.White)
            {
                currentCell.BackColor = Color.Black;
            }
            else
            {
                currentCell.BackColor = Color.White;
            }
        }

        // Obrót mrówki w prawo - jeśli jest na białym polu,
        // w lewo - jeśli jest na czarnym polu
        private void rotateAnt(Color cellColor)
        {
            if (antDirection == Direction.TOP)
            {
                if (cellColor == Color.White)
                    antDirection = Direction.RIGHT;
                else
                    antDirection = Direction.LEFT;
            }
            else
            if (antDirection == Direction.BOTTOM)
            {
                if (cellColor == Color.White)
                    antDirection = Direction.LEFT;
                else
                    antDirection = Direction.RIGHT;
            }
            else
            if (antDirection == Direction.RIGHT)
            {
                if (cellColor == Color.White)
                    antDirection = Direction.BOTTOM;
                else
                    antDirection = Direction.TOP;
            }
            else
            if (antDirection == Direction.LEFT)
            {
                if (cellColor == Color.White)
                    antDirection = Direction.TOP;
                else
                    antDirection = Direction.BOTTOM;
            }
        }

        private void RandomizeColor(double whiteProbability)
        {
            Random random = new Random();

            // Wybranie wszystkich komórek.
            for (int i = 0; i < cellRows; i++)
            {
                for (int j = 0; j < cellColumns; j++)
                {
                    // Wybranie koloru.
                    Color color = Color.White;

                    double randValue = random.NextDouble();
                    if (randValue > whiteProbability)
                        color = Color.Black;

                    // Zamiana koloru.
                    Label label = cells[i, j];
                    label.BackColor = color;
                }
            } // pętla podmiany kolorów
        }
    }
}
