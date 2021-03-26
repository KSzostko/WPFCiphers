using System.Collections.Generic;

namespace WPFCiphers.Generators
{
    interface RandomGenerator
    {
        void StartGenerator(int[] powers);
        // you don't have to call GenerateSequence in the app
        void GenerateSequence(int[] powers);
        void StopGenerator();
        List<bool> GetSequence();
    }
}