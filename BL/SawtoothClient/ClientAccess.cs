using BL.Models;
using Newtonsoft.Json.Linq;
using PeterO.Cbor;
using Sawtooth.Sdk;
using Sawtooth.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.SawtoothClient
{
    public class ClientAccess
    {
        public resData postMedicalRecord(string userId,string verb, string value)
        {
            var obj = CBORObject.NewMap()
                                .Add("Name", userId)
                                .Add("Verb", verb)
                                .Add("Value", value);
            var prefix = "intkey".ToByteArray().ToSha512().ToHexString().Substring(0, 6);
            var signer = new Signer();

            var settings = new EncoderSettings()
            {
                BatcherPublicKey = signer.GetPublicKey().ToHexString(),
                SignerPublickey = signer.GetPublicKey().ToHexString(),
                FamilyName = "intkey",
                FamilyVersion = "1.0"
            };
            settings.Inputs.Add(prefix);
            settings.Outputs.Add(prefix);
            var encoder = new Sawtooth.Sdk.Client.Encoder(settings, signer.GetPrivateKey());

             var payload = encoder.EncodeSingleTransaction(obj.EncodeToBytes());
            //var payload = obj.EncodeToBytes();

            var content = new ByteArrayContent(payload);
            content.Headers.Add("Content-Type", "application/octet-stream");

            var httpClient = new HttpClient();

            var response = httpClient.PostAsync("http://localhost:8008/batches", content).Result;

            return new resData(){ response=response,encoderSettings=settings,obj=obj};
        }
        public async Task<dynamic> GetMedicalRecord(string uname)
        {
            var httpClient = new HttpClient();

            var response = httpClient.GetAsync("http://localhost:8008/batches").Result;

            var data =await response.Content.ReadAsStringAsync();
            dynamic json = JValue.Parse(data);
            
            return json;
        }
    }
}
