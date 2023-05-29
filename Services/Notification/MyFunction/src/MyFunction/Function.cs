using System.Text.Json.Serialization;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MyFunction;

public class RequestModel
{
    [JsonPropertyName("senderName")]
    public string SenderName { get; set; }
    [JsonPropertyName("receivers")]
    public IEnumerable<Message> Receivers { get; set; }
}

public class Message
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("receiverName")]
    public string ReceiverName { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }
    [JsonPropertyName("longidute")]
    public float Longidute { get; set; }
    [JsonPropertyName("latidute")]
    public float Latidute { get; set; }
    [JsonPropertyName("time")]
    public string Time { get; set; }
}


public class Function
{
    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<bool> FunctionHandler(RequestModel request, ILambdaContext context)
    {
        if (request == null)
        {
            throw new ArgumentNullException("invalid value.");
        }

        // Credentials
        var client = new AmazonSecretsManagerClient();
        var awsKeyId = await client.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
        {
            SecretId = "sns-key-id"
        });
        var awsKeySecret = await client.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
        {
            SecretId = "sns-key-secret"
        });

        var awsCredentials = new BasicAWSCredentials(awsKeyId.SecretString, awsKeySecret.SecretString);
        AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(awsCredentials,Amazon.RegionEndpoint.EUCentral1);

        PublishRequest publishRequest = new PublishRequest();

        if (!request.Receivers.Any())
        {
            throw new ArgumentNullException("Receivers must have one item at least!");
        }

        foreach (var i in request.Receivers)
        {
            publishRequest.Message = $"Sayın {i.ReceiverName}, bu mesaj {request.SenderName} tarafından acil durumlarda kullanılması için oluşturulmuş otomatik bir mesajdır. Acil durum listesinde olmanızdan ötürü {i.Time} tarihinde mesaj size gönderilmiştir.Konum: Latidute: {i.Latidute} - Longidute: {i.Longidute}";
            publishRequest.PhoneNumber = i.PhoneNumber;
            publishRequest.Subject = "ACİL YARDIM ÇAĞRISI";
            publishRequest.MessageAttributes.Add("AWS.SNS.SMS.SMSType", new MessageAttributeValue { StringValue = "Transactional", DataType = "String" });
            PublishResponse publishResponse = await snsClient.PublishAsync(publishRequest);
        }
        return true;
    }
}
