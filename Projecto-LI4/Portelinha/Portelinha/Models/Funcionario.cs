namespace Portelinha.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;

    [Table("Funcionario")]
    public partial class Funcionario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Funcionario()
        {
            Servico = new HashSet<Servico>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "NIF apenas contém números.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z0-9ÁÂÃÀÇÉÊÍÓÔÕÚÜáâãàçéêíóôõúü-\\s]+$", ErrorMessage = "Insira apenas caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression("^[0-9]{9}", ErrorMessage = "Insira apenas números.")]
        public int Telemovel { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(30)]
        [RegularExpression("^\\d{4}$", ErrorMessage = "Código postal inválido.")]
        public string CP { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [RegularExpression("^[0-9]{5}", ErrorMessage = "Insira apenas números.")]
        public int Nr { get; set; }

        [StringLength(30)]
        public string Apartamento { get; set; }

        [Column(TypeName = "text")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Insira uma Password.")]
        [StringLength(50)]
        public string Pass_word { get; set; }

        [Required] // não deve aparecer
        [StringLength(10)]
        public string Role { get; set; }

        public class FuncionarioContext : DbContext
        {
            public DbSet<Funcionario> funcionarios { get; set; }
        }

        public void setRole()
        {
            this.Role = "func";
        }

        public void setRoleAdmin()
        {
            this.Role = "admin";
        }
        
        public virtual CP_Localidade CP_Localidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servico> Servico { get; set; }
    }
}
