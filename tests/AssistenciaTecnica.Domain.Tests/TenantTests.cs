using AssistenciaTecnica.Domain.Entities;

namespace AssistenciaTecnica.Domain.Tests
{
    public class TenantTests
    {
        [Fact]
        public void CriarTenant_QuandoDadosValidos_DeveCriarTenant()
        {
            string codigo = "techrepair";
            string nome = "TechRepair";

            var tenant = CriarTenant(codigo, nome);

            Assert.NotEqual(Guid.Empty, tenant.Id);
            Assert.Equal(nome, tenant.Nome);
            Assert.Equal(codigo, tenant.Codigo);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CriarTenant_QuandoCodigoNaoInformado_DeveLancarArgumentException(string? codigo)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _ = new Tenant(codigo!, "TechRepair");
            });

            Assert.Equal("codigo", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CriarTenant_QuandoNomeNaoInformado_DeveLancarArgumentException(string? nome)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _ = new Tenant("techrepair", nome!);
            });

            Assert.Equal("nome", exception.ParamName);
        }

        private static Tenant CriarTenant(string codigo, string nome)
        {
            return new Tenant(codigo, nome);
        }
    }
}