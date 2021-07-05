using PeterO.Cbor;
using Sawtooth.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class resData
    {
        public HttpResponseMessage response { get; set; }
        public EncoderSettings encoderSettings { get; set; }
        public CBORObject obj { get; set; }
    }
}
