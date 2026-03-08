using DOMAIN;
using Xunit;

namespace GerenciadorPropostas.UnitTests;

public class UtilPasswordTests
{
    [Fact]
    public void HashPassword_GeraHashArgon2_E_VerificaComSucesso()
    {
        Environment.SetEnvironmentVariable("PAPER_SECRETY", "pepper-test");

        var senha = "teste123";
        var hash = Util.HashPassword(senha);

        Assert.StartsWith("$argon2id$", hash);
        Assert.True(Util.VerifyPassword(senha, hash));
        Assert.False(Util.VerifyPassword("senha-errada", hash));
    }

    [Fact]
    public void ValidateSHA1HashData_ValidaHashLegadoConhecido()
    {
        const string hashLegado = "2242461295221015719538209212227614317113501631961762";
        Assert.True(Util.ValidateSHA1HashData("teste123", hashLegado));
    }
}
