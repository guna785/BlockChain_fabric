using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleTree
{
    public class DemoMerkleTree : MerkleTree
    {
        protected override MerkleNode CreateNode(MerkleHash hash)
        {
            return new DemoMerkleNode(hash);
        }

        protected override MerkleNode CreateNode(MerkleNode left, MerkleNode right)
        {
            return new DemoMerkleNode((DemoMerkleNode)left, (DemoMerkleNode)right);
        }
    }
}
