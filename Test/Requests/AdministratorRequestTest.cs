using System.Net;
using System.Text;
using System.Text.Json;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.ModelViews;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public sealed class AdministratorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContest)
    {
        Setup.ClassInit(testContest);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }
    
    [TestMethod]
    public async Task GetSetPropsTest()
    {
        // Arrange
        var loginDto = new LoginDTO
        {
            Email = "adm@test.com",
            Senha = "123456"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "Application/json");

        // Action
        var response = await Setup.client.PostAsync("administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministratorLoggedIn>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Perfil ?? "");
        Assert.IsNotNull(admLogado?.Token ?? "");

        Console.WriteLine(admLogado?.Token);
    }
}