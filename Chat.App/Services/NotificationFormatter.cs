using System.Text.Json;

namespace Chat.App.Services
{
    public static class NotificationFormatter
    {
        public static string FormatErrorMessage(string jsonError)
        {
            try
            {
                var errorObject = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonError);

                if (errorObject != null && errorObject.ContainsKey("error"))
                {
                    return errorObject["error"];
                }

                return jsonError;
            }
            catch (JsonException)
            {
                return jsonError;
            }
        }
    }
}