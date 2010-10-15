namespace Endjin.Templify.MSBuild.Contracts.Mappers
{
    using Endjin.Templify.Domain.Infrastructure;

    public interface ICommandOptionsMapper
    {
        CommandOptions MapFrom(CreatePackage createPackage);
    }
}