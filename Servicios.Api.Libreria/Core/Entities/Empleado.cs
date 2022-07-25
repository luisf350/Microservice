using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.Api.Libreria.Core.Entities
{
    [BsonCollection("Empleado")]
    public class Empleado : Document
    {
        [BsonElement("nombre")]
        public string Nombre { get; set; }
    }
}
