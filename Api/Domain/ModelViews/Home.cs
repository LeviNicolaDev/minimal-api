namespace minimal_api.Domain.ModelViews;

public struct Home
{
    public string Mensagem
    {
        get => "Bem vindo(a) a API de veículos - Minimal API";
    }
    public string Doc
    {
        get => "/swagger";
    }
}