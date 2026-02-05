using System.Xml.Serialization;

[XmlRoot("ValCurs")]
public class ValCurs
{
    [XmlElement("Valute")]
    public List<Valute> Valutes { get; set; } = new();
}

public class Valute
{
    [XmlElement("CharCode")]
    public string CharCode { get; set; } = null!;

    [XmlElement("Value")]
    public string Value { get; set; } = null!;
}
