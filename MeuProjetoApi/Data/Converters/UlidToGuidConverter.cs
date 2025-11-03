using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUlid;

namespace MeuProjetoApi.Data.Converters
{
    public class UlidToGuidConverter : ValueConverter<Ulid, Guid>
    {
        public UlidToGuidConverter() : base(
            ulid => ulid.ToGuid(),
            guid => new Ulid(guid))
        {
        }
    }
}
