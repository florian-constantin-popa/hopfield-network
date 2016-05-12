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
        int [,] A;
        String path = "../../Character/";
        private int _noOfNeurons;
        private int _inputMatrixSize;
        private bool[] _visitedNeurons;
        private Random _randomNumber;
        private double[,] WeightMatrix;
        private int[,] InputMatrix;
        private int[,] OutputMatrix;
        System.Drawing.Graphics formGraphics;
        public Form1()
        {
            InitializeComponent();
          //  A = new Double[10,10];
            formGraphics = this.CreateGraphics();
           
        }
        public void initHopfieldNetwork(int noOfNeurons, int inputMatrixSize)
        {
            _noOfNeurons = noOfNeurons;
            _inputMatrixSize = noOfNeurons;
            WeightMatrix = new double[_noOfNeurons, _noOfNeurons];
            _visitedNeurons = new bool[_noOfNeurons];
            _randomNumber = new Random();
        }

        public void TrainNetwork(List<int[,]> patternsToLearn)
        {
            var noOfPaterns = patternsToLearn.Count;
            foreach (var pattern in patternsToLearn)
            {
                for (int i = 0; i < _noOfNeurons; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        if (i == j)
                            WeightMatrix[i, j] = 0;
                        else
                        {
                            var wij = 1 / (double)noOfPaterns * pattern[i / _inputMatrixSize, i % _inputMatrixSize] * pattern[j / _inputMatrixSize, j % _inputMatrixSize];
                            WeightMatrix[i, j] += wij;
                            WeightMatrix[j, i] += wij;
                        }

                    }
                }
            }
        }

        private void SetOutputAsInput()
        {

            for (int i = 0; i < _noOfNeurons; i++)
            {
                for (int j = 0; j < _noOfNeurons; j++)
                {
                    OutputMatrix[i, j] = InputMatrix[i, j];
                }
            }
        }

        private void CleanVisitedNeuronMatrix()
        {
            for (int i = 0; i < _noOfNeurons; i++)
            {
                _visitedNeurons[_noOfNeurons] = false;
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


        public void RunRecognition()
        {
            SetOutputAsInput();
            int noOfChanges = 1000;
            while (noOfChanges > 0)
            {
                noOfChanges = 0;
                CleanVisitedNeuronMatrix();
                var neuron = GetRandomUnVisitedNeuron(); //select neuron
                double neuronOutput = (double)InputMatrix[neuron / _inputMatrixSize, neuron % _inputMatrixSize];
                for (int i = 0; i < _noOfNeurons; i++)
                {
                    neuronOutput += WeightMatrix[neuron, i] * OutputMatrix[i / _inputMatrixSize, i % _inputMatrixSize];
                }
                var discreetNeuronOutput = (neuronOutput < 0) ? -1 : 1;
                if (OutputMatrix[neuron / _inputMatrixSize, neuron % _inputMatrixSize] != discreetNeuronOutput)
                {
                    noOfChanges++;
                    OutputMatrix[neuron / _inputMatrixSize, neuron % _inputMatrixSize] = discreetNeuronOutput;
                }
                DrawCharacterFromMatrix(OutputMatrix);
                Thread.Sleep(2000);
            }
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
        public int[,] ReadMatrixFromFile(string character)
        {
            String input = File.ReadAllText(path + character + ".txt" );
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
                        result[i, j] = int.Parse(col.Trim());
                        j++;
                    }
                }
                i++;
            }
        return result;
        }
        public void DrawCharacterFromMatrix(int[,] A)
        {
            Init();
            Brush[] myBrush = { Brushes.Black, new SolidBrush(Color.FromKnownColor(KnownColor.Control)) };
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                        if (A[i,j] == 1)
                        {
                            RectangleF rectangle = new RectangleF((j) * 30 + 1, (i) * 30 + 1, 29, 29);
                            formGraphics.FillRectangle(myBrush[0], rectangle);
                        }
                    }
                }
        }
        public void WriteMatrixToFile(int[,] A)
        {
            using (System.IO.TextWriter tw = new System.IO.StreamWriter(path + textBox1.Text + ".txt"))
            {
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    for (int i = 0; i < A.GetLength(1); i++)
                    {
                        if (i != 0)
                        {
                            tw.Write(" ");
                        }
                        if (A[i, j] == 1)
                        {
                            tw.Write(" ");
                            tw.Write(A[i, j]);
                        }
                        else
                        {
                            tw.Write(A[i, j]);
                        }
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
            Brush[] myBrush = { Brushes.Black, new SolidBrush(Color.FromKnownColor(KnownColor.Control))};
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    if ((e.X > (i - 1) * 30 && e.X < i * 30) && (e.Y > (j - 1) * 30 && e.Y < j * 30))
                    {
                       if(A[i-1,j-1] == 1){
                        A[i-1,j-1] = -1;
                        RectangleF rectangle = new RectangleF((i - 1) * 30 + 1, (j - 1) * 30 + 1, 29, 29);
                        formGraphics.FillRectangle(myBrush[1],rectangle);
                       }else
                        {
                        A[i-1,j-1] = 1;
                        RectangleF rectangle = new RectangleF((i - 1) * 30 + 1, (j - 1) * 30 + 1, 29, 29);
                        formGraphics.FillRectangle(myBrush[0],rectangle);
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
                MessageBox.Show("Gata");
            }
            else
            {
                MessageBox.Show("Alegeti sabloanele pentru invatare!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
            string value = comboBox1.SelectedItem.ToString();
            DrawCharacterFromMatrix(ReadMatrixFromFile(value));
        }
    }
}
