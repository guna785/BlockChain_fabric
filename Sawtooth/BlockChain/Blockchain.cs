using System;
using System.Collections.Generic;
using System.Text;

namespace Sawtooth.BlockChain
{
    public class Blockchain
    {
        IList<BlockTransaction> PendingTransactions = new List<BlockTransaction>();
        public IList<Block> Chain { set; get; }
        public int Difficulty { set; get; } = 2;
        public int Reward = 1; //1 cryptocurrency

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }


        public void InitializeChain()
        {
            Chain = new List<Block>();
        }

        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<BlockTransaction>();
            return block;
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void CreateTransaction(BlockTransaction transaction)
        {
            PendingTransactions.Add(transaction);
        }
        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<BlockTransaction>();
            CreateTransaction(new BlockTransaction(minerAddress,null,null,DateTime.Now));
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(this.Difficulty);
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

       
    }
}
