using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    interface Cipher
    {
        string Encrypt(string s);
        string Decrypt(string s);
    }
}
