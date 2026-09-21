/*
teste 1

Nome:
CriarCliente_QuandoDadosValidos_DeveCriarCliente

Arrange:
razão social = "Empresa Exemplo Ltda."
cnpj = "12345678901234"

Act:
criar Cliente

Assert:
Id != Guid.Empty
RazaoSocial == "Empresa Exemplo Ltda."
Cnpj == "12345678901234"

*/

using AssistenciaTecnica.Domain.Entities;
using Xunit.Sdk;

namespace AssistenciaTecnica.Domain.Tests;

public class ClienteTests
{
    [Fact]
    public void CriarCliente_QuandoDadosValidos_DeveCriarCliente()
    {
        var cliente = CriarCliente(Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal("Empresa Exemplo Ltda.", cliente.RazaoSocial);
        Assert.Equal("12345678901234", cliente.Cnpj);

    }

    [Fact]
    public void CriarCliente_QuandoTenantIdVazio_DeveLancarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.Empty,
                "Empresa Exemplo Ltda.",
                "12345678901234"
            );
        });

        Assert.Equal("tenantId", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CriarCliente_QuandoRazaoSocialInvalida_DeveLancarArgumentException(string? razaoSocial)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                razaoSocial!,
                "12345678901234"
            );
        });

        Assert.Equal("razaoSocial", exception.ParamName);
    }

    [Fact]
    public void CriarCliente_QuandoTenantIdValido_DevePreservarTenantId()
    {
        var tenantId = Guid.NewGuid();
        var cliente = CriarCliente(tenantId);

        Assert.Equal(tenantId, cliente.TenantId);
    }

    private static Cliente CriarCliente(Guid tenantId, string razaoSocial = "Empresa Exemplo Ltda.", string cnpj = "12345678901234")
    {
        return new Cliente(tenantId, razaoSocial, cnpj);
    }
}
