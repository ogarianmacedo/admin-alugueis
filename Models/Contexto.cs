using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoAlugar.Mapeamento;

namespace ProjetoAlugar.Models
{
    public class Contexto : IdentityDbContext<Usuario, NivelAcesso, string>
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<NivelAcesso> NiveisAcessos { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Carro> Carros { get; set; }
        public DbSet<Aluguel> Alugueis { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UsuarioMap());
            builder.ApplyConfiguration(new NivelAcessoMap());
            builder.ApplyConfiguration(new ContaMap());
            builder.ApplyConfiguration(new EnderecoMap());
            builder.ApplyConfiguration(new CarroMap());
            builder.ApplyConfiguration(new AluguelMap());
        }
    }
}
