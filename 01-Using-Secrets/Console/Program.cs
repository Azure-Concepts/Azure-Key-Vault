using System;
using System.IO;
using KeyVault.Core;
using KeyVault.Core.Spec;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeyVault.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("Key Vault Manager");

            do
            {
                PrintOptions();
                var choice = System.Console.ReadLine();

                if (!(int.TryParse(choice, out int option)))
                    break;

                if (option > 2 || option < 0)
                    break;

                var keyStoreReader = serviceProvider.GetService<IKeyStoreReader>();
                switch(option)
                {
                    case 1:
                        ShowAllSecretsInConsole(keyStoreReader);
                        break;
                    case 2:
                        ShowSecretInConsole(keyStoreReader);
                        break;
                    default:
                        System.Console.WriteLine("Bad choice");
                        break;
                }

            } while (true);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            services.AddOptions();
            services.Configure<KeyVaultSettings>(configuration.GetSection("KeyVault"));

            services.AddSingleton(typeof(KeyVaultSettings), (sp) => sp.GetService<IOptions<KeyVaultSettings>>().Value);
            services.AddSingleton<IKeyStoreReader, KeyStoreReader>();
        }

        private static void PrintOptions()
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("1. See all Secrets");
            System.Console.WriteLine("2. View Secret");
            System.Console.WriteLine("3. Exit");
        }

        private static void ShowAllSecretsInConsole(IKeyStoreReader keyStoreReader)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            var secrets = keyStoreReader.GetAllSecrets().Result;
            if (secrets.Count == 0)
                System.Console.WriteLine("No secrets found in Key Vault");
            var secretCounter = 0;
            foreach (var secret in secrets)
            {
                secretCounter++;
                System.Console.WriteLine($"{secretCounter}. {secret.Key} - {secret.Value}");
            }
        }

        private static void ShowSecretInConsole(IKeyStoreReader keyStoreReader)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write(" > Enter secret name: ");
            var secretName = System.Console.ReadLine();
            var secret = keyStoreReader.GetSecretAsync(secretName).Result;
            if (string.IsNullOrEmpty(secret))
                System.Console.WriteLine($"No secret with this name exists");
            else
                System.Console.WriteLine($"Secret: {secret}");
        }
    }
}