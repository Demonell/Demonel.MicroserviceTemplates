using AutoMapper;
using Domain.Common;

namespace Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAuditableProperties<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
            where TDestination : AuditableEntity
        {
            return expression.ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.LastModified, opt => opt.Ignore())
                .ForMember(d => d.LastModifiedBy, opt => opt.Ignore());
        }
    }
}