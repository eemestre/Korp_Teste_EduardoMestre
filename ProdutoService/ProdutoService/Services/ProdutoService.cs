using MongoDB.Driver;

public class ProdutoService
{
  private readonly IMongoCollection<Produto> _collection;

  public ProdutoService(IMongoDatabase database)
  {
    _collection = database.GetCollection<Produto>("produtos");
  }

  public async Task<Produto> GetByCodigo(string codigo)
  {
    return await _collection
            .Find(p => p.Codigo == codigo)
            .FirstOrDefaultAsync();
  }

  public async Task<List<Produto>> GetAll()
  {
    return await _collection.Find(_ => true).ToListAsync();
  }

  public async Task<Produto> Create(CriarProdutoDto dto)
  {
    var produto = new Produto
    {
      Descricao = dto.Descricao,
      Saldo = dto.Saldo
    };

    await _collection.InsertOneAsync(produto);

    return produto;
  }

  public async Task Delete(string codigo)
  {
    await _collection.DeleteOneAsync(p => p.Codigo == codigo);
  }

  public async Task<bool> AtualizarSaldo(string codigo, int quantidade)
  {
    var filter = Builders<Produto>.Filter.Eq(p => p.Codigo, codigo);

    var update = Builders<Produto>.Update
        .Inc(p => p.Saldo, quantidade);

    var result = await _collection.UpdateOneAsync(filter, update);

    return result.ModifiedCount > 0;
  }
}