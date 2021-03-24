using System.Collections.Generic;

namespace WPFCiphers.Generators
{
    public interface RandomGenerator
    {
        public void StartGenerator(int[] powers);
        // you don't have to call GenerateSequence in the app
        public void GenerateSequence(int[] powers);
        public void StopGenerator();
        public List<bool> GetSequence();
    }
}