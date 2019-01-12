using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;

namespace PastaRiot
{
    public class GoogleApiHelper
    {
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        private static readonly string ApplicationName = "Bearded Pasta Riot subscribe";

        internal static bool AddToSheet(Order order)
        {
            var cr = new CredentialModel()
            {
                client_email = "connectapp@bearded-pasta-ri-1546694084499.iam.gserviceaccount.com",
                private_key = "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQChf3BQUtTDx59J\n3VMrMgoB5VRMmTx3oG5LW4SpjoriAtTBPW1FsnHS1b1IDKz3ScX1Q7c1H4l1Q8iP\nyPNi1smFfekPI+eqp3xZeExoU9B59XgYXjM826f0PN4nS3bytVWET/FFC5KJDVXH\nSoJuIM3WIK/o8QpRRkQPuw5ulHqNY0lOxPF7vCa3hJyKFvuLA+XReXyGXKbrcZ2s\nj7p1e3USzBWgVSH4cEQMtm5HiO+5pEZqVmfiWepVzvu8Uqb4Dy+VS+bgXOSMUksP\n+oTIU5RdxgvJYsg5nc1Fl2ocUs2mrcGSbrIC84khIZKXlG2lAU2lGGdq1zZeK7OL\nBaQA2qytAgMBAAECggEAD7eBTK/i+PKyc9PQRkHma2zQGWxIUDQSx86uSHoQ1hIE\ndpHt0nydSzF17E8ffil11Hq0l7zr+beRCz05QoM4tX8dcVXDM/58wZBNj1Go1lWn\nTGfOgLu3yrMpw5U6jEBNhETrbBbGOgRMNsrvipwyrAYeWIMsx+AYu5cuvmBA1QWO\n94DUmEJeP3ex9s+jENdbeGmaJxxz9DpKOgUcNnZupPI1632FMp/bdwtTW+35Ujo8\npA9qPYlhDuE4WFfMt3j9lWaz9f5AGww2Wv7bYS483QSRKb6/BeVjyHlUEbRSj+aC\nPXDeGVtq5w7Seoc8XAA0RjhuU0+3coIp6jaXQGWbmQKBgQDW+7iP0xCHfLLsD6/7\naFGyCdOgt2uf04IfyQMJpbNXiBCXIViX695RyGpC8+i2yRUSnkZlzv6lTEkYf3sA\n9iZVwSpttNp3hmyM7OI/nn394pehXOKj4FMDtjTLmRXhFc4P0Y/zLR5ILjPPe9kd\npXdgjB+A7agtwqot9ezu+pWZDwKBgQDAT10imTgBF72kqEGEQCPw1sbo3V3rXvhN\nyoe0R9lAAViPNJfYeQ+TGoW1/j0xJRPdFlDHWw7nQ7Y+sIOjJZBYKtE5FPGGGOj6\nS6pwB33YLUGYKyjZ6D89kHboPqqmjmwr73dJyK/qO1lFLnKL/rag+pmCt2HRXlnt\nw/UWCdkGgwKBgQCcabSUX9oM2XtMKPW/Et0tjdy9d/YD6N0pxxRAtqBPNR3s90P/\n9IpiMlCtucw9APwl+aX6eKnpFiGwgz+5KV4m0k3OV+EDSXg86DSMWQIN8AimTHBR\nDusXLkAnSZATncu40p3LLa50kbj8Yn1LBpJeWlSpdz//Wzx6CA2EqiihnQKBgD57\nSqpMF8sbGI2f8TFz2XmNfrD4A2TOxWQY8oBbe7V0+n4Eu3Uk2C+WRW08/kqyXkLN\n1k7/QddXw6WYmhxuvvg2aSXfrR1BiKiR3v1pIbUT9yTmmO47rBhnkIqimbVlvrWD\np0E8yBMVV0rH7YWYq2OgKDI1PL3Wpuc+bKnNZ0rbAoGBAKrPoiovZl2WGZ2ZoHsj\nTeQu2DpD/3DZszsZg4N3XVEVvrGBNJYmLYs4Lg5htLFQsv03qmD5tvfWRPZ0J2x+\nWgPiB+0yFvrnRSVFtKZpkWaOfl6WPo+MQpGVbZRmLDu/s3pm6Nkd7PNFko7j4PRm\n8lsC+ZSSDfoZSiHoZ0fB281F\n-----END PRIVATE KEY-----\n"
            };

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(cr.client_email)
               {
                   Scopes = Scopes
               }.FromPrivateKey(cr.private_key));

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            var spreadsheetId = "1n8GA7-N8cxlBgVohTBJEFl4DCa2reEgSyeJjjJ9g6CM";

            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>()
            };

            valueRange.Values.Add(new List<object> { order.Id, order.Name, order.Email, order.Amount, order.TimeArrival, order.TakeAway ? "Take away" : "" });
            order.Choices.ForEach(x => valueRange.Values.Add(new List<object> { "", x.Type.ToString(), x.Kids ? "Kids" : "", x.Amount }));

            var request = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, "A1");
            request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

            try
            {
                var response = request.Execute();
                return true;
            }
            catch (Exception e)
            {
                var t = e;
                return false;
            }
        }
    }
}