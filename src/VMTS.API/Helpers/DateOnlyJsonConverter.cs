using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VMTS.API.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private static readonly string[] SupportedFormats = { "yyyy-MM-dd", "yyyy/MM/dd" };

        public override DateOnly Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            string dateString = reader.GetString();
            foreach (var format in SupportedFormats)
            {
                if (
                    DateOnly.TryParseExact(
                        dateString,
                        format,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var result
                    )
                )
                {
                    return result;
                }
            }

            throw new FormatException($"Invalid date format: {dateString}");
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateOnly value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd")); // You can choose any format for writing back.
        }
    }
}
