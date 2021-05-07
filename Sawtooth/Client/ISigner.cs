using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawtooth.Client
{
    public interface ISigner
    {
        /// <summary>
        /// Sign the specified digest.
        /// </summary>
        /// <returns>The sign.</returns>
        /// <param name="digest">Digest.</param>
        byte[] Sign(byte[] digest);

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <returns>The public key.</returns>
        byte[] GetPublicKey();
    }
}
