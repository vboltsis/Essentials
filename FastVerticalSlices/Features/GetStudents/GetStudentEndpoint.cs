using FastEndpoints;
using FastVerticalSlices.Shared.Repositories;

namespace FastVerticalSlices.Features.GetStudents;

public class GetStudentEndpoint : Endpoint<GetStudentRequest, GetStudentResponse>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentEndpoint(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public override void Configure()
    {
        Get("/api/students");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(GetStudentRequest req, CancellationToken ct)
    {
        //resolve based on request
        _studentRepository.GetStudent();

        var response = new GetStudentResponse
        (
            req.FirstName + " " + req.LastName,
            req.Age > 18
        );

        await SendAsync(response, cancellation: ct);
    }
}

public readonly record struct GetStudentResponse
{
    public readonly string FullName;
    public readonly bool IsOver18;

    public GetStudentResponse(string fullName, bool isOver18)
    {
        FullName = fullName;
        IsOver18 = isOver18;
    }
}

public readonly record struct GetStudentRequest
{
    public readonly string FirstName;
    public readonly string LastName;
    public readonly int Age;

    public GetStudentRequest(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }
}
