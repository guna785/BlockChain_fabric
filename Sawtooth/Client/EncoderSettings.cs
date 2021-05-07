using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawtooth.Client
{
    public class EncoderSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sawtooth.Sdk.Client.EncoderSettings"/> class.
        /// </summary>
        public EncoderSettings()
        {
            Inputs = new List<string>();
            Outputs = new List<string>();
        }
        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        /// <value>The name of the family.</value>
        public string FamilyName { get; set; }
        /// <summary>
        /// Gets or sets the family version.
        /// </summary>
        /// <value>The family version.</value>
        public string FamilyVersion { get; set; }
        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>The inputs.</value>
        public List<string> Inputs { get; set; }
        /// <summary>
        /// Gets or sets the outputs.
        /// </summary>
        /// <value>The outputs.</value>
        public List<string> Outputs { get; set; }
        /// <summary>
        /// Gets or sets the signer publickey.
        /// </summary>
        /// <value>The signer publickey.</value>
        public string SignerPublickey { get; set; }
        /// <summary>
        /// Gets or sets the batcher public key.
        /// </summary>
        /// <value>The batcher public key.</value>
        public string BatcherPublicKey { get; set; }
    }
}
