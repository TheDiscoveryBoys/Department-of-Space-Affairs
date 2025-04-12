using System.Text.Json.Serialization;

public record ApplicationDocument(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("fileName")] string FileName,
    [property: JsonPropertyName("s3Url")] string S3Url,
    [property: JsonPropertyName("passportApplicationId")] int PassportApplicationId
);