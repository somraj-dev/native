namespace AxioVital.Application.Commands;

public record CreateUserCommand(
    Guid TenantId,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    List<string> Roles
);
