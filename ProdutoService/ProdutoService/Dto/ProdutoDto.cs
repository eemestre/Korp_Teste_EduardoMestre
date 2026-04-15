using System.ComponentModel.DataAnnotations;

public class CriarProdutoDto
{
  [Required]
  public string? Descricao { get; set; }

  [Required]
  [Range(0, int.MaxValue, ErrorMessage = "O saldo deve ser maior ou igual a 0")]
  public int? Saldo { get; set; }
}

public class AtualizarSaldoDto
{
  [Required]
  public int Quantidade { get; set; }
}