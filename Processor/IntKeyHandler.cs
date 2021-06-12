using Google.Protobuf;
using PeterO.Cbor;
using Sawtooth.Sdk;
using Sawtooth.Sdk.Processor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colorful;
using Console = Colorful.Console;

namespace Processor
{
    public class IntKeyHandler : ITransactionHandler
    {
        const string familyName = "intkey";
        readonly string PREFIX = familyName.ToByteArray().ToSha512().ToHexString().Substring(0, 6);

        public string FamilyName { get => familyName; }
        public string Version { get => "1.0"; }
        public string[] Namespaces { get => Arrayify(PREFIX); }

        T[] Arrayify<T>(T obj) => new[] { obj };
        string GetAddress(string name) => PREFIX + name.ToByteArray().ToSha512().TakeLast(32).ToArray().ToHexString();

        public async Task ApplyAsync(TpProcessRequest request, TransactionContext context)
        {
            var obj = CBORObject.DecodeFromBytes(request.Payload.ToByteArray());

            var name = obj["Name"].AsString();
            var verb = obj["Verb"].AsString().ToLowerInvariant();

            switch (verb)
            {
                case "set":
                    var value = obj["Value"].AsString();
                    await SetValue(name, value, context);
                    break;
                case "update":
                     value = obj["Value"].AsString();
                    await UpdateValue(name,value, context);
                    break;
                case "dec":
                    await Decrease(name, context);
                    break;
                default:
                    throw new InvalidTransactionException($"Unknown verb {verb}");
            }
        }

        async Task Decrease(string name, TransactionContext context)
        {
            var state = await context.GetStateAsync(Arrayify(GetAddress(name)));
            if (state != null && state.Any() && !state.First().Value.IsEmpty)
            {
                var val = BitConverter.ToInt32(state.First().Value.ToByteArray(), 0) - 1;
                await context.SetStateAsync(new Dictionary<string, ByteString>
                {
                    { state.First().Key, ByteString.CopyFrom(BitConverter.GetBytes(val)) }
                });
                Console.WriteLine($"Value for {name} decreased to {val}", Color.Orange);
                return;
            }
            throw new InvalidTransactionException($"Verb is 'dec', but state wasn't found at this address");
        }

        async Task UpdateValue(string name,string value, TransactionContext context)
        {
            var state = await context.GetStateAsync(Arrayify(GetAddress(name)));
            if (state != null && state.Any() && !state.First().Value.IsEmpty)
            {
                await context.SetStateAsync(new Dictionary<string, ByteString>
                {
                    { state.First().Key, ByteString.CopyFrom(Encoding.UTF8.GetBytes(value)) }
                });
                Console.WriteLine($"Value for {name} increased to {value}", Color.Green);
                return;
            }
            throw new InvalidTransactionException("Verb is 'inc', but state wasn't found at this address");
        }

        async Task SetValue(string name, string value, TransactionContext context)
        {
            var state = await context.GetStateAsync(Arrayify(GetAddress(name)));
            if (state != null && state.Any() && !state.First().Value.IsEmpty)
            {
                throw new InvalidTransactionException($"Verb is 'set', but address is aleady set");
            }
            await context.SetStateAsync(new Dictionary<string, ByteString>
            {
                { GetAddress(name), ByteString.CopyFrom(Encoding.UTF8.GetBytes(value)) }
            });
            Console.WriteLine($"Value for {name} set to {value}", Color.Blue);
        }
    }
}
