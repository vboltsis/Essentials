using FastEndpoints;
using FastEndpoints.Swagger;
using FastVerticalSlices.Shared.Repositories;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints()
   .SwaggerDocument(settings =>
   {
   });
builder.Services.AddResponseCaching();

builder.Services.AddSingleton<IStudentRepository, StudentRepository>();

var app = builder.Build();
app.UseResponseCaching();
app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();




















//Random example
public class MyEndpoint : Endpoint<MyRequest>
{
    public override void Configure()
    {
        Post("/api/user/create");
        AllowAnonymous();
        Description(b => b
            .Accepts<MyRequest>("application/json")
          .Produces<ErrorResponse>(400, "application/json")
          .ProducesProblemFE<InternalErrorResponse>(500));
    }

    public override async Task HandleAsync(MyRequest req, CancellationToken ct)
    {
        var response = new MyResponse
        {
            FullName = req.FirstName + " " + req.LastName,
            IsOver18 = req.Age > 18
        };

        await SendAsync(response);
    }
}

public class MyRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public class MyResponse
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
}

class MyEndpointSummary : Summary<MyEndpoint>
{
    public MyEndpointSummary()
    {
        Summary = "short summary goes here";
        Description = "long description goes here";
        ExampleRequest = new MyRequest { Age = 10, FirstName = "takis", LastName = "kamateros" };
        Response(200, "ok response with body", example: new MyResponse { FullName = "takis kamateros", IsOver18 = false });
        Response<ErrorResponse>(400, "validation failure");
        Response(404, "account not found");
    }
}