using Microsoft.Extensions.Configuration;

namespace Vicgital.Infrastructure.Configuration
{
    /// <summary>
    /// Provides static methods for building application configuration by loading settings from JSON files, environment
    /// variables, and Azure App Configuration sources.
    /// </summary>
    /// <remarks>The methods in this class support environment-specific configuration by reading the
    /// 'ASPNETCORE_ENVIRONMENT' environment variable, defaulting to 'dev' if not set. Configuration is automatically
    /// reloaded if the underlying JSON files change at runtime. Use these methods to centralize and standardize
    /// configuration loading across the application.</remarks>
    public static class ConfigurationBuilder
    {


        /// <summary>
        /// Builds an application configuration by loading settings from JSON files and environment variables.
        /// </summary>
        /// <remarks>The method determines the environment by reading the 'ASPNETCORE_ENVIRONMENT'
        /// environment variable. If the variable is not set, it defaults to 'dev'. The configuration includes
        /// 'appsettings.json' (required), an optional 'appsettings.{Environment}.json', and all environment variables.
        /// Changes to the JSON files are reloaded automatically if modified at runtime.</remarks>
        /// <returns>An <see cref="IConfiguration"/> instance containing the combined application settings from
        /// 'appsettings.json', an optional environment-specific JSON file, and environment variables.</returns>
        public static IConfiguration BuildConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "dev";

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        /// <summary>
        /// Builds an application configuration by loading settings from JSON files, Azure App Configuration, and
        /// environment variables.
        /// </summary>
        /// <remarks>The method loads the base 'appsettings.json' file, an environment-specific JSON file
        /// based on the 'ASPNETCORE_ENVIRONMENT' variable (defaulting to 'dev' if not set), Azure App Configuration
        /// using the provided connection string, and environment variables. Later sources override values from earlier
        /// ones.</remarks>
        /// <param name="connectionString">The connection string used to access the Azure App Configuration store. Cannot be null or empty.</param>
        /// <returns>An <see cref="IConfiguration"/> instance containing the combined application settings from all configured
        /// sources.</returns>
        public static IConfiguration BuildAzureAppConfiguration(string connectionString)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "dev";

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddAzureAppConfiguration(connectionString)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        /// <summary>
        /// Builds an application configuration by loading settings from JSON files, Azure App Configuration, and
        /// environment variables.
        /// </summary>
        /// <remarks>The method loads 'appsettings.json' and an environment-specific JSON file based on
        /// the 'ASPNETCORE_ENVIRONMENT' variable. It then adds configuration from the specified Azure App Configuration
        /// endpoint and includes environment variables. The resulting configuration reflects the merged settings from
        /// all these sources.</remarks>
        /// <param name="endpoint">The URI of the Azure App Configuration endpoint to connect to. Cannot be null.</param>
        /// <param name="credential">The Azure credential used to authenticate with Azure App Configuration. Cannot be null.</param>
        /// <returns>An IConfiguration instance containing the combined application settings from JSON files, Azure App
        /// Configuration, and environment variables.</returns>
        public static IConfiguration BuildAzureAppConfiguration(Uri endpoint, Azure.Core.TokenCredential credential)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "dev";

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect(endpoint, credential);
                })
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
