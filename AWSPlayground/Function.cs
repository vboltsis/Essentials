using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSPlayground;

public class Function
{
    private static readonly AmazonDynamoDBClient _client =
        new AmazonDynamoDBClient("AccessKey", "SecretKey",
            new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
        });

    private static readonly Table _table = new TableBuilder(_client, "Products")
        .AddHashKey("ProductId", DynamoDBEntryType.String)
        .Build();

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request)
    {
        Console.WriteLine("FunctionHandler called");

        var product = JsonSerializer.Deserialize<Product>(request.Body);

        if (product == null)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 400,
                Body = "Invalid product data."
            };
        }

        var document = new Document
        {
            ["ProductId"] = product.ProductId,
            ["Name"] = product.Name,
            ["Price"] = product.Price
        };

        await _table.PutItemAsync(document);

        Console.WriteLine("Product added successfully!");

        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Body = "Product added successfully!"
        };
    }
}

public class Product
{
    public string ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}
