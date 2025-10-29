using System.Text.Json.Serialization;

namespace Menza.WebApi.Models.OsijekMenze
{
    public class Jelo
    {
        [JsonPropertyName("sifra")]
        public string Sifra { get; set; }

        [JsonPropertyName("naziv")]
        public string Naziv { get; set; }

        [JsonPropertyName("grupa")]
        public string Grupa { get; set; }

        [JsonPropertyName("jm")]
        public string Jm { get; set; }

        [JsonPropertyName("cijena")]
        public decimal Cijena { get; set; }

        [JsonPropertyName("stud_cijena")]
        public decimal StudCijena { get; set; }

        [JsonPropertyName("posto_sub")]
        public decimal PostoSub { get; set; }

        [JsonPropertyName("datum")]
        public string Datum { get; set; }
    }

    public class GrupaJela
    {
        [JsonPropertyName("naziv_grupe")]
        public string NazivGrupe { get; set; }

        [JsonPropertyName("jela_u_grupi")]
        public List<Jelo> JelaUGrupi { get; set; }
    }
}
