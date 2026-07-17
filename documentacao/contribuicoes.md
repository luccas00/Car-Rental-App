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

- Refatoração da Issue #216
- Hotfix de configuração local
- Pipeline GitHub Actions
- Documentação da arquitetura
- Documentação de padrões e code smells
- Documentação final

## Estratégia de Branches

- `main`
- `DEV`
- `refactor/issue-216-login-di`
- `ci/github-actions`
- `docs/architecture`
- `docs/quality-analysis`
- `docs/final-documentation`

Todos os desenvolvimentos foram realizados em branches próprias e integrados à `DEV` por Pull Requests.
