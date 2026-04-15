using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class NotaFiscalItem
{
  public string? Codigo { get; set; }

  public string? Descricao { get; set; }

  public int? Quantidade { get; set; }
}

public class NotaFiscal
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Numeracao { get; set; }

  public string? Status { get; set; }

  public List<NotaFiscalItem>? Produtos { get; set; }
}