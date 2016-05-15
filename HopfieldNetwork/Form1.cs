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
using System.Threading;

namespace HopfieldNetwork
{
    public partial class Form1 : Form
    {
        int N = 10;
        int[,] A;
        String path = "../../Character/";
        private int _noOfNeurons;
        private int _inputMatrixSize;
        private bool[] _visitedNeurons;
        private bool load;
        private bool train;
        private Random _randomNumber;
        private int[,] WeightMatrix;
        private int[,] InputMatrix;
        private int[,] OutputMatrix;
        System.Drawing.Graphics formGraphics;
        public Form1()
        {
            InitializeComponent();
            formGraphics = this.CreateGraphics();

        }
        public void initHopfieldNetwork(int noOfNeurons, int inputMatrixSize)
        {
            _noOfNeurons = noOfNeurons;
            _inputMatrixSize = inputMatrixSize;
            WeightMatrix = new int[_noOfNeurons, _noOfNeurons];

            _visitedNeurons = new bool[_noOfNeurons];
            _randomNumber = new Random();
        }

        public void TrainNetwork(List<int[,]> patternsToLearn)
        {
            var noOfPaterns = patternsToLearn.Count;
            for (int k = 0; k < noOfPaterns; k++)
            {
                for (int i = 0; i < _noOfNeurons; i++)
                {
                    for (int j = 0; j < _noOfNeurons; j++)
                    {

                        if (i == j)
                            WeightMatrix[i, j] = 0;
                        else
                        {
                            int wij = patternsToLearn[k][i / _inputMatrixSize, i % _inputMatrixSize] * patternsToLearn[k][j / _inputMatrixSize, j % _inputMatrixSize];
                            WeightMatrix[i, j] += wij;
                            WeightMatrix[j, i] += wij;
                        }

                    }
                }
            }

        }

        private void SetOutputAsInput()
        {
            OutputMatrix = new int[_inputMatrixSize, _inputMatrixSize];
            for (int i = 0; i < _inputMatrixSize; i++)
            {
                for (int j = 0; j < _inputMatrixSize; j++)
                {
                    OutputMatrix[i, j] = InputMatrix[i, j];
                }
            }
        }

        private void CleanVisitedNeuronMatrix()
        {
            for (int i = 0; i < _noOfNeurons; i++)
            {
                _visitedNeurons[i] = false;
            }
        }

        private int GetRandomUnVisitedNeuron()
        {
            int index;
            do
            {
                index = _randomNumber.Next(0, _noOfNeurons);
            } while (_visitedNeurons[index]);
            return index;
        }

        private bool allVisited()
        {
            for (int i = 0; i < _noOfNeurons; i++)
            {
                if (!_visitedNeurons[i])
                    return false;
            }
            return true;
        }


        public void RunRecognition()
        {
            SetOutputAsInput();
            int noOfChanges = 1000;
            while (noOfChanges > 0)
            {
                noOfChanges = 0;
                // CleanVisitedNeuronMatrix();

                for (int neuron = 0; neuron < _noOfNeurons; neuron++) //select neuron
                {
                    double neuronOutput = (double)InputMatrix[neuron / _inputMatrixSize, neuron % _inputMatrixSize];
                    // double neuronOutput = 0;
                    for (int i = 0; i < _noOfNeurons; i++)
                    {
                        neuronOutput += WeightMatrix[neuron, i] * OutputMatrix[i / _inputMatrixSize, i % _inputMatrixSize];
                    }
                    var discreetNeuronOutput = Math.Sign(neuronOutput);
                    if (OutputMatrix[neuron / _inputMatrixSize, neuron % _inputMatrixSize] != discreetNeuronOutput)
                    {
                        noOfChanges++;
                        OutputMatrix[neuron / _inputMatrixSize, neuron % _inputMatrixSize] = discreetNeuronOutput;
                        DrawCharacterFromMatrix(OutputMatrix);
                        Thread.Sleep(1000);
                    }

                }

            }
            MessageBox.Show("Recognition is finished!");

        }

