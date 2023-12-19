namespace YBS2.Service.Swaggers
{
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Struct |
        AttributeTargets.Enum |
        AttributeTargets.Property |
        AttributeTargets.Parameter
        , AllowMultiple = false)]
    public class SwaggerSchemaExampleAttribute : Attribute
    {
        public string Example { get; set; }

        public SwaggerSchemaExampleAttribute(string example)
        {
            Example = example;
        }
    }
}
