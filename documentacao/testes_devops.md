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

### Pipeline implementada

Foi adicionada uma pipeline de integração contínua no arquivo:

```text
.github/workflows/dotnet-ci.yml
```

A pipeline é acionada automaticamente em:

- pushes para as branches `DEV` e `main`;
- Pull Requests direcionados às branches `DEV` e `main`.

Ela executa:

- checkout do repositório;
- instalação e configuração do .NET 5 e .NET 8;
- restauração das dependências;
- build da aplicação Windows Forms;
- execução dos testes unitários;
- execução dos testes da camada de aplicação;
- publicação dos resultados como artefato.

## Estratégia de execução no CI

A pipeline executa os projetos que não dependem de uma instância externa de SQL Server:

| Projeto | Quantidade | Execução |
|---|---:|---|
| `LocadoraDeVeiculos.UnitTests` | 54 | Local e CI |
| `LocadoraDeVeiculos.ApplicationTests` | 30 | Local e CI |
| `LocadoraDeVeiculos.ORMTests` | 42 | Somente local |
| `LocadoraDeVeiculos.DependencyInjection.Tests` | 6 | Somente local |

Assim, a pipeline executa automaticamente 84 testes, enquanto a validação local completa totaliza 132 testes.

## Justificativa para testes locais

Os testes ORM dependem diretamente de uma instância SQL Server configurada.

Os testes de injeção de dependência também possuem dependência indireta do banco. Ao criar a `TelaPrincipalForm`, o construtor carrega o dashboard e consulta dados por meio do `LocacaoAppService`.

No ambiente local, esses testes foram executados com a instância SQL Server configurada e todos foram aprovados.

No runner padrão do GitHub Actions, a instância `SQLEXPRESS` utilizada pela aplicação não está disponível. Por esse motivo, os testes ORM e os testes de injeção de dependência foram mantidos na validação local, enquanto a pipeline executa os testes independentes de infraestrutura.

Essa separação evita resultados falsamente negativos no CI sem remover os testes do projeto.

## Justificativa DevOps

A pipeline automatiza as verificações de build e testes a cada alteração integrada ao repositório.

Isso reduz o risco de regressões e permite identificar problemas antes que sejam incorporados às branches principais.

A separação entre testes independentes e testes dependentes de infraestrutura mantém o processo de integração contínua confiável e compatível com o ambiente disponível no GitHub Actions.

## Evidências

As evidências utilizadas na apresentação incluem:

- execução dos 132 testes no Gerenciador de Testes do Visual Studio;
- detalhamento dos 6 testes de injeção de dependência;
- execução dos novos testes pela CLI;
- execução da pipeline GitHub Actions;
- histórico dos Pull Requests utilizados durante o desenvolvimento.
