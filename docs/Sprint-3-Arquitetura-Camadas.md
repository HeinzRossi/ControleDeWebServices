# Sprint 3: Arquitetura Em Camadas

## Objetivo

Preparar a base arquitetural para separar View, ViewModels, Application, Domain e Infrastructure no `ControleDeWebServices`, sem migrar telas, sem alterar regras de negocio e sem modificar modelos.

## Estado

- Branch: `codex/sprint-3-arquitetura-camadas`.
- Base: `codex/sprint-2-componentes`.
- Target mantido: `net8.0-windows`.
- `StartupUri="MainWindow.xaml"` permanece para evitar migracao do Shell nesta sprint.
- Regra transversal mantida: nao alterar `ControleDeWebServices/Modelo`.

## Camadas Criadas

| Camada | Finalidade Nesta Sprint |
| --- | --- |
| `Application` | Contratos para dados, feedback e navegacao. |
| `Domain` | Tipos puros novos, sem dependencia de WPF e sem alterar modelos existentes. |
| `Infrastructure` | Implementacao inicial de factory para `DadosDBContext`. |
| `Presentation` | Implementacoes neutras temporarias para feedback e navegacao. |
| `ViewModels` | Base MVVM para telas futuras. |

## Contratos Criados

- `IDadosDbContextFactory`: ponto unico futuro para criar `DadosDBContext`.
- `IToastService`: contrato para toasts de informacao, sucesso, aviso e erro.
- `IConfirmDialogService`: contrato para dialogos customizados bloqueantes.
- `INavigationService`: contrato para navegacao futura sem depender de `FrameActive` espalhado.
- `ViewModelBase`: base MVVM com `ObservableObject`, `IsBusy` e `StatusMessage`.

## Decisoes

- `Microsoft.Extensions.DependencyInjection` passa a ser a base oficial de DI.
- `CommunityToolkit.Mvvm` passa a ser a base oficial para `ObservableObject` e comandos futuros.
- Implementacoes `NoOp` foram registradas apenas para permitir composicao sem alterar comportamento visual atual.
- A migracao real de telas para binding, commands, toasts e dialogos customizados fica para Sprint 4 em diante.

## Validacao

- `dotnet restore ControleDeWebServices.sln`: executado com sucesso.
- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, mantendo 0 erros e avisos herdados do codigo atual.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/`: continua ignorada e sem arquivos rastreados.
- `rg` para `new DadosDBContext`, `MessageBox` e eventos XAML: executado para registrar baseline; ocorrencias permanecem para Sprint 4 em diante.

## Pendencias Para Sprint 4

- Migrar Shell para ViewModel e `INavigationService` real.
- Criar host visual de toasts e dialogos customizados.
- Substituir `MessageBox` como experiencia visual padrao.
- Comecar troca de eventos XAML por bindings e commands.
