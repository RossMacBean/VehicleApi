using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Vehicles.ComponentTests;

public class ComponentTestFixture
{
    public HttpClient HttpClient { get; private init; } = null!;
    
    public static ComponentTestFixture Create()
    {
        var webApplicationFactory = new TestWebApplicationFactory<Api.Program>();
        
        return new ComponentTestFixture()
        {
            HttpClient = webApplicationFactory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                })
        };
    }
}