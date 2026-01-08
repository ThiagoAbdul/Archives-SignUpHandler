using Amazon.Lambda.CognitoEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using System.Text.Json.Serialization;

namespace SignUpHandler;

public class Function
{
    private static async Task Main()
    {
        var handler = FunctionHandler;
        await LambdaBootstrapBuilder.Create(handler, new SourceGeneratorLambdaJsonSerializer<LambdaFunctionJsonSerializerContext>())
            .Build()
            .RunAsync();
    }

    public static CognitoPreSignupEvent FunctionHandler(CognitoPreSignupEvent ev, ILambdaContext context)
    {
        var email = ev.Request.UserAttributes["email"];

        IEnumerable<string> allowedEmails = Environment.GetEnvironmentVariable("ALLOWED_EMAILS")?.Split(',')
            ?? [ "thiagoabdul10@gmail.com" ];
        

        if (!allowedEmails.Contains(email))
        {
            throw new Exception("E-mail não permitido.");
        }

        ev.Response.AutoConfirmUser = true;
        ev.Response.AutoVerifyEmail = true;

        return ev;
    }
}


[JsonSerializable(typeof(CognitoPreSignupEvent))]
[JsonSerializable(typeof(CognitoPreSignupResponse))]
public partial class LambdaFunctionJsonSerializerContext : JsonSerializerContext {}