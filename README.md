# Você realmente precisa do `?.`?

Este repositório contém o código de uma apresentação sobre os impactos do uso indiscriminado do operador null‑conditional no C#.

A proposta não é desencorajar o uso do `?.`, mas mostrar como pequenas escolhas sintáticas podem introduzir complexidade invisível e até comportamento inesperado no código.

A apresentação aborda semântica, performance, alternativas e boas práticas. Ela também apresenta uma linha de raciocínio que serve para outras tomadas de decisão durante o desenvolvimento.

## Build da apresentação

Para o build, é necessário o [nodejs](https://nodejs.org/) v18+. Certifique-se de ter instalado as dependências:

```
npm install
```

Para gerar a apresentação em PDF ou HTML, use os seguintes comandos:

```
npm run build-pdf
npm run build-html
```

Esses comandos usarão o Marp CLI para gerar os arquivos em `dist/`.
