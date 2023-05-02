namespace UrbanFiesta.Filters.Extensions
{
    public class Functions
    {
        public static string IdFrom(string pathToId, IDictionary<string, object?> arguments)
        {
            var id = string.Empty;
            if (pathToId.Contains('.'))
            {
                var properties = pathToId.Split('.');
                var obj = arguments[properties[0]];
                if (obj!.GetType().GetProperty(properties[1]) != null)
                    id = obj.GetType().GetProperty(properties[1])!.GetValue(obj)!.ToString();
            }
            else
                id = arguments[pathToId]!.ToString();

            return id!;
        }
    }
}
