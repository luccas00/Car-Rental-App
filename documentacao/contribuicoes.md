# Contribuições

## Identificação

- Projeto: `Matrix-Developers/Car-Rental-App`
- Repositório original: https://github.com/Matrix-Developers/Car-Rental-App
- Fork da dupla: https://github.com/luccas00/Car-Rental-App

## Integrantes

| Nome | Matrícula | GitHub | Principais contribuições |
|------|-----------|--------|--------------------------|
| Luccas Vinicius | **20.1.8015** | `luccas00` | Refatoração da Issue #216, arquitetura, pipeline GitHub Actions, documentação e integração |
| Davi Zanoti | **24.1.8062** | `DaviZ2399` | Revisão dos PRs, validações, documentação e colaboração em dupla |

## Issue escolhida

**#216 – Refactor - Implementar injeção de dependência corretamente para a tela de login**

### Resumo

A tela de login criava manualmente o DbContext, repositórios e serviços de aplicação, além de instanciar diretamente a TelaPrincipalForm, aumentando o acoplamento entre interface, aplicação e infraestrutura.

### Solução

- remoção da criação manual do DbContext;
- utilização do Autofac para composição das dependências;
- injeção do FuncionarioAppService;
- utilização de `Func<TelaPrincipalForm>`;
- remoção de código morto;
- redução do acoplamento entre os formulários.

## Principais Pull Requests

| PR | Título | Link |
|---|---|---|
| #1 | `refactor(login): resolve dependencies with Autofac` | https://github.com/luccas00/Car-Rental-App/pull/1 |
| #2 | `chore: ignore environment-specific database configuration` | https://github.com/luccas00/Car-Rental-App/pull/2 |
| #3 | `ci: add automated build and test workflow` | https://github.com/luccas00/Car-Rental-App/pull/3 |
| #4 | `docs(architecture): document system architecture` | https://github.com/luccas00/Car-Rental-App/pull/4 |
| #5 | `docs(quality): document patterns and code smells` | https://github.com/luccas00/Car-Rental-App/pull/5 |
| #6 | `docs: complete project contribution documentation` | https://github.com/luccas00/Car-Rental-App/pull/6 |
| #7 | `test(di): add Autofac integration tests` | https://github.com/luccas00/Car-Rental-App/pull/7 |

## Estratégia de Branches

- `main`
- `DEV`
- `refactor/issue-216-login-di`
- `ci/github-actions`
- `docs/architecture`
- `docs/quality-analysis`
- `docs/final-documentation`

Todos os desenvolvimentos foram realizados em branches próprias e integrados à `DEV` por Pull Requests.
