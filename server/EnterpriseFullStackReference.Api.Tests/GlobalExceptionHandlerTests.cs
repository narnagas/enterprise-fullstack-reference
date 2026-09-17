using EnterpriseFullStackReference.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EnterpriseFullStackReference.Api.Tests;

public sealed class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_WritesSanitizedProblemDetailsWithTraceId()
    {
        var services = new ServiceCollection();
        services.AddProblemDetails();
        await using var provider = services.BuildServiceProvider();

        var context = new DefaultHttpContext
        {
            RequestServices = provider,
            TraceIdentifier = "trace-test-123"
        };
        context.Request.Path = "/api/projects/search";
        context.Response.Body = new MemoryStream();

        var handler = new GlobalExceptionHandler(
            NullLogger<GlobalExceptionHandler>.Instance,
            provider.GetRequiredService<IProblemDetailsService>());

        var handled = await handler.TryHandleAsync(
            context,
            new InvalidOperationException("Sensitive implementation detail"),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();

        Assert.Contains("An unexpected error occurred.", body);
        Assert.Contains("trace-test-123", body);
        Assert.DoesNotContain("Sensitive implementation detail", body);
    }
}
