using Microsoft.Extensions.DependencyInjection;
using Vicgital.Infrastructure.Configuration;
using Vicgital.Infrastructure.Configuration.Extensions;
using Vicgital.Infrastructure.Configuration.Services;

var config = ConfigurationBuilder.BuildConfiguration();

var serviceCollection = new ServiceCollection();
serviceCollection.AddAppConfiguration(config);

var serviceProvider = serviceCollection.BuildServiceProvider();
var appConfig = serviceProvider.GetRequiredService<IAppConfiguration>();

var defaultValue = appConfig.GetValue("EnvKey1");
Console.WriteLine($"Value for 'EnvKey1': {defaultValue}");

defaultValue = appConfig.GetValue("Setting1");
Console.WriteLine($"Value for 'Setting1': {defaultValue}");

var defaultValue2 = appConfig.GetValue("IntSetting", 42);
Console.WriteLine($"Value for 'IntSetting': {defaultValue2}");



