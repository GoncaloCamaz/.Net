namespace Portelinha.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Fatura")]
    public partial class Fatura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fatura()
        {
            Servico = new HashSet<Servico>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Localidade { get; set; }

        [Required]
        [StringLength(30)]
        public string CP { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Rua { get; set; }

        public int Nr { get; set; }

        public int NIF { get; set; }

        [StringLength(30)]
        public string Apartamento { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome_Cliente { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome_Pack { get; set; }

        public int Id_Pack { get; set; }

        [Column(TypeName = "money")]
        public decimal Preco { get; set; }

        public DateTime Data { get; set; }

        public virtual CP_Localidade CP_Localidade { get; set; }

        public Fatura(int id)
        {
            this.Id = id;
            this.Localidade = "";
            this.CP = "";
            this.Rua = "";
            this.Nr = 0;
            this.NIF = 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servico> Servico { get; set; }
    }
}
