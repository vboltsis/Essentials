using FastEndpoints;

namespace FastVerticalSlices.Features.GetStudents;

class GetStudentEndpointSummary : Summary<GetStudentEndpoint>
{
    public GetStudentEndpointSummary()
    {
        Summary = "short summary goes here";
        Description = "long description goes here";
        ExampleRequest = new GetStudentRequest ("Takis", "Vassallo", 36);
        Response(200, "ok response with body", example: new GetStudentResponse ("Takis Vassallo", false ));
        Response<ErrorResponse>(400, "validation failure");
        Response(404, "account not found");
    }
}