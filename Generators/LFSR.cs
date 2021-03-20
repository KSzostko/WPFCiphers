using System;
using System.Collections.Generic;
using System.Threading;

namespace WPFCiphers.Generators
{
    public class LFSR : RandomGenerator
    {
        private List<bool> _result;
        private bool[] _currentBits;
        private bool[] _performXor;

        private Thread _thread;
        private volatile bool _isRunning = true;

        public void StartGenerator(int[] powers)
        {
            _thread = new Thread(() => GenerateSequence(powers));
            _thread.Start();
        }

        public void GenerateSequence(int[] powers)
        {
            _result = new List<bool>();

            CheckPowers(powers);
            GenerateStartingBits();

            while (_isRunning)
            {
                bool prevBit = _currentBits[0];
                XorBits();
                
                for (int i = 1; i < _currentBits.Length; i++)
                {
                    bool temp = _currentBits[i];
                    _currentBits[i] = prevBit;
                    prevBit = temp;
                }

                _result.Add(prevBit);
            }
        }

        public void StopGenerator()
        {
            _isRunning = false;
            _thread.Join();
        }

        public List<bool> GetSequence()
        {
            return _result;
        }

        private void CheckPowers(int[] powers)
        {
            Array.Sort(powers);
            
            _performXor = new bool[powers[powers.Length - 1]];
            _currentBits = new bool[powers[powers.Length - 1]];

            foreach (int number in powers)
            {
                _performXor[number - 1] = true;
            }
        }

        private void GenerateStartingBits()
        {
            Random random = new Random();
            
            for (int i = 0; i < _currentBits.Length; i++)
            {
                _currentBits[i] = random.NextDouble() > 0.5;
            }
        }

        private void XorBits()
        {
            bool firstValue = true;
            bool res = false;
            
            for(int i = 0; i < _currentBits.Length; i++)
            {
                if (_performXor[i])
                {
                    if (firstValue)
                    {
                        res = _currentBits[i];
                        firstValue = false;
                    }
                    else
                    {
                        res = res != _currentBits[i];
                    }
                }
            }

            _currentBits[0] = res;
        }
    }
}