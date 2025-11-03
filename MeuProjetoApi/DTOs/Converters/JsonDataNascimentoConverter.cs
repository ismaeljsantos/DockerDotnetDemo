using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeuProjetoApi.DTOs.Converters
{
    public class JsonDataNascimentoConverter : JsonConverter<DateTime>
    {
        private static readonly string[] FormatosAceitos = new[]
        { 
            "yyyy-MM-dd",
            "dd/MM/yyyy",
            "MM-dd-yyyy",
            "yyyyMMdd"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? dateString = reader.GetString();
                if (DateTime.TryParseExact(dateString, FormatosAceitos, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return parsedDate.Date;
                }
            }

            throw new JsonException($"Formato de data inválido. Formatos aceitos: {string.Join(", ", FormatosAceitos)}");
            
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
}
