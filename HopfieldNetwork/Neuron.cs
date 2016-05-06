using System;

namespace HopfieldNeuralNetwork
{

    public class Neuron
    {
        public int State { get; set; }


        public Neuron()
        {
            int r = new Random().Next(2);
            switch (r)
            {
                case 0: State = -1;
                    break;
                case 1: State = 1;
                    break;
            }
        }
        public Neuron(int state)
        {
            State = state;
        }

        public bool ChangeState(Double field)
        {
            bool res = false;
            if (field * State < 0)
            {
                State = -State;
                res = true;
            }
            return res;
        }
    }
}
