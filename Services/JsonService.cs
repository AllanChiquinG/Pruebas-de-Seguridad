using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VulnerableApi.Services;

public class JsonService
{
    public T? DeserializeObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public string SerializeObject(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public bool ValidateWithRegex(string input)
    {
        var pattern = @"^(a|a)+$";
        return Regex.IsMatch(input, pattern);
    }

    public string FormatXml(string xml)
    {
        var doc = new System.Xml.XmlDocument();
        doc.LoadXml(xml);
        return doc.OuterXml;
    }
}
