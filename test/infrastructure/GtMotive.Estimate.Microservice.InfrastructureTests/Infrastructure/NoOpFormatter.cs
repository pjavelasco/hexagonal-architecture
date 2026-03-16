using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    /// <summary>
    /// Custom formatter to validate the host and http response codes
    /// Ingest all output objects without use System.Text.Json.
    /// </summary>
    internal sealed class NoOpFormatter : IOutputFormatter
    {
        public bool CanWriteResult(OutputFormatterCanWriteContext context) => true;

        public Task WriteAsync(OutputFormatterWriteContext context)
        {
            return Task.CompletedTask;
        }
    }
}
