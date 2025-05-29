namespace Application.Common.Interfaces;

public interface IUser<TKey>
{
    TKey? Id { get; }
    List<string> Roles { get; }
}
