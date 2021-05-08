using System;
using System.Collections.Generic;
using System.Text;

namespace Sawtooth.BlockChain
{
    public class BlockTransaction
    {
        public string userId { get; set; }
        public string BP { get; set; }
        public DateTime createdAt { get; set; }
        public BlockTransaction()
        {

        }
        public BlockTransaction(string _userId,string _BP,string _gulcoseLevel, DateTime _createdAt)
        {
            userId = _userId;
            BP = _BP;
            createdAt = _createdAt;
        }

    }
}
