# Mapper IA

- NUGET: [MapperIA.Core](https://www.nuget.org/packages/MapperIA.Core/)

---
## Descrição

O **Mapper IA** é um projeto que usa inteligência artificial para facilitar o mapeamento de dados. Ele foi criado para simplificar a extração de informações de documentos e permitir que os usuários transformem dados de diferentes fontes em estruturas organizadas e úteis. Neste momento, o Mapper IA já consegue extrair informações de documentos PDF, realizar mapeamentos entre diferentes tipos de classes, e converter arquivos de linguagens diferentes para C#.

**Nota:** Para utilizar o Mapper IA, é necessário ter uma API Key do Gemini.



## Funcionalidades Atuais

Atualmente, o **Mapper IA** oferece as seguintes funcionalidades:

1. **Mapeamento de texto de PDF para classes**: O uso é simples e intuitivo:
   - **Crie uma Classe**: Primeiro, o usuário deve criar uma classe que contenha os atributos que deseja extrair do PDF.
   - **Forneça o PDF**: Depois, forneça um arquivo PDF com o texto que será analisado.
   - **Processamento pela IA**: A inteligência artificial vai analisar o texto no PDF e descobrir quais valores se encaixam nos atributos definidos na classe.
   - **Retorno da Classe Preenchida**: O projeto retorna a classe preenchida com os dados extraídos do PDF.

2. **Conversão de classes**: O Mapper IA também permite converter uma classe em outra, facilitando o mapeamento entre entidades e DTOs. Por exemplo, você pode converter um objeto `Usuario` para um `UsuarioDTO` de forma rápida e eficiente.

3. **Conversão de arquivos de linguagens diferentes para C#**: Agora, o Mapper IA também suporta a conversão de arquivos de linguagens diferentes para C#. Por exemplo, você pode converter um arquivo `Student.py` para `Student.cs`. Além disso, é possível alterar o nome da classe durante o processo de conversão.

## Instalação

Para instalar o **Mapper IA**, você pode utilizar o NuGet:

```bash
dotnet add package MapperIA.Core
```

## Status do Projeto

O **Mapper IA** está em desenvolvimento, e novas funcionalidades estão sendo planejadas e implementadas para melhorar a experiência do usuário e a eficiência do mapeamento.


## Tecnologias Utilizadas

O **Mapper IA** é construído com as seguintes tecnologias:

- **C#**: A linguagem de programação principal utilizada para desenvolver a lógica do projeto.
- **JsonSerializer**: Para serialização e desserialização de dados em formato JSON, facilitando a troca de informações entre o aplicativo e a IA.
- **iText 7**: Uma biblioteca poderosa para manipulação de arquivos PDF, utilizada para extrair texto e informações dos documentos.
- **Gemini IA**: A IA que está integrada ao projeto, fornecendo suporte para o mapeamento de dados e facilitando a extração de informações relevantes.

## Como Contribuir

Contribuições são muito bem-vindas! Se você tiver ideias ou sugestões para melhorar o projeto, fique à vontade para abrir uma issue ou enviar um pull request.
