namespace Sheeva.Mapping;

using InnerMapper = global::MapsterMapper.IMapper;

public class MapsterMapper(InnerMapper mapper) : IMapper
{
    public TDest Map<TSource, TDest>(TSource source) => mapper.Map<TSource, TDest>(source);

    public TDest Map<TSource, TDest>(TSource source, TDest destination) =>
        mapper.Map(source, destination);
}
