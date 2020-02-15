using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Mapeamento
{
    public class NivelAcessoMap : IEntityTypeConfiguration<NivelAcesso>
    {
        public void Configure(EntityTypeBuilder<NivelAcesso> builder)
        {
            builder.Property(n => n.Descricao).IsRequired().HasMaxLength(400);
            builder.ToTable("NiveisAcessos");
        }
    }
}
