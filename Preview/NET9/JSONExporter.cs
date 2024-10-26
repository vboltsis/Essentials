using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace Preview;

public class JsonSchemaExample
{
    public void Example()
    {
        var options = new JsonSchemaExporterOptions
        {
            TransformSchemaNode = (context, node) =>
            {
                if (context.PropertyInfo != null)
                {
                    var descriptionAttribute = context.PropertyInfo.AttributeProvider
                        .GetCustomAttributes(false)
                        .OfType<DescriptionAttribute>().FirstOrDefault();

                    if (descriptionAttribute != null)
                    {
                        if (node is JsonObject propertySchema)
                        {
                            propertySchema["description"] = descriptionAttribute.Description;
                        }
                    }
                }

                return node;
            }
        };

        JsonNode schemaNode = JsonSerializerOptions.Default.GetJsonSchemaAsNode(typeof(Developer), options);
        /* { "type" : "object", "properties" : { "Bugs" :
            { "type" : "integer", "description" : "How many bugs have been produced" } } }
        */
    }
}


public class Developer
{
    [Description("How many bugs have been produced")]
    public int Bugs { get; set; }
}