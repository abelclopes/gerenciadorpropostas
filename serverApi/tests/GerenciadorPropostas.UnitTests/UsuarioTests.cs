using DOMAIN;
using Xunit;

namespace GerenciadorPropostas.UnitTests;

public class UsuarioTests
{
    [Fact]
    public void Constructor_ComSenha_DeveArmazenarHashArgon2()
    {
        Environment.SetEnvironmentVariable("PAPER_SECRETY", "pepper-test");

        var usuario = new Usuario(
            nome: "Abel",
            email: "abell@gmail.com",
            cpf: "12345678900",
            dataNacimento: new DateTime(1990, 1, 1),
            senha: "teste123"
        );

        Assert.StartsWith("$argon2id$", usuario.Senha);
        Assert.True(Util.VerifyPassword("teste123", usuario.Senha));
    }
}
