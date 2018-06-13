namespace Portelinha.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Servico")]
    public partial class Servico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Servico()
        {
            Funcionario = new HashSet<Funcionario>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        [Column(TypeName = "money")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(15)]
        public string Estado { get; set; }

        [StringLength(30)]
        public string Apartamento { get; set; }

        [Required]
        [StringLength(30)]
        public string CP { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Rua { get; set; }

        [Column(TypeName = "text")]
        public string Comentario { get; set; }

        public int Nr { get; set; }

        public int Id_Cliente { get; set; }

        public int Id_Pack { get; set; }

        public int Id_Fatura { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual CP_Localidade CP_Localidade { get; set; }

        public virtual Fatura Fatura { get; set; }

        public virtual Pack Pack { get; set; }

        public Servico(int id, DateTime hora, decimal preco, string estado, string apartamento, string cp, string rua, int nr, int idCliente, int idPack, int fatura, string comentario)
        {
            this.Id = id;
            this.DataHora = hora;
            this.Preco = preco;
            this.Estado = estado;
            this.Apartamento = apartamento;
            this.CP = cp;
            this.Rua = rua;
            this.Nr = nr;
            this.Id_Cliente = idCliente;
            this.Id_Pack = idPack;
            this.Id_Fatura = fatura;
            this.Comentario = comentario;
        }

        public void SetEstadoCancalado()
        {
            this.Estado = "Cancelado";
        }

        public void SetEstadoCompleto()
        {
            this.Estado = "Completo";
        }

        public void SetComentario(string coment)
        {
            this.Comentario = coment;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funcionario> Funcionario { get; set; }
    }
}
