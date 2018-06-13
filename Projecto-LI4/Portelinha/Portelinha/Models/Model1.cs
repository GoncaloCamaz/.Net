namespace Portelinha.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=DataBase_LI4")
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<CP_Localidade> CP_Localidade { get; set; }
        public virtual DbSet<Fatura> Fatura { get; set; }
        public virtual DbSet<Funcionario> Funcionario { get; set; }
        public virtual DbSet<Pack> Pack { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Servico> Servico { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.CP)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Apartamento)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Rua)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Pass_word)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Servico)
                .WithRequired(e => e.Cliente)
                .HasForeignKey(e => e.Id_Cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CP_Localidade>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<CP_Localidade>()
                .Property(e => e.Localidade)
                .IsUnicode(false);

            modelBuilder.Entity<CP_Localidade>()
                .HasMany(e => e.Cliente)
                .WithRequired(e => e.CP_Localidade)
                .HasForeignKey(e => e.CP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CP_Localidade>()
                .HasMany(e => e.Fatura)
                .WithRequired(e => e.CP_Localidade)
                .HasForeignKey(e => e.CP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CP_Localidade>()
                .HasMany(e => e.Funcionario)
                .WithRequired(e => e.CP_Localidade)
                .HasForeignKey(e => e.CP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CP_Localidade>()
                .HasMany(e => e.Servico)
                .WithRequired(e => e.CP_Localidade)
                .HasForeignKey(e => e.CP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.Localidade)
                .IsUnicode(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.CP)
                .IsUnicode(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.Rua)
                .IsUnicode(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.Apartamento)
                .IsUnicode(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.Nome_Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.Nome_Pack)
                .IsUnicode(false);

            modelBuilder.Entity<Fatura>()
                .Property(e => e.Preco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Fatura>()
                .HasMany(e => e.Servico)
                .WithRequired(e => e.Fatura)
                .HasForeignKey(e => e.Id_Fatura)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.Rua)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.CP)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.Apartamento)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.Pass_word)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<Funcionario>()
                .HasMany(e => e.Servico)
                .WithMany(e => e.Funcionario)
                .Map(m => m.ToTable("Servico_Funcionario").MapLeftKey("Id_Funcionario").MapRightKey("Id_Servico"));

            modelBuilder.Entity<Pack>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Pack>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Pack>()
                .Property(e => e.Preco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Pack>()
                .HasMany(e => e.Produto)
                .WithRequired(e => e.Pack)
                .HasForeignKey(e => e.Id_Pack)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pack>()
                .HasMany(e => e.Servico)
                .WithRequired(e => e.Pack)
                .HasForeignKey(e => e.Id_Pack)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Produto>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Produto>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.Preco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Servico>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.Apartamento)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.CP)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.Rua)
                .IsUnicode(false);

            modelBuilder.Entity<Servico>()
                .Property(e => e.Comentario)
                .IsUnicode(false);
        }
    }
}
