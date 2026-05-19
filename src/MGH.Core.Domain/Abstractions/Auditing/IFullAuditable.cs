
namespace MGH.Core.Domain.Abstractions.Auditing;

public interface IFullAuditable : ICreatedAuditable, IDeletedAuditable, IModifiedAuditable
{

}