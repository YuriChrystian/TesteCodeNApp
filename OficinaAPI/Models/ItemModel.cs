using System.ComponentModel.DataAnnotations;

namespace OficinaAPI.Models
{
    public class ItemModel
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal Subtotal => Quantidade * ValorUnitario;

        public int OrcamentoModelId { get; set; }

    }
}
