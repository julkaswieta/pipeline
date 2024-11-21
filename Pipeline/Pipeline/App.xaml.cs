using Microsoft.Data.SqlClient;
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


        public App()
        {

            InitializeComponent();

            MainPage = new AppShell();


        }
    }
}
