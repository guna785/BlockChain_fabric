using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleTree
{
    public class MerkleException : ApplicationException
    {
        public MerkleException(string msg) : base(msg)
        {
        }
    }
}
