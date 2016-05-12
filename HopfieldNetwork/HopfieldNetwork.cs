using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopfieldNetwork
{
    public class HopfieldNetwork
    {
        private int _noOfNeurons;
        private int _inputMatrixSize;
        private bool[] _visitedNeurons;
        private Random _randomNumber;
        private double[,] WeightMatrix { get; set; }
        public int[,] InputMatrix { get; set; }
        public int[,] OutputMatrix { get; set; }


        public HopfieldNetwork(int noOfNeurons, int inputMatrixSize)
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

            }
        }

    }
}
