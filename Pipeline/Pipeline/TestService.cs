using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System.Diagnostics;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Pipeline
{
    public class TestService
    {
        /**Checks if an integer is greater than 0,
         * If int > 1 then returns a string ending in 's'
         * Else returns a string without 's'
         * 
         * @param count integer to check
         */
        
        public string PluralChecker(int count)
        {
            if (count == 1)
                return $"Clicked {count} time";
            else
                return $"Clicked {count} times";
        }

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public class Remote()
        {
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }


        // Returns a list of all books from bookdb
        public List<Book> GetBooks()
        {


            Remote? remote = config.GetRequiredSection("Remote").Get<Remote>();

            string server = remote.Server;
            string username = remote.Username;
            string password = remote.Password;

            using (var client = new SshClient(server, username, password))
            {
                try
                {
                    client.Connect();

                    if (client.IsConnected)
                    {

                        int localPort = 1431;

                        // Create the port forwarding
                        var portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)localPort, server, (uint)1433);
                        client.AddForwardedPort(portForwarded);
                        portForwarded.Start();


                        using (var context = new BookDb())
                        {
                            var x = context.Books.ToList();
                            portForwarded.Stop();

                            return x;
                        }

                    }
                    else
                    {
                        Debug.WriteLine("SSH client failed to connect.");
                        var x = new List<Book>();
                        var v = new Book { Id = 1, Name = "Exception Happened :(", Author = "Sorry", Blurb = "SSH client failed to connect." };
                        x.Add(v);
                        return x;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    var x = new List<Book>();
                    var v = new Book { Id = 1, Name = "Exception Happened :(", Author = "Sorry", Blurb = "" };
                    x.Add(v);
                    return x;
                }
                finally
                {
                    client.Disconnect();
                }
            }

            return null;

        }
            
    }
}
