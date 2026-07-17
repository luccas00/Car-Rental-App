# Testes de Software e DevOps

## Identificação

- Projeto: `Matrix-Developers/Car-Rental-App`
- Luccas Vinicius — `20.1.8015`
- Davi Zanoti — `24.1.8062`

## Estrutura de testes existente

O projeto já possuía uma estrutura de testes automatizados separada em projetos específicos:

```text
LocadoraDeVeiculos.UnitTests
LocadoraDeVeiculos.ApplicationTests
LocadoraDeVeiculos.ORMTests
LocadoraDeVeiculos.TestDataBuilders
```

Os testes existentes cobrem regras de domínio, serviços de aplicação e integração com a camada de persistência.

## Execução dos testes existentes

Os testes já presentes no projeto foram executados localmente pelo Visual Studio e também por meio do comando:

```powershell
dotnet test LocadoraDeVeiculos.sln --logger "console;verbosity=normal"
```

Resultado obtido:

```text
Testes existentes executados: 126
Aprovados: 126
Falhas: 0
Ignorados: 0
```

A execução completa da solução pela CLI apresentou uma limitação relacionada ao projeto SQL Server `.sqlproj`, que depende do SSDT instalado no Visual Studio.

Essa limitação não representa falha nos testes automatizados. Todos os projetos de teste reconhecidos foram executados com sucesso.

## Novos testes adicionados

Foi criado o projeto:

```text
LocadoraDeVeiculos.DependencyInjection.Tests
```

O projeto foi organizado na solução dentro da categoria de testes de integração.

Foi utilizado o MSTest, framework de testes integrado ao ecossistema .NET, ao Visual Studio e ao comando `dotnet test`.

## Objetivo dos novos testes

Os novos testes validam a configuração do Autofac implementada durante a refatoração da Issue #216.

A refatoração removeu a criação manual das dependências na tela de login e passou a utilizar o container para resolver os formulários e seus serviços.

## Cenários implementados

### 1. Registro da tela de login

Verifica se a `TelaLogin` foi registrada corretamente no container Autofac.

```text
DevePossuirTelaLoginRegistradaNoContainer
```

### 2. Registro da tela principal

Verifica se a `TelaPrincipalForm` foi registrada corretamente no container Autofac.

```text
DevePossuirTelaPrincipalRegistradaNoContainer
```

### 3. Resolução da tela de login

Verifica se o container consegue criar uma instância válida da `TelaLogin` com suas dependências.

```text
DeveResolverTelaLoginPeloContainer
```

### 4. Resolução da tela principal

Verifica se o container consegue criar uma instância válida da `TelaPrincipalForm` com seus serviços.

```text
DeveResolverTelaPrincipalPeloContainer
```

### 5. Resolução da fábrica da tela principal

Verifica se o Autofac fornece uma fábrica do tipo:

```csharp
Func<TelaPrincipalForm>
```

Teste:

```text
DeveResolverFactoryDaTelaPrincipal
```

### 6. Criação de instâncias independentes

Verifica se a fábrica cria instâncias diferentes da tela principal, evitando o compartilhamento acidental do mesmo formulário.

```text
FactoryDeveCriarInstanciasDiferentesDaTelaPrincipal
```

## Resultado dos novos testes

Comando utilizado:

```powershell
dotnet test .\LocadoraDeVeiculos.DependencyInjection.Tests\LocadoraDeVeiculos.DependencyInjection.Tests.csproj --verbosity minimal
```

Resultado:

```text
Total: 6
Bem-sucedidos: 6
Falhas: 0
Ignorados: 0
```

## Resultado consolidado

Após a inclusão dos novos testes, o Gerenciador de Testes do Visual Studio apresentou:

```text
Total de testes: 132
Aprovados: 132
Falhas: 0
```

Distribuição:

| Projeto | Quantidade |
|---|---:|
| `LocadoraDeVeiculos.UnitTests` | 54 |
| `LocadoraDeVeiculos.ApplicationTests` | 30 |
| `LocadoraDeVeiculos.ORMTests` | 42 |
| `LocadoraDeVeiculos.DependencyInjection.Tests` | 6 |
| **Total** | **132** |

## Relação com a Issue #216

Os novos testes protegem os principais pontos da refatoração:

- registro da `TelaLogin` no Autofac;
- registro da `TelaPrincipalForm`;
- resolução automática dos formulários;
- utilização de `Func<TelaPrincipalForm>`;
- criação de instâncias independentes;
- centralização da composição das dependências;
- redução do acoplamento entre interface e infraestrutura.

## Classificação dos testes adicionados

Os novos testes são classificados como testes de integração, pois verificam o funcionamento conjunto de:

- container Autofac;
- formulários Windows Forms;
- serviços de aplicação;
- repositórios;
- contexto do Entity Framework.

Eles não automatizam cliques ou elementos visuais da interface.

Testes completos de interface poderiam ser implementados futuramente com ferramentas específicas para automação de aplicações desktop Windows.

## Instruções de execução local

### Executar os novos testes

```powershell
dotnet test .\LocadoraDeVeiculos.DependencyInjection.Tests\LocadoraDeVeiculos.DependencyInjection.Tests.csproj --verbosity minimal
```

### Executar os testes existentes separadamente

```powershell
dotnet test .\LocadoraDeVeiculos.UnitTests\LocadoraDeVeiculos.UnitTests.csproj
dotnet test .\LocadoraDeVeiculos.ApplicationTests\LocadoraDeVeiculos.ApplicationTests.csproj
dotnet test .\LocadoraDeVeiculos.ORMTests\LocadoraDeVeiculos.ORMTests.csproj
```

Os testes ORM exigem uma instância SQL Server configurada e acessível.

## DevOps e CI/CD

## Pipeline implementada

Foi adicionada uma pipeline de integração contínua no arquivo:

```text
.github/workflows/dotnet-ci.yml
```

A pipeline executa:

- checkout do repositório;
- instalação e configuração do .NET;
- restauração das dependências;
- build da aplicação Windows Forms;
- execução dos testes unitários;
- execução dos testes de aplicação.

## Situação atual da pipeline

A pipeline já foi adicionada e integrada ao repositório.

Os novos testes de injeção de dependência ainda serão adicionados ao workflow em uma alteração posterior.

A execução da pipeline com esses novos testes também será validada posteriormente no GitHub Actions.

## Justificativa DevOps

A pipeline automatiza as verificações de build e testes a cada alteração integrada ao repositório.

Isso reduz o risco de regressões e permite identificar problemas antes que sejam incorporados às branches principais.

## Evidências

As evidências utilizadas na apresentação incluem:

- execução dos 132 testes no Gerenciador de Testes do Visual Studio;
- detalhamento dos 6 testes de injeção de dependência;
- execução dos novos testes pela CLI;
- histórico da pipeline GitHub Actions;
- Pull Requests utilizados durante o desenvolvimento.
