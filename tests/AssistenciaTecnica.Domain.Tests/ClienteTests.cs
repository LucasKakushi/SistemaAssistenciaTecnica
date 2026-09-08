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

namespace AssistenciaTecnica.Domain.Tests;

public class ClienteTests
{
    [Fact]
    public void CriarCliente_QuandoDadosValidos_DeveCriarCliente()
    {
        var cliente = CriarCliente();

        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal("Empresa Exemplo Ltda.", cliente.RazaoSocial);
        Assert.Equal("12345678901234", cliente.Cnpj);

    }

    private static Cliente CriarCliente(string razaoSocial = "Empresa Exemplo Ltda.", string cnpj = "12345678901234")
    {
        return new Cliente(razaoSocial, cnpj);
    }
}