        public void Init()
        {
            Color back = Color.FromKnownColor(KnownColor.Control);
            formGraphics.Clear(back);
            Rectangle rectangle = new Rectangle();
            A = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rectangle.drawRectangle(formGraphics, i * 30, j * 30, 30, 30);
                    A[i, j] = -1;
                }
            }
        }
        public void InitGrid()
        {
            Color back = Color.FromKnownColor(KnownColor.Control);
            formGraphics.Clear(back);
            Rectangle rectangle = new Rectangle();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rectangle.drawRectangle(formGraphics, i * 30, j * 30, 30, 30);
                }
            }
        }

        public int[,] ReadMatrixFromFile(string character)
        {
            String input = File.ReadAllText(path + character + ".txt");
            int i = 0, j = 0;
            int[,] result = new int[N, N];
            foreach (var row in input.Split('\r'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    if (col.Trim().Contains('\n') || col.Trim().Length == 0)
                    {

                    }
                    else
                    {
                        result[j, i] = int.Parse(col.Trim());
                        j++;
                    }
                }
                i++;
            }
            return result;
        }
        public void DrawCharacterFromMatrix(int[,] matrix)
        {
            InitGrid();
            Brush[] myBrush = { Brushes.Black, new SolidBrush(Color.FromKnownColor(KnownColor.Control)) };
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        RectangleF rectangle = new RectangleF((j) * 30 + 1, (i) * 30 + 1, 29, 29);
                        formGraphics.FillRectangle(myBrush[0], rectangle);
                    }
                }
            }
        }
        public void WriteMatrixToFile(int[,] matrix)
        {
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(path + textBox1.Text + ".txt"))
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {                                
                           tw.Write(matrix[i, j]);
                           tw.Write(" ");
                    }
                    tw.WriteLine();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
            // formGraphics.Dispose();

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Brush[] myBrush = { Brushes.Black, new SolidBrush(Color.FromKnownColor(KnownColor.Control)) };
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    if ((e.X > (i - 1) * 30 && e.X < i * 30) && (e.Y > (j - 1) * 30 && e.Y < j * 30))
                    {
                        if (A[i - 1, j - 1] == 1)
                        {
                            A[i - 1, j - 1] = -1;
                            RectangleF rectangle = new RectangleF((i - 1) * 30 + 1, (j - 1) * 30 + 1, 29, 29);
                            formGraphics.FillRectangle(myBrush[1], rectangle);
                        }
                        else
                        {
                            A[i - 1, j - 1] = 1;
                            RectangleF rectangle = new RectangleF((i - 1) * 30 + 1, (j - 1) * 30 + 1, 29, 29);
                            formGraphics.FillRectangle(myBrush[0], rectangle);
                        }
                    }
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Init();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Alegeti un nume pentru caracter!");
            }
            else
            {
                WriteMatrixToFile(A);
            }
        }


        private void button3_Click(object sender, EventArgs e) //run recognition
        {
            if (load && train)
            {
                InputMatrix = new int[_inputMatrixSize, _inputMatrixSize];
                for (int i = 0; i < _inputMatrixSize; i++)
                {

                    for (int j = 0; j < _inputMatrixSize; j++)
                    {
                        InputMatrix[i, j] = A[i, j];
                    }
                }
                RunRecognition();
            }
            else
            {
                if (load)
                {
                    MessageBox.Show("Train the network!");
                }
                else
                {
                    MessageBox.Show("Load number!");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) //init network
        {

            if (checkedListBox1.CheckedItems.Count != 0)
            {
                System.Collections.Generic.List<int[,]> sablon = new List<int[,]>();
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    sablon.Add(ReadMatrixFromFile((string)itemChecked));
                }
                initHopfieldNetwork(100, 10);
                TrainNetwork(sablon);
                train = true;
                MessageBox.Show("Weight matrix is ready!");
            }
            else
            {
                MessageBox.Show("Choose patern(s) to learn!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string value = comboBox1.SelectedItem.ToString();
                A = ReadMatrixFromFile(value);
                DrawCharacterFromMatrix(A);
                load = true;
            }
            else
            {
                MessageBox.Show("Choose a number to load!");
            }
        }
    }
}
