using BL.Models;
using BL.service;
using DAL.Models;
using PeterO.Cbor;
using Sawtooth.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SawtoothClient
{
    public class ClientKeyManager
    {
        private IGenericBL<KeyMaintainer> _key;
        public ClientKeyManager(IGenericBL<KeyMaintainer> key)
        {
            _key = key;
        }
        public async Task<bool> SwatoothKeyStore(EncoderSettings settings, CBORObject obj)
        {
            if (obj != null && settings != null)
            {
                var res = await _key.InsertOneAsync(new KeyMaintainer()
                {
                    certficate = Convert.ToBase64String(obj.EncodeToBytes()),
                    keysettings = Newtonsoft.Json.JsonConvert.SerializeObject(settings),
                    uname = obj["Name"].ToString(),
                    CreatedAt = DateTime.Now

                });
                return res;
            }
            return false;
        }
        public async Task<dynamic> SwatoothRetriveData(dynamic data, string user)
        {
            var dat = _key.AsQueryable().Where(x => x.uname == user);
            var dlist = new List<MRecord>();
            foreach (var k in dat)
            {
                var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<EncoderSettings>(k.keysettings);
                if (settings.SignerPublickey.Length > 0)
                {
                    var d = CBORObject.DecodeFromBytes(Convert.FromBase64String(k.certficate));
                    var c = d["Value"].ToString();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordData>(Encoding.UTF8.GetString(Convert.FromBase64String(c)));
                    dlist.Add(new MRecord()
                    {
                        uname = user,
                        bp = obj.bp,
                        createdAt = k.CreatedAt
                    });
                }
            }
            return dlist;
        }
    }
}
