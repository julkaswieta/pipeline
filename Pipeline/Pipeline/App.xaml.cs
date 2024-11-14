using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System.ComponentModel.DataAnnotations;

namespace Pipeline
{
    public partial class App : Application
    {

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public class Remote()
        {
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
        }

        public App()
        {
            Remote? remote = config.GetRequiredSection("Remote").Get<Remote>();

            string server = remote.Server;
            int port = remote.Port;
            string username = remote.Username;
            string password = remote.Password;



            // Create a new SSH client connection
            using (var client = new SshClient(server, port, username, password))
            {
                try
                {
                    client.Connect();

                    if (client.IsConnected)
                    {

                        int localPort = 8080;
                        string remoteHost = remote.Server;
                        int remotePort = 22;

                        // Create the port forwarding
                        var portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)localPort, remoteHost, (uint)remotePort);
                        client.AddForwardedPort(portForwarded);
                        portForwarded.Start();


                        InitializeComponent();

                        MainPage = new AppShell();

                        portForwarded.Stop();
                    }
                    else
                    {
                        Console.WriteLine("SSH client failed to connect.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    client.Disconnect();
                }
            }

        }
    }
}
