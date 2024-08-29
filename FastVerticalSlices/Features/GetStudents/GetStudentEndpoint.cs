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
        Post("/api/students/getstudent");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetStudentRequest req, CancellationToken ct)
    {
        //resolve based on request
        _studentRepository.GetStudent();

        var response = new GetStudentResponse
        {
            FullName = req.FirstName + " " + req.LastName,
            IsOver18 = req.Age > 18
        };

        await SendAsync(response);
    }
}

public class GetStudentResponse
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
}

public class GetStudentRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}
