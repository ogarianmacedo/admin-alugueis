using Moq;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ProjetoAlugarTest
{
    public class EnderecoTest
    {
        [Fact]
        public void Inserir()
        {
            var mock = new Mock<IEndereco>();
            mock.Setup(e => e.Inserir(It.IsAny<Endereco>()));

            Endereco endereco = new Endereco
            {
                EnderecoId = 1,
                Rua = "Abc",
                Numero = 1,
                Bairro = "Abc",
                Cidade = "Abc",
                Estado = "Abc"
            };

            mock.Object.Inserir(endereco);
            mock.Verify(e => e.Inserir(It.IsAny<Endereco>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Atualizar()
        {
            var mock = new Mock<IEndereco>();
            mock.Setup(e => e.Atualizar(It.IsAny<Endereco>()));

            Endereco endereco = new Endereco
            {
                EnderecoId = 1,
                Rua = "Abc",
                Numero = 1,
                Bairro = "Abc",
                Cidade = "Abc",
                Estado = "Abc"
            };

            mock.Object.Atualizar(endereco);
            mock.Verify(e => e.Atualizar(It.IsAny<Endereco>()), Times.AtLeast(1));
        }

        [Fact]
        public void Excluir()
        {
            var mock = new Mock<IEndereco>();
            mock.Setup(e => e.Excluir(It.IsAny<int>()));
            mock.Object.Excluir(2);
            mock.Verify(e => e.Excluir(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void BuscarPeloId()
        {
            var mock = new Mock<IEndereco>();
            mock.Setup(e => e.BuscarPeloId(It.IsAny<int>()));
            mock.Object.BuscarPeloId(1);
            mock.Object.BuscarPeloId(2);
            mock.Object.BuscarPeloId(3);
            mock.Object.BuscarPeloId(4);

            mock.Verify(e => e.BuscarPeloId(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public void BuscarTodos()
        {
            var mock = new Mock<IEndereco>();
            mock.Setup(e => e.BuscarTodos());
            var enderecos = mock.Object.BuscarTodos();
            IQueryable<Endereco> end = null;

            Assert.Equal<Endereco>(end, enderecos);
        }
    }
}
