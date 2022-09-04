using System.Net.Mail;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minever.Networking.Serialization.Json;

public class JsonAttachmentConverter : JsonConverter<Attachment>
{
    public override Attachment Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var content = reader.GetString()!;

        if (content.StartsWith("data:"))
            content = content[5..];

        var splitedContent = content.Split(';');

        if (splitedContent.Length != 2)
            throw new FormatException("The content is in an invalid format.");

        var contentType = new ContentType(splitedContent[0]);
        var attachment  = Attachment.CreateAttachmentFromString(splitedContent[1], contentType);

        return attachment;
    }

    public override void Write(Utf8JsonWriter writer, Attachment value, JsonSerializerOptions options) =>
        throw new NotSupportedException();
}
