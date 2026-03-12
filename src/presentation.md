---
marp: true
class: invert
paginate: true
---

# Você realmente precisa do `?.`?

Identificando a complexidade invisível no código

---

# :dart: Objetivos

- Entender o C# mais a fundo
- Compartilhar alguns problemas conhecidos
- Refletir sobre complexidade do código no geral

---

# :thinking: Contexto

Existem usos **desnecessários** do operador [null-conditional](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-) (vulgo `?.` ou `?[]`)

Exemplo:

```cs
if (pessoa?.Empresa?.Endereco?.Cidade?.Nome == "Campo Bom")
```

Pensamento comum:

> :bulb: Mas é só um caractere a mais! Não é melhor estar defendido de valores nulos?

---

# :shrug: Depende

Toda funcionalidade da linguagem tem um custo (cognitivo ou computacional).

Para embasar melhor a resposta, primeiro precisamos entender mais a fundo a implicação de usar o operador.

---

# Semântica e Comportamento

1. Primeiro verifica o lado esquerdo da expressão
1. Se não for nulo, segue avaliando o membro da direita normalmente
1. Caso seja nulo, interrompe a operação e retorna null (short-circuiting)
1. Permite encadeamento (chaining)

```cs
A?.B?.C?.Do();
```

---

# Tipo de Retorno

Qual o tipo de retorno da seguinte expressão? (considerando que `Invisivel` é `bool`)

```cs
var invisivel = funcionario?.Invisivel;
```

Existe problema no código abaixo?

```cs
if (funcionario?.Invisivel)
{
    // ...
}
```

E nesse? :skull: :boom:

```cs
await pessoa?.SalvarAsync();
```

---

# :candy: Syntactic Sugar

São features das linguagens de programação que servem para facilitar cenários comuns, diminuindo a quantidade de código escrito. Geralmente são compiladas para outras funcionalidades mais rudimentares da linguagem. Alguns exemplos:

| Feature                  | Transformado em                         |
| ------------------------ | --------------------------------------- |
| `using`                  | `try ... finally`                       |
| `var`                    | Tipo explícito                          |
| Propriedades automáticas | "Backing field" criado automaticamente  |
| Inicializadores          | Atribuição das propriedades individuais |
| `?.` e `?[]`             | ifs e ternários                         |

---

# :test_tube: SharpLab

