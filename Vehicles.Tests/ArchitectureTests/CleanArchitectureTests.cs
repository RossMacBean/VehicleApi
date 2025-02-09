using System.Reflection;

namespace Vehicles.UnitTests.ArchitectureTests;

/// <summary>
/// A simple set of tests that ensure that no one has accidentally broken our architecture constraints
/// </summary>
[TestFixture]
public class CleanArchitectureTests
{
    [Test]
    public void DomainLayer_DoesNotReferenceOtherLayers()
    {
        // Arrange
        var domainAssembly = typeof(Domain.AssemblyMarker).Assembly;
        var apiAssembly = typeof(Api.AssemblyMarker).Assembly;
        var infrastructureAssembly = typeof(Infrastructure.AssemblyMarker).Assembly;
        var applicationAssembly = typeof(Application.AssemblyMarker).Assembly;
        
        // Act
        var referencedAssemblies = domainAssembly.GetReferencedAssemblies();
        var disallowedAssemblies = new List<Assembly> { apiAssembly, infrastructureAssembly, applicationAssembly };
        
        // Assert
        Assert.Multiple(() =>
        {
            foreach (var disallowedAssembly in disallowedAssemblies)
            {
                Assert.That(referencedAssemblies.Select(ra => ra.Name), 
                    Does.Not.Contain(disallowedAssembly.GetName().Name));
            }
        });
    }

    [Test]
    public void InfrastructureLayer_DoesNotReferenceApplicationLayer()
    {
        // Arrange
        var infrastructureAssembly = typeof(Infrastructure.AssemblyMarker).Assembly;
        var applicationAssembly = typeof(Application.AssemblyMarker).Assembly;
        
        // Act
        var referencedAssemblies = infrastructureAssembly.GetReferencedAssemblies();
        
        // Assert
        Assert.That(referencedAssemblies.Select(ra => ra.Name), 
            Does.Not.Contain(applicationAssembly.GetName().Name));
    }
}