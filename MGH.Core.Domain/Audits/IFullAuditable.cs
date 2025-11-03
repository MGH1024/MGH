
namespace MGH.Core.Domain.Audits;

public interface IFullAuditable : ICreationAuditable, IDeletionAuditable, IModificationAuditable
{

}