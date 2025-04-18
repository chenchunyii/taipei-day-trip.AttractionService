using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace taipei_day_trip_dotnet.Models
{
    public class TaipeiModels
    {
        [Table("webpage")]
        public class Webpage
        {
            [Key]
            [Column("ID")]
            public long Id { get; set; }

            [Required]
            [Column("NAME")]
            public string Name { get; set; } = null!;

            [Required]
            [Column("CATEGORY")]
            public string Category { get; set; } = null!;

            [Required]
            [Column("DESCRIPTION")]
            public string Description { get; set; } = null!;

            [Required]
            [Column("ADDRESS")]
            public string Address { get; set; } = null!;

            [Required]
            [Column("TRANSPORT")]
            public string Transport { get; set; } = null!;

            [Column("MRT")]
            public string? MRT { get; set; }

            [Column("LAT")]
            public double Latitude { get; set; }

            [Column("LNG")]
            public double Longitude { get; set; }

            [Column("IMAGES")]
            public string? Images { get; set; } // 可以考慮後續轉成 List<string>
        }

    }
}