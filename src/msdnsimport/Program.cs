namespace msdnsimport
{
    using CommandLine;
    using MaestroPanel.MsDnsZoneManager;
    using MaestroPanelApi;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var result = CommandLine.Parser.Default.ParseArguments(args, options);

            if (!result)
            {
                Console.WriteLine("Invalid parameters.");
                System.Environment.Exit(1);                
            }

            Console.WriteLine("Key: {0}", options.APIKey);
            Console.WriteLine("Host: {0}", options.Host);

            var c = new Client(options.APIKey, options.Host, options.Port, options.SSL);
            var d = new ZoneManage();

            Console.WriteLine("Loading DNS Zones...");

            var zones = d.GetAllZones();
            var index = 1;
                        
            Console.WriteLine("Zone Count: {0}", zones.Count);
            Console.WriteLine("Deploy DNS Zones to MaestroPanel...");

            var count = new Stopwatch();
            count.Start();

            foreach (var item in zones)
            {
                var records = item.Records
                                .Select(m => String.Format("{0},{1},{2},{3}", GetName(item.Name, m.Name), m.RecordType, m.Data, m.Priority))
                                .ToArray();

                var r = c.SetDnsZone(item.Name,
                        item.SOA.ExpireLimit.ToString(),
                        item.SOA.MinimumTTL.ToString(),
                        item.SOA.RefreshInterval.ToString(),
                        item.SOA.ResponsibleParty,
                        item.SOA.RetryDelay.ToString(),
                        item.SOA.SerialNumber.ToString(), 
                        item.SOA.PrimaryServer, true, records);

                Console.Write("{2} {0}/{1} ", index, zones.Count, DateTime.Now);

                if (r.Code != 0)
                {
                    Console.Write("Error: {1}, {0}", r.Message, item.Name);                    
                }
                else
                {
                    Console.Write("Success: {0}", item.Name);
                }

                Console.Write(Environment.NewLine);
                index++;
            }

            count.Stop();
            Console.WriteLine("Time elapsed: {0}", count.Elapsed);
        }

        private static string GetName(string domain, string recordName)
        {
            var nameValue = "@";

            if (domain == recordName)
                return nameValue;

            var octets = recordName.Split('.');

            if (octets.Length > 0)
                nameValue = octets.FirstOrDefault();

            return nameValue;
        }
    }

    public class Options
    {
        [Option('k',"key", HelpText = "MaestroPanel API Key", Required = true)]
        public string APIKey { get; set; }

        [Option('h', "host", HelpText = "MaestroPanel Host", Required = true)]
        public string Host { get; set; }

        [Option('p', "port", DefaultValue = 9715, HelpText = "MaestroPanel Port")]
        public int Port { get; set; }

        [Option('s', "ssl", DefaultValue = false, HelpText = "MaestroPanel Enable SSL")]
        public bool SSL { get; set; }

        [HelpOption]
        public string Usage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("MaestroPanel Microsoft DNS Import Tool");
            usage.AppendLine("");            
            usage.AppendLine("Parameters:");
            usage.AppendLine("");            
            usage.AppendLine("\tkey: MaestroPanel API Key");
            usage.AppendLine("\thost: MaestroPanel Web Management Service Host. IP or Hostname");
            usage.AppendLine("\tport: MaestroPanel Web Management Service Port. Default 9715");
            usage.AppendLine("\tssl: Use SSL protocols access to MaestroPanel. Default false");
            usage.AppendLine("");            
            usage.AppendLine("Usage:");
            usage.AppendLine("");            
            usage.AppendLine("msdnsimport --key 1_885bd9d868494d078d4394809f5ca7ac --host 192.168.5.2");

            return usage.ToString();
        }
    }
}
