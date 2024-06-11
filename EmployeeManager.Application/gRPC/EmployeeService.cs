using EmployeeManager.Domain.Repositories;
using Grpc.Core;
namespace EmployeeManager.Application.gRPC;

public class EmployeeServiceImplementation : EmployeeService.EmployeeServiceBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeeServiceImplementation(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public override async Task<EmployeeReply> GetEmployeeInfo(EmployeeRequest request, ServerCallContext context)
    {
        var employee = await _repository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Employee with ID {request.Id} not found"));
        }

        return new EmployeeReply
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Department = employee.Department.Name
        };
    }
}
