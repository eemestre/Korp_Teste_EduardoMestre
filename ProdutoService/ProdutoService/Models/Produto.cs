using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Produto
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Codigo { get; set; }

  public string? Descricao { get; set; }

  public int? Saldo { get; set; }
}