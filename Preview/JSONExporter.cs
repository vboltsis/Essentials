namespace Preview;

//var options = new JsonSchemaExporterOptions
//{
//    OnSchemaNodeGenerated = static (ctx, schema) =>
//    {
//        DescriptionAttribute? descriptionAttribute = ctx.PropertyInfo?.AttributeProvider?
//            .GetCustomAttribute<DescriptionAttribute>();

//        if (descriptionAttribute != null)
//        {
//            schema["description"] = (JsonNode)descriptionAttribute.Description;
//        }
//    }
//};

//JsonNode schemaNode = JsonSerializerOptions.Default.GetJsonSchemaAsNode(typeof(Developer), options);
//// { "type" : "object", "properties" : { "Bugs" : { "type" : "integer", "description" : "How many bugs have been produced" } } }

//public class Developer
//{
//    [Description("How many bugs have been produced")]
//    public int Bugs { get; set; }
//}