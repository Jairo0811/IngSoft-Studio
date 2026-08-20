namespace IngSoftStudio.Application.Common;

public interface ICurrentUserContext
{
    Guid UserId { get; }
    bool IsAdmin { get; }
}
