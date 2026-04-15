using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("produtos")]
public class ProdutosController : ControllerBase
{
  private readonly ProdutoService _service;

  public ProdutosController(ProdutoService service)
  {
    _service = service;
  }

  [HttpGet("{codigo}")]
  public async Task<IActionResult> Get(string codigo)
  {
    var produto = await _service.GetByCodigo(codigo);

    if (produto == null)
      return NotFound();

    return Ok(produto);
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var produtos = await _service.GetAll();

    if (produtos == null)
      return NotFound();

    return Ok(produtos);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CriarProdutoDto dto)
  {
    var produto = await _service.Create(dto);
    return CreatedAtAction(nameof(Get), new { codigo = produto.Codigo }, produto);
  }

  [HttpDelete("{codigo}")]
  public async Task<IActionResult> Delete(string codigo)
  {
    try
    {
      await _service.Delete(codigo);
      return NoContent();
    } catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPut("{codigo}")]
  public async Task<IActionResult> AtualizarSaldo(string codigo, [FromBody] AtualizarSaldoDto dto)
  {
    var result = await _service.AtualizarSaldo(codigo, dto.Quantidade);

    if (!result)
      return NotFound();

    return NoContent();
  }
}