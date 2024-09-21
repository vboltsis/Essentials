using FastEndpoints;
using FastVerticalSlices.Features.GetStudents;
using NetArchTest.Rules;

namespace ArchitecturalUnitTest;

public class EndpointSummaryTests
{
    [Fact]
    public void AllEndpointsShouldHaveCompleteSummaries()
    {
        var assembly = typeof(GetStudentEndpoint).Assembly;

        var endpointTypes = Types.InAssembly(assembly)
            .That()
            .Inherit(typeof(Endpoint<,>))
            .GetTypes();

        var summaryType = Types.InAssembly(assembly)
            .That()
            .Inherit(typeof(Summary<>))
            .GetTypes();

        Assert.Equal(endpointTypes.Count(), summaryType.Count());
    }
}
