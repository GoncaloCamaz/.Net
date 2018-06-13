namespace Portelinha.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Servico = new HashSet<Servico>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
       // [RegularExpression("^[0-9]{9}", ErrorMessage = "NIF apenas contém números.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50)]
        //[RegularExpression("[a-zA-ZÁÂÃÀÇÉÊÍÓÔÕÚÜáâãàçéêíóôõúü ]+", ErrorMessage = "Insira apenas caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50)]
        //[RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)", ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        //[RegularExpression("^[0-9]{9}", ErrorMessage = "Insira apenas números.")]
        public int Telemovel { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(30)]
        //[RegularExpression("^\\d{4}", ErrorMessage = "Código postal inválido.")]
        public string CP { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        //[RegularExpression("^[0-9]+", ErrorMessage = "Insira apenas números.")]
        public int Nr { get; set; }

        [StringLength(30)]
        public string Apartamento { get; set; }

        [Column(TypeName = "text")]
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Insira uma Password.")]
        [StringLength(50)]
        public string Pass_word { get; set; }

        [Required] 
        [StringLength(10)]
        public string Role { get; set; }

        public class ClienteContext : DbContext
        {
            public DbSet<Cliente> clientes { get; set; }
        }

        public void setRole()
        {
            this.Role = "user";
        }

        public Cliente(int id, string nome, string email, int telm, string cp, int num, string ap, string rua, string pass)
        {
            this.Id = id;
            this.Nome = nome;
            this.Email = email;
            this.Telemovel = telm;
            this.CP = cp;
            this.Nr = num;
            this.Apartamento = ap;
            this.Rua = rua;
            this.Pass_word = pass;
            this.Role = "user";
        }

        public Cliente(Cliente u)
        {

            this.Id = u.getId();
            this.Nome = u.getNome();
            this.Email = u.getEmail();
            this.Telemovel = u.getTelm();
            this.Rua = u.getRua();
            this.CP = u.getCp();
            this.Nr = u.getNr();
            this.Apartamento = u.getApartamento();
            this.Pass_word = u.getPassword();
        }
        //////////////////////////////////// Métodos Get e Set ////////////////////////////////////////////////

        public int getId()
        {
            return this.Id;
        }

        public string getNome()
        {
            return this.Nome;
        }

        public string getEmail()
        {
            return this.Email;
        }

        public int getTelm()
        {
            return this.Telemovel;
        }

        public string getCp()
        {
            return this.CP;
        }

        public string getRua()
        {
            return this.Rua;
        }

        public int getNr()
        {
            return this.Nr;
        }

        public string getApartamento()
        {
            return this.Apartamento;
        }

        public string getPassword()
        {
            return this.Pass_word;
        }

        public string getRole()
        {
            return this.Role;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////

        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servico> Servico { get; set; }

        public virtual CP_Localidade CP_Localidade { get; set; }
    }
}
