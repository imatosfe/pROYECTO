namespace pROYECTO.Models
{
   

    public class ConsultaRequest
    {
        public required Parametros param { get; set; }
    }

    public class Parametros
    {
        public required string SysticKey { get; set; }
        public required string SysticHash { get; set; }
        public required string TipoIdentifica { get; set; }
        public required string Identifica { get; set; }
    }

}
