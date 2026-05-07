namespace OficinaAPI.Models
{
    public class OrcamentoModel
    {
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }
        public List<ItemModel> Itens { get; set; } = new();
        public decimal ValorTotal => Itens.Sum( x => x.Subtotal);
    }
}
