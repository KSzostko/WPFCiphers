using System;
using System.Collections.Generic;

namespace WPFCiphers.Generators
{
    public class LFSR : RandomGenerator
    {
        private List<bool> _result;
        private bool[] _currentBits;
        private bool[] _performXor;

        public void StartGenerator(int[] powers)
        {
            throw new System.NotImplementedException();
        }

        public void GenerateSequence(int[] powers)
        {
            _result = new List<bool>();

            CheckPowers(powers);
        }

        public void StopGenerator()
        {
            throw new System.NotImplementedException();
        }

        public List<bool> GetSequence()
        {
            throw new System.NotImplementedException();
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
    }
}