using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("notas")]
public class NotaFiscalController : ControllerBase
{
  private readonly NotaFiscalService _service;

  public NotaFiscalController(NotaFiscalService service)
  {
    _service = service;
  }

  [HttpGet("{numeracao}")]
  public async Task<IActionResult> Get(string numeracao)
  {
    var notaFiscal = await _service.GetByNumeracao(numeracao);

    if (notaFiscal == null)
      return NotFound();

    return Ok(notaFiscal);
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var notas = await _service.GetAll();

    if (notas == null)
      return NotFound();

    return Ok(notas);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CriarNotaFiscalDto dto)
  {
    try
    {
      var nota = await _service.Create(dto);
      return CreatedAtAction(nameof(Get), new { numeracao = nota.Numeracao }, nota);
    }
    catch (Exception e)
    {
      return NotFound(e.Message);
    }
  }

  [HttpPut("{numeracao}")]
  public async Task<IActionResult> AtualizarStatus(string numeracao, [FromBody] AtualizarStatusDto dto)
  {
    try
    {
      var result = await _service.AtualizarStatus(numeracao, dto.Status);

      if (!result)
        return NotFound();

      return NoContent();
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }

  }
}