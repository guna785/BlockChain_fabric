using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleTree
{
    public class Demo
    {
        public void CreateTree(ref MerkleTree tree, int numLeaves, List<DemoMerkleNode> merkleNodes)
        {
            
            for (int i = 0; i < numLeaves; i++)
            {
                tree.AppendLeaf(DemoMerkleNode.Create(i.ToString()).SetText(merkleNodes[i].Text));
            }

            tree.BuildTree();
        }
    }
}