Usando o [sharplab](https://sharplab.io/#v2:CYLg1APgAgDABFAjAOgJIHkDcBYAUHqAZgQCYFEB2PAbzzntJLodtwfbgDcBDAJzgAOcALxwAdgFMA7nAAKEgM4KA9twAUASjjU4AOWUBbCSLgAiAGIBXADbcxy0wBo4qYN2DHRiABxwAvjhsHPQCAPzIAMrc1jy8moHsfnhJ+LhEpHKKKtw0zPTpSPD6RtpwAOYSAC6YcApVNSns6QCWYpUubh6lFdW19f55cIMFACxwUTF8moOswfnwarFwfACOls2cyiaSMhGVvBLcBgDqvM2VEnFQUKYAwiAAOgAqEgYCDwJZqsiVAB6Vpg0GkG7Fmc3Yq3Wm2Qp3OEgAMq0JGpihINAlwQxIRtlDCzhdEZI1K53GiMcFGgwUikgA===), podemos verificar como as features da linguagem são compiladas em diversos níveis: C#, IL, ASM. Também é possível ver a diferença de debug e release.

---

# null-conditional operator

Podemos usar novamente o [sharplab](https://sharplab.io/#v2:D4AQTAjAsAULIGYAE4kGEkG9ZN0gDgE4CWAbgIYAuApkgArUDOjA9uQU6+QNyw56IUAFiQBRCAAoAlFn54BEAJwT8nNgDoAciwC21Kbxjz5IJSrXkA/Ft37D8gL5ykzwSBGiw0rMbzOTZqrMGgASLITkJCw2egb+CspBXNZhEVHW2rH2eE5Gfnm4bh4I3pi+uPGFgRbqojpETOS1AHYAJtSE1ADG0WjEreTtMXaVKNXBVrX1nYyTom0d3SzWfQPtGbZxBUi5ufDIqAwTsttuEAAMSJm0ZQDm1JTcSIwPT7kmyHUNs2LTjT73R7PV47VzIVKRYgsJAQqIAkEvIG7PgwNxgGHhSHQ7CnA4XK62eFAxFvWB7VEHdFfGbsHEfMaXa5Ep4k0G4sQLTo9DntLnYpCAlkg5FwCkoKmcpYnemrQa0WXtZnApFklFo9D9OXSgR4xmEu4I4WqmBAA=) para verificar o impacto de ficar colocando `?.` no código.

| Antes              | Depois                                      |
| ------------------ | ------------------------------------------- |
| `p?.Nome`          | `(pessoa != null) ? pessoa.Nome : null`     |
| `p?.Horario?.Nome` | if + variáveis extras + ternários aninhados |

### :bulb: Um pouco de açucar ajuda a "adoçar" nosso código, mas se exagerarmos, podemos deixá-lo "enjoativo".

---

# :snail: Performance

- Cada `?.` adiciona mais caminhos possíveis de execução
- Encadeamentos criam árvores de execução ao invés de caminhos lineares
- Mais instruções de IL precisam ser geradas, compiladas e executadas, ocupando CPU, memória e armazenamento

## Benchmark

| Method                  |      Mean |     Error |    StdDev |    Median |
| ----------------------- | --------: | --------: | --------: | --------: |
| `pessoa.Horario.Nome`   | 0.0025 ns | 0.0070 ns | 0.0066 ns | 0.0000 ns |
| `pessoa?.Horario?.Nome` | 0.0992 ns | 0.0718 ns | 0.0737 ns | 0.1028 ns |

---

# Resumo até aqui

- O operador faz com que o código precise se preocupar com valores nulos (propaga o nulo)
- É muito fácil de "adicionar por garantia", afinal, "é apenas um caractere a mais", porém, isso tem um custo "escondido"
- Fácil de passar despercebido em revisões, afinal, "é só um caractere", em comparação ao uso de IFs mais explícitos
- Quando for de fato preciso tratar os nulos, geralmente é mais simples usar o operador do que escrever o IF manualmente

---

# Quando usado **desnecessariamente**

- Aumenta a complexidade do código
- Ocupa recursos sem necessidade
  - Diferença pode ser pequena dependendo do contexto, mas não deixa de ser desnecessária
  - Muitas vezes podemos querer trocar performance por manutenabilidade do código, mas não devemos trocar se o ganho for <= 0
  - Lembrem que complexidade é incremental

---

# Exemplo: O que acham deste método?

```cs
public string ConsistirDados(Pessoa pessoa)
{
    if (string.IsNullOrWhiteSpace(pessoa?.Nome))
    {
        return "Nome obrigatório";
    }

    if (pessoa?.Empresa?.BloquearNovosCadastros == true)
    {
        return "A empresa da pessoa não permite novos cadastros";
    }

    if (pessoa?.Tipo == TipoPessoa.Juridica && string.IsNullOrWhiteSpace(pessoa?.Cnpj))
    {
        return "CNPJ obrigatório para pessoas jurídicas";
    }

    // etc...

    return null;
}
```

---

# Alternativa: Early Return

```cs
public string ConsistirDados(Pessoa pessoa)
{
    if (pessoa == null) // Caso for um cenário de exceção, seria válido deixar estourar erro também
    {
        return "Pessoa não encontrada";
    }

    if (string.IsNullOrWhiteSpace(pessoa.Nome))
    {
        return "Nome obrigatório";
    }

    if (pessoa.Empresa?.BloquearNovosCadastros == true) // É válido manter o `?.` aqui?
    {
        return "A empresa da pessoa não permite novos cadastros";
    }

    if (pessoa.Tipo == TipoPessoa.Juridica && string.IsNullOrWhiteSpace(pessoa.Cnpj))
    {
        return "CNPJ obrigatório para pessoas jurídicas";
    }

    // etc...

    return null;
}
```

---

# :pencil: Tema de casa

1. Escreva as duas versões do exemplo acima no [sharplab](https://sharplab.io/) e compare o resultado
   - :bulb: Dica: pode escrever como dois métodos na mesma classe para facilitar a comparação
1. Experimente alterar entre Debug e Release para verificar se existem diferenças
1. Além de C#, altere o resultado para IL, que já é um pouco mais próximo do ASM, e verifique a diferença

---

# :question: Devemos evitar o uso do `?.` e `?[]`?

- Não! Porém, assim como qualquer outra funcionalidade da linguagem, deve ser usada para problemas **onde ela se encaixa bem**, e não de forma indiscriminada.

## Checklist antes de usar

- Você teria que escrever o IF validando o `null` de qualquer forma?
- O operador é usado de forma pontual e ajuda na legibilidade do código?
- O uso ajuda na **divisão do problema** ou ele acaba trazendo a preocupação do `null` para pontos do código onde nem precisaria?
- Estou usando em uma chamada com `await`? :skull:

---

# Código simples

- :x: Código simples não é
  - Código que não se preocupa com a arquitetura / evolução futura
  - O primeiro código que vem à cabeça
  - Código que não usa recursos avançados da linguagem
- :white_check_mark: Código simples é
  - Quando todos os recursos utilizados (dos mais básicos aos mais avançados) "caem como uma luva" para aquele problema em específico
  - Escrito de forma que não traga complexidade adicional e **desnecessária** ao problema em questão

---

# :exploding_head: Essa apresentação não foi sobre `?.`!

As dicas comentadas e forma de pensar servem para muita coisa no desenvolvimento. Principalmente, o fato de entender como as coisas são implementadas para utilizar a melhor ferramenta para cada caso.

Sempre tenham curiosidade.

---

# :speaking_head: Perguntas? Comentários?

---

# :link: Código da apresentação

- https://github.com/fhelwanger/null-conditional-op-presentation
