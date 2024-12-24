using AutoMapper;

namespace Benchmark;

/*
| Method        | Mean      | Error     | StdDev    | Gen0   | Allocated |
|-------------- |----------:|----------:|----------:|-------:|----------:|
| AutoMapper    | 57.756 ns | 1.0197 ns | 0.9039 ns | 0.0038 |      32 B |
| ManualMapping |  4.403 ns | 0.0866 ns | 0.0723 ns | 0.0038 |      32 B | 
*/

[MemoryDiagnoser] 
public class AutoMapperVsManualMapping
{
    private readonly IMapper _mapper;
    private readonly User _user;

    public AutoMapperVsManualMapping()
    {
        _mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>()).CreateMapper();
        _user = new User
        {
            Id = 1,
            Name = "John Doe",
            Age = 30
        };
    }

    [Benchmark]
    public UserDto AutoMapper()
    {
        return _mapper.Map<UserDto>(_user);
    }

    [Benchmark]
    public UserDto ManualMapping()
    {
        return new UserDto
        {
            Id = _user.Id,
            Name = _user.Name,
            Age = _user.Age
        };
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}