using System.Text.Json;

namespace LibraryManagement.Domain.ErrorModels
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        // ToString() daaoverraidebs rata shemdgom serializacia vukna JSONshi
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}