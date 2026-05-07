namespace OficinaAPI.Models
{
    public class ItemModel
    {
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal Subtotal => Quantidade * ValorUnitario;

    }
}
