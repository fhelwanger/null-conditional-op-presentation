# Você realmente precisa do `?.`?

Este repositório contém o código de uma apresentação sobre os impactos do uso indiscriminado do operador null‑conditional no C#.

A proposta não é desencorajar o uso do `?.`, mas mostrar como pequenas escolhas sintáticas podem introduzir complexidade invisível e até comportamento inesperado no código.

A apresentação aborda semântica, performance, alternativas e boas práticas. Ela também apresenta uma linha de raciocínio que serve para outras tomadas de decisão durante o desenvolvimento.

## Apresentação

A versão HTML da apresentação é publicada via GitHub Pages e pode ser acessada em:

https://fhelwanger.github.io/null-conditional-op-presentation/


## Build da apresentação

Para o build, é necessário o [nodejs](https://nodejs.org/) v18+. Certifique-se de ter instalado as dependências:

```bash
npm install
```

Para gerar a apresentação em PDF ou HTML, use os seguintes comandos:

```bash
npm run build-pdf
npm run build-html
```

Esses comandos usarão o Marp CLI para gerar os arquivos em `dist/`.

## Executando o benchmark

Os benchmarks estão localizados em `src/benchmark/` e requerem .NET 10 ou superior.

Para executar os benchmarks:

```bash
cd src/benchmark
dotnet run -c Release
```

Isso executará os testes de performance usando BenchmarkDotNet.
