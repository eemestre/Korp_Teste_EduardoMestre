using DnsClient.Protocol;
using MongoDB.Driver;

public class NotaFiscalService
{
  private readonly IMongoCollection<NotaFiscal> _colletcion;
  private readonly HttpClient _httpClient;

  public NotaFiscalService(IMongoDatabase database, HttpClient httpClient)
  {
    _colletcion = database.GetCollection<NotaFiscal>("notas");
    _httpClient = httpClient;
  }

  public async Task<List<NotaFiscal>> GetAll()
  {
    return await _colletcion.Find(_ => true).ToListAsync();
  }

  public async Task<NotaFiscal> GetByNumeracao(string numeracao)
  {
    return await _colletcion.Find(n => n.Numeracao == numeracao).FirstOrDefaultAsync();
  }

  public async Task<NotaFiscal> Create(CriarNotaFiscalDto dto)
  {
    var itens = new List<NotaFiscalItem>();

    foreach (var p in dto.Produtos)
    {
      var response = await _httpClient.GetAsync($"http://localhost:5095/produtos/{p.Codigo}");
      if (!response.IsSuccessStatusCode)
        throw new Exception($"Produto {p.Codigo} não existe");

      var content = await response.Content.ReadFromJsonAsync<ProdutoResponseDto>();

      var added = false;
      foreach (var i in itens)
      {
        if (i.Codigo == p.Codigo)
        {
          i.Quantidade += +p.Quantidade;
          added = true;

          if (content.Saldo < i.Quantidade)
            throw new Exception($"Produto {content.Codigo} tem apenas {content.Saldo} unidade(s) restante(s)");

          break;
        }
      }

      if (!added)
      {
        if (content.Saldo < p.Quantidade)
          throw new Exception($"Produto {content.Codigo} tem {content.Saldo} unidade(s) restante(s)");

        itens.Add(new NotaFiscalItem
        {
          Codigo = content.Codigo,
          Descricao = content.Descricao,
          Quantidade = p.Quantidade
        });
      }
    }

    foreach (var i in itens)
    {
      var response = await _httpClient.PutAsJsonAsync($"http://localhost:5095/produtos/{i.Codigo}", new
      {
        Quantidade = -i.Quantidade
      });

      if (!response.IsSuccessStatusCode)
      {
        throw new Exception($"Erro ao atualizar o produto {i.Codigo}");
      }
    }

    var nota = new NotaFiscal
    {
      Status = "Aberta",
      Produtos = itens
    };

    await _colletcion.InsertOneAsync(nota);

    return nota;
  }

  public async Task Delete(string numeracao)
  {
    await _colletcion.DeleteOneAsync(n => n.Numeracao == numeracao);
  }

  public async Task<bool> AtualizarStatus(string numeracao, string status)
  {
    if (status != "Aberta" && status != "Fechada")
      throw new Exception($"{status} não é um status válido: 'Aberta' | 'Fechada'");

    var filter = Builders<NotaFiscal>.Filter.Eq(n => n.Numeracao, numeracao);

    var update = Builders<NotaFiscal>.Update
        .Set(n => n.Status, status);

    var result = await _colletcion.UpdateOneAsync(filter, update);

    return result.ModifiedCount > 0;
  }
}