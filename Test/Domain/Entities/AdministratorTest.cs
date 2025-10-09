using minimal_api.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public sealed class AdministratorTest
{
    [TestMethod]
    public void GetSetPropsTest()
    {
        // Arrange
        var adm = new Administrator();
        
        // Action
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Senha = "123456";
        adm.Perfil = "Adm";

        // Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("teste@teste.com", adm.Email);
        Assert.AreEqual("123456", adm.Senha);
        Assert.AreEqual("Adm", adm.Perfil);
    }
}