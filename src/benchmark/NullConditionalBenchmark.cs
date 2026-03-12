using BenchmarkDotNet.Attributes;

public class NullConditionalBenchmark
{
    private Pessoa pessoa;

    public NullConditionalBenchmark()
    {
        pessoa = new Pessoa
        {
            Nome = "Fernando",
            Horario = new Horario { Nome = "Horário Geral" },
        };
    }

    [Benchmark]
    public string SemNullConditional()
    {
        return pessoa.Horario.Nome;
    }

    [Benchmark]
    public string ComNullConditional()
    {
        return pessoa?.Horario?.Nome;
    }
}
