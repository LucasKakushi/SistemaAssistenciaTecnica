/*
teste 1

Nome:
CriarCliente_QuandoDadosValidos_DeveCriarCliente

Arrange:
razão social = "Empresa Exemplo Ltda."
cnpj = ""04252011000110""

Act:
criar Cliente

Assert:
Id != Guid.Empty
RazaoSocial == "Empresa Exemplo Ltda."
Cnpj == ""04252011000110""

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
        Assert.Equal("04252011000110", cliente.Cnpj);

    }

    [Fact]
    public void CriarCliente_QuandoTenantIdVazio_DeveLancarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.Empty,
                "Empresa Exemplo Ltda.",
                "04252011000110"
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
                "04252011000110"
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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CriarCliente_QuandoCnpjNaoInformado_DeveLancarArgumentException(string? cnpj)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                cnpj!
            );
        });

        Assert.Equal("cnpj", exception.ParamName);
    }

    [Fact]
    public void CriarCliente_QuandoCnpjFormatado_DeveNormalizarCnpj()
    {
        string cnpj = "04.252.011/0001-10";
        string cnpjEsperado = "04252011000110";
        var cliente = new Cliente(Guid.NewGuid(), "Empresa Exemplo Ltda.", cnpj);

        Assert.Equal(cnpjEsperado, cliente.Cnpj);

    }

    [Fact]
    public void CriarCliente_QuandoCnpjContemApenasPontuacao_DeveRetornarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                "./-"
            );
        });

        Assert.Equal("cnpj", exception.ParamName);

    }

    [Theory]
    [InlineData("123")]
    [InlineData("123456789012345")]
    public void CriarCliente_QuandoCnpjTemTamanhoInvalido_DeveLancarArgumentException(string cnpj)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                cnpj
            );
        });

        Assert.Equal("cnpj", exception.ParamName);
    }

    [Fact]
    public void CriarCliente_QuandoCnpjContemCaractereNaoPermitido_DeveRetornarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                "12@45678901234"
            );
        });

        Assert.Equal("cnpj", exception.ParamName);

    }

    [Theory]
    [InlineData("AB1234567890A1")]
    [InlineData("AB12345678901A")]
    public void CriarCliente_QuandoDigitosVerificadoresContemLetra_DeveLancarArgumentException(string cnpj)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                cnpj
            );
        });

        Assert.Equal("cnpj", exception.ParamName);

    }

    [Fact]
    public void CriarCliente_QuandoPrimeiroDigitoVerificadorIncorreto_DeveLancarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                "04252011000120"
            );
        });

        Assert.Equal("cnpj", exception.ParamName);
    }

    [Fact]
    public void CriarCliente_QuandoSegundoDigitoVerificadorIncorreto_DeveLancarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                "04252011000111"
            );
        });

        Assert.Equal("cnpj", exception.ParamName);
    }

    [Fact]
    public void CriarCliente_QuandoTodosOsDigitosSaoZero_DeveLancarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                "00000000000000"
            );
        });
    }

    [Theory]
    [InlineData("12.ABC.345/01DE-35")]
    [InlineData("12.abc.345/01de-35")]
    public void CriarCliente_QuandoAlfanumerico_DeveNormalizarParaMaiusculo(string cnpj)
    {
        var cliente = new Cliente(Guid.NewGuid(), "Empresa Exemplo Ltda.", cnpj);
        Assert.Equal("12ABC34501DE35", cliente.Cnpj);
    }

    [Fact]
    public void CriarCliente_QuandoCnpjAlfanumericoTemSegundoDigitoIncorreto_DeveLancarArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 
        {
            _ = new Cliente(
                Guid.NewGuid(),
                "Empresa Exemplo Ltda.",
                "12ABC34501DE36"
            );
        });
    }

    private static Cliente CriarCliente(Guid tenantId, string razaoSocial = "Empresa Exemplo Ltda.", string cnpj = "04252011000110")
    {
        return new Cliente(tenantId, razaoSocial, cnpj);
    }
}
