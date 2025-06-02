using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace VideoProjector.Configuration
{
    /// <summary>
    /// Provides configuration for the Serilog logger.
    /// </summary>
    public static class LoggerConfig
    {
        /// <summary>
        /// Configures the Serilog logger using settings from the provided configuration.
        /// </summary>
        /// <param name="configuration">The application configuration containing logger settings.</param>
        public static void ConfigureLogger(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        AutoCreateSqlTable = true, // Automatically create the table if not exists
                        TableName = "Logs"       // Name of the table to store logs
                    },
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information // Minimum log level
                )
                .Filter.ByExcluding(logEvent=>logEvent.Properties.ContainsKey("SourceContext")&&
                                              logEvent.Properties["SourceContext"].ToString().Contains("Microsoft"))
                .CreateLogger();
        }
    }
}
