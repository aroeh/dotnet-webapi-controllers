using Refit;

namespace Demo.Restuarants.Client.MVC.Helpers;

internal static class ApiErrorHelper
{
    public static string GetMessage(Exception exception)
    {
        if (exception is ApiException apiException)
        {
            if (!string.IsNullOrWhiteSpace(apiException.Content))
            {
                return apiException.Content;
            }

            return $"API request failed ({(int)apiException.StatusCode} {apiException.StatusCode}).";
        }

        return exception.Message;
    }
}
