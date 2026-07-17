# Padrões, Code Smells e Refatorações

## Identificação

- Projeto: `Matrix-Developers/Car-Rental-App`
- Luccas Vinicius — `luccas00`
- Davi Zanoti — `DaviZ2399`
- Issue: `#216 - Implementar injeção de dependência corretamente para a tela de login`

## Code Smell 1 — Alto acoplamento na tela de login

### Trecho original

```csharp
db = new();
servicoAppService = new(new ServicoORM(db));
clienteAppService = new(new ClienteORM(db));
funcionarioAppService = new(new FuncionarioORM(db));
veiculoAppService = new(
    new VeiculoORM(db),
    new ImagemVeiculoORM(db)
);
locacaoAppService = new(new LocacaoORM(db));
```

### Problema

A camada de apresentação criava diretamente o contexto do Entity Framework, os repositórios e os serviços de aplicação. Isso tornava a tela dependente das implementações concretas da infraestrutura e aumentava o acoplamento entre as camadas.

Além disso, qualquer alteração na criação dos repositórios ou serviços exigia mudanças na própria tela de login, dificultando a manutenção e os testes.

### Solução

A `TelaLogin` passou a receber somente o `FuncionarioAppService` necessário para realizar a autenticação.

A criação do contexto, dos repositórios e dos demais serviços passou a ser responsabilidade do container Autofac, centralizando a composição das dependências.

### Princípios relacionados

- Dependency Inversion Principle;
- Single Responsibility Principle.

---

## Code Smell 2 — Tela de login atuando como Composition Root

### Trecho original

```csharp
Application.Run(new TelaPrincipalForm(
    servicoAppService,
    clienteAppService,
    veiculoAppService,
    locacaoAppService
));
```

### Problema

A `TelaLogin` conhecia todas as dependências da `TelaPrincipalForm` e era responsável por construí-la manualmente.

Uma tela de autenticação não deveria conhecer os parâmetros internos de outro formulário nem controlar a composição de sua árvore de dependências.

Essa responsabilidade aumentava o acoplamento entre os formulários e dificultava futuras alterações na tela principal.

### Solução

Foi injetada uma fábrica:

```csharp
private readonly Func<TelaPrincipalForm> telaPrincipalFactory;
```

A criação da tela principal passou a ser realizada da seguinte forma:

```csharp
TelaPrincipalForm telaPrincipal = telaPrincipalFactory();
Application.Run(telaPrincipal);
```

O Autofac resolve automaticamente a `TelaPrincipalForm` e todos os serviços necessários para sua construção.

### Princípios relacionados

- Dependency Inversion Principle;
- Single Responsibility Principle.

---

## Code Smell 3 — Código morto e criação desnecessária

### Trecho original

```csharp
TelaLogin login = new();
this.Dispose();
login.Close();
```

### Problema

Uma nova instância da tela de login era criada apenas para ser fechada logo em seguida, sem produzir comportamento útil para a aplicação.

Esse trecho aumentava a complexidade do fluxo de navegação e criava um objeto desnecessário na memória.

### Solução

O trecho foi substituído por:

```csharp
Close();
```

A tela atual é encerrada diretamente após a autenticação, removendo a criação desnecessária de uma nova instância.

### Princípios relacionados

- Single Responsibility Principle;
- simplicidade e redução de código desnecessário.

---

## Padrão 1 — Dependency Injection

### Onde é utilizado

O projeto utiliza Autofac para centralizar a criação e o fornecimento de dependências, incluindo:

- contexto do Entity Framework;
- repositórios;
- serviços de aplicação;
- operações;
- formulários.

Na refatoração da issue #216, a `TelaLogin` passou a receber suas dependências pelo construtor, em vez de criá-las diretamente.

### Justificativa

A injeção de dependência reduz o acoplamento entre a camada de apresentação e as implementações concretas da infraestrutura.

Isso facilita a manutenção, a substituição de implementações e a criação de testes automatizados.

---

## Padrão 2 — Repository

### Onde é utilizado

Contratos como `IRepository<T>` isolam a aplicação dos detalhes de persistência.

Classes como:

- `ClienteORM`;
- `FuncionarioORM`;
- `VeiculoORM`;
- `LocacaoORM`;
- `ServicoORM`;
- `CupomORM`;

implementam operações de acesso aos dados por meio do Entity Framework.

### Justificativa

O padrão Repository centraliza as operações de persistência e impede que as regras de negócio dependam diretamente do Entity Framework ou do SQL Server.

Essa separação melhora a organização do código e facilita a manutenção da camada de dados.

---

## Padrão adicional — Factory

### Onde é utilizado

O Autofac fornece automaticamente uma fábrica do tipo:

```csharp
Func<TelaPrincipalForm>
```

A fábrica é injetada na `TelaLogin` e utilizada somente após a autenticação ser concluída.

### Justificativa

A fábrica encapsula a criação da tela principal e posterga sua instanciação até o momento necessário.

Com isso, a `TelaLogin` não precisa conhecer os parâmetros do construtor da `TelaPrincipalForm`, reduzindo o acoplamento entre os formulários.

---

## Refatoração aplicada

A refatoração da issue #216 realizou as seguintes alterações:

- remoção da criação manual do `LocadoraDeVeiculosDBContext` na tela de login;
- remoção da criação manual dos repositórios e serviços;
- injeção do `FuncionarioAppService` pelo construtor;
- injeção de `Func<TelaPrincipalForm>`;
- resolução da `TelaLogin` pelo Autofac;
- resolução da `TelaPrincipalForm` e de suas dependências pelo Autofac;
- remoção de código morto relacionado à criação de uma nova tela de login;
- redução do acoplamento entre a interface, a aplicação e a infraestrutura.

## Análise de qualidade de código

A solução foi analisada utilizando o SonarQube for IDE, anteriormente denominado SonarLint, integrado ao Visual Studio.

A análise auxiliou na identificação de problemas relacionados a:

- criação manual de dependências;
- responsabilidades excessivas na tela de login;
- código morto;
- forte acoplamento entre a interface e a infraestrutura;
- uso de APIs obsoletas;
- dependências legadas com alertas de compatibilidade e segurança.

Após a refatoração da issue #216:

- a criação das dependências foi centralizada no Autofac;
- a `TelaLogin` passou a possuir menos responsabilidades;
- o código desnecessário foi removido;
- a composição da tela principal deixou de ser realizada manualmente.

As advertências relacionadas a bibliotecas legadas, como `PDFsharp`, `System.Drawing.Common` e `BinaryFormatter`, foram registradas, mas não foram modificadas por estarem fora do escopo da issue selecionada.

## Evidência da análise

A evidência da análise deve ser adicionada no seguinte caminho:

```text
documentacao/evidencias/sonarlint-analysis.png
```

Após adicionar a imagem, utilizar a referência:

```md
![Análise de qualidade com SonarQube for IDE](evidencias/sonarlint-analysis.png)
```

## Arquivos alterados

```text
LocadoraDeVeiculos/LocadoraDeVeiculos.WindowsApp/Features/Login/TelaLogin.cs
LocadoraDeVeiculos/LocadoraDeVeiculos.WindowsApp/Shared/AutoFac.cs
LocadoraDeVeiculos/LocadoraDeVeiculos.WindowsApp/Program.cs
```
