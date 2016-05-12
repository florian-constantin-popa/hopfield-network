using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HopfieldNeuralNetwork
{
    /// <summary>
    /// Represents the method that will handle an event that rise when Energy of Hopfield Neural Network changes.
    /// </summary>
    /// <param name="sender">The source of the event</param>
    /// <param name="e">An <typeparamref name="EnergyEventArgs"/> that contains value of Energy</param>
    public delegate void EnergyChangedHandler(object sender, EnergyEventArgs e);

    /// <summary>
    /// Defines the class for Hopfield Neural Network
    /// </summary>
    public class NeuralNetwork
    {
        public List<Neuron> Neurons { get; set; }

        public int N { get; set; }
        private int M { get; set; }
        private double Energy { get; set; }
        private int[,] WeightMatrix { get; set; }

        public NeuralNetwork(int n)
        {
            this.N = n;
            Neurons = new List<Neuron>(n);
            for (int i = 0; i < n; i++)
            {
                Neuron neuron = new Neuron();
                neuron.State = 0;
                Neurons.Add(neuron);
            }

            WeightMatrix = new int[n, n];
            M = 0;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    WeightMatrix[i, j] = 0;
                }
        }

        private void CalculateEnergy()
        {
            double tempE = 0;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (i != j)
                        tempE += WeightMatrix[i, j] * Neurons[i].State * Neurons[j].State;
            Energy = -1 * tempE / 2;
        }


        public List<Neuron> MatrixToNeuronList(int[,] matrix)
        {
            var neurons = new List<Neuron>();
            for (int i=0;i<matrix.GetLength(0);i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    neurons.Add(new Neuron(matrix[i, j]==0?-1:1));
            return neurons;

        }

        public void AddPattern(List<Neuron> pattern)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    if (i == j) WeightMatrix[i, j] = 0;
                    else WeightMatrix[i, j] += (pattern[i].State * pattern[j].State);
                }
            M++;
        }

     
        public void FreeMatrix()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    WeightMatrix[i, j] = 0;
        }

        public void Run(List<Neuron> initialState)
        {
            Neurons = initialState;
            int k = 1;
            int h = 0;
            while (k != 0)
            {
                k = 0;
                for (int i = 0; i < N; i++)
                {
                    h = 0;
                    for (int j = 0; j < N; j++)
                        h += WeightMatrix[i, j] * (Neurons[j].State);

                    if (Neurons[i].ChangeState(h))
                    {
                        k++;
                        CalculateEnergy();
                        OnEnergyChanged(new EnergyEventArgs(Energy, i));
                    }
                }
            }
            CalculateEnergy();
        }

        /// <summary>
        /// Occurs when the energy of neural network changes
        /// </summary>
        public event EnergyChangedHandler EnergyChanged;

        protected virtual void OnEnergyChanged(EnergyEventArgs e)
        {
            if (EnergyChanged != null)
                EnergyChanged(this, e);
        }
    }
}
