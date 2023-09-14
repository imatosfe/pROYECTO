namespace pROYECTO.Models
{
    public class ConsultaResponse
    {
        public required string ItemInternalId { get; set; }
        public required int TipoIdentifica { get; set; }
        public required string Identifica { get; set; }
        public required   string Nombre { get; set; }
        public required  string email { get; set; }
        public required string Movil { get; set; }
        public required string EntidTipo { get; set; }
        public required int EntidCodigo { get; set; }
    }
}
