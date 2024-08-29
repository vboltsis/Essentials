using FastEndpoints;
using FluentValidation;

namespace FastVerticalSlices.Features.GetStudents;

public class GetStudentValidator : Validator<GetStudentRequest>
{
    public GetStudentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("your name is required!")
            .MinimumLength(5)
            .WithMessage("your name is too short!");

        RuleFor(x => x.Age)
            .GreaterThan(0)
            .WithMessage("we need your age!");
    }
}