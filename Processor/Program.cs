using Sawtooth.Sdk.Processor;
using System;
using System.Linq;
using Console = Colorful.Console;
namespace Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            var validatorAddress = args.Any() ? args.First() : "tcp://127.0.0.1:4004";

            var processor = new TransactionProcessor(validatorAddress);
            processor.AddHandler(new IntKeyHandler());
            processor.Start();

            Console.CancelKeyPress += delegate { processor.Stop(); };
        }
    }
}
