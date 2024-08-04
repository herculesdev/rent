namespace Rent.Shared.Library.Messaging;

public class RabbitMqConfig
{
    public string Hostname { get; set; } = string.Empty;
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string ClientProvidedName { get; set; } = string.Empty;
    public bool AutomaticRecoveryEnabled { get; set; } = true;
    public bool TopologyRecoveryEnabled { get; set; } = true;
    public int RequestedConnectionTimeout { get; set; } = 60000;
    public int RequestedHeartbeat = 60;
    public int InitialConnectionRetries = 5;
    public int InitialConnectionRetryTimeoutMilliseconds = 200;

}