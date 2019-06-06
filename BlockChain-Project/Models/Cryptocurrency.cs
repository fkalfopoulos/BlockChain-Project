using Newtonsoft.Json;
using RSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_Project.Models
{
    public class Cryptocurrency
    {
        private List<Transaction> _currentTransactions = new List<Transaction>();
        private List<Block> _chain = new List<Block>();
        private List<Node> _nodes = new List<Node>();
        private Block _lastBlock => _chain.Last();


        public string NodeId { get; private set; }
        static int blockCount = 0;
        static decimal reward = 50;


        static string minerPrivateKey = "";
        static Wallet _minersWallet = RSA.RSA.KeyGenerate();

        public Cryptocurrency()
        {
            minerPrivateKey = _minersWallet.PrivateKey;
            NodeId = _minersWallet.PublicKey;

            var transaction = new Transaction { Sender = "0", Recipient = NodeId, Amount = 50, Fees = 0, Signature = "" };
            _currentTransactions.Add(transaction);

            CreateNewBlock(proof: 100, previousHash: "1"); //genesis block
        }

        private Block CreateNewBlock(int proof, string previousHash = null)
        {
            var block = new Block

            {
                Index = _chain.Count,
                TimeStamp = DateTime.UtcNow,
                Transactions = _currentTransactions.ToList(),
                Proof = proof,
                PreviousHash = previousHash ?? GetHash(_chain.Last())
            };

            _currentTransactions.Clear();
            _chain.Add(block);
            return block;
        }
        private string GetHash(Block block)
        {
            string blockText = JsonConvert.SerializeObject(block);
            return GetSha256(blockText);
        }

        private string GetSha256(string data)
        {

            var sha256 = new SHA256Managed();
            var hashBuilder = new StringBuilder();

            byte[] bytes = Encoding.Unicode.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);

            foreach (byte x in hash)
                hashBuilder.Append($"{x:x2}");
            return hashBuilder.ToString();
        }
    }
}

