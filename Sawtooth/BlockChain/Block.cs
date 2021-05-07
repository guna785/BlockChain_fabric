using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Cms.Ecc;

namespace Sawtooth.BlockChain
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<BlockTransaction> Transactions { get; set; }
        public int Nonce { get; set; } = 0;

        public Block(DateTime timeStamp, string previousHash, IList<BlockTransaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
        }

        public string CalculateHash()
        {
            ECDsa sha256 = ECDsa.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            byte[] outputBytes = sha256.SignHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
