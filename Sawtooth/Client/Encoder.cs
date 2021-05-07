using Google.Protobuf;
using Sawtooth.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawtooth.Client
{
    public class Encoder
    {
        readonly EncoderSettings settings;
        readonly ISigner signer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Client.Encoder"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <param name="privateKey">Private key.</param>
        public Encoder(EncoderSettings settings, byte[] privateKey)
        {
            this.settings = settings;
            this.signer = new Signer(privateKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Client.Encoder"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <param name="signer">Signer.</param>
        public Encoder(EncoderSettings settings, ISigner signer)
        {
            this.settings = settings;
            this.signer = signer;
        }

        /// <summary>
        /// Creates new transaction.
        /// </summary>
        /// <returns>The transaction.</returns>
        /// <param name="payload">Payload.</param>
        public Transaction CreateTransaction(byte[] payload)
        {
            var header = new TransactionHeader();
            header.FamilyName = settings.FamilyName;
            header.FamilyVersion = settings.FamilyVersion;
            header.Inputs.AddRange(settings.Inputs);
            header.Outputs.AddRange(settings.Outputs);
            header.Nonce = Guid.NewGuid().ToString();
            header.SignerPublicKey = settings.SignerPublickey;
            header.BatcherPublicKey = settings.BatcherPublicKey;
            header.PayloadSha512 = payload.ToSha512().ToHexString();

            var transaction = new Transaction();
            transaction.Payload = ByteString.CopyFrom(payload);
            transaction.Header = header.ToByteString();
            transaction.HeaderSignature = signer.Sign(header.ToByteArray().ToSha256()).ToHexString();

            return transaction;
        }

        /// <summary>
        /// Creates new batch.
        /// </summary>
        /// <returns>The batch.</returns>
        /// <param name="transactions">Transactions.</param>
        public Batch CreateBatch(IEnumerable<Transaction> transactions)
        {
            var batchHeader = new BatchHeader();
            batchHeader.TransactionIds.AddRange(transactions.Select(x => x.HeaderSignature));
            batchHeader.SignerPublicKey = signer.GetPublicKey().ToHexString();

            var batch = new Batch();
            batch.Transactions.AddRange(transactions.Select(x => x.Clone()));
            batch.Header = batchHeader.ToByteString();
            batch.HeaderSignature = signer.Sign(batchHeader.ToByteArray().ToSha256()).ToHexString();

            return batch;
        }

        /// <summary>
        /// Creates new batch.
        /// </summary>
        /// <returns>The batch.</returns>
        /// <param name="transaction">Transaction.</param>
        public Batch CreateBatch(Transaction transaction)
        {
            return CreateBatch(new[] { transaction });
        }

        /// <summary>
        /// Encode the specified batches.
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="batches">Batches.</param>
        public byte[] Encode(IEnumerable<Batch> batches)
        {
            var batchList = new BatchList();
            batchList.Batches.AddRange(batches);
            return batchList.ToByteArray();
        }

        /// <summary>
        /// Encode the specified batch.
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="batch">Batch.</param>
        public byte[] Encode(Batch batch)
        {
            return Encode(new[] { batch });
        }

        /// <summary>
        /// Encodes a single transaction.
        /// </summary>
        /// <returns>The single transaction.</returns>
        /// <param name="payload">Payload.</param>
        public byte[] EncodeSingleTransaction(byte[] payload) => Encode(CreateBatch(CreateTransaction(payload)));
    }
}
