using System.ComponentModel.DataAnnotations;

public class CriarNotaFiscalItemDto
{
  [Required]
  public string? Codigo { get; set; }

  [Required]
  [Range(1, int.MaxValue, ErrorMessage = "Cada item deve ter uma quantidade de ao menos um")]
  public int? Quantidade { get; set; }
}

public class CriarNotaFiscalDto
{
  [Required]
  [MinLength(1, ErrorMessage = "A nota fiscal deve conter ao menos um item")]
  public List<CriarNotaFiscalItemDto>? Produtos { get; set; }
}

public class AtualizarStatusDto
{
  [Required]
  public string Status { get; set; }
}