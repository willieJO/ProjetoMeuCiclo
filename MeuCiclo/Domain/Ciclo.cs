namespace MeuCiclo.Domain.Entities;

public class Ciclo
{
    public Guid Id { get; private set; }
    public DateTime Data { get; private set; }
    public string Fluxo { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Ciclo() { }

    public Ciclo(DateTime data, string fluxo)
    {
        Id = Guid.NewGuid();
        Data = data.Date;
        Fluxo = fluxo;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(DateTime data, string fluxo)
    {
        Data = data.Date;
        Fluxo = fluxo;
    }
}
