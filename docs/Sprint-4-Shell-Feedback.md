# Sprint 4: Shell, Navegacao E Feedback Base

## Objetivo

Centralizar a navegacao do Shell e criar a base visual de feedback do `ControleDeWebServices`, sem migrar todas as telas e sem alterar modelos, regras de negocio, banco, importacao ou execucao de WebServices.

## Estado

- Branch: `codex/sprint-4-shell-feedback`.
- Base: `codex/sprint-3-arquitetura-camadas`.
- Target mantido: `net8.0-windows`.
- Escopo aplicado: Shell + feedback base.
- Regra transversal mantida: nao alterar `ControleDeWebServices/Modelo`.

## Entregas

| Area | Resultado |
| --- | --- |
| Shell | `MainWindowViewModel` criado com rota ativa, comandos de navegacao e comando de saida. |
| Navegacao | `WpfNavigationService` centraliza a criacao das paginas e usa `FramePrincipal` como host visual. |
| Toast | `ToastService` criado com pilha limitada, fechamento manual e remocao automatica. |
| Dialogo | `ConfirmDialogService` criado com overlay/modal customizado para confirmacoes futuras. |
| XAML | `MainWindow.xaml` recebeu host de toasts e overlay de dialogo sem redesenhar o layout final. |
| Excecao global | `MessageBox` do handler global do Shell foi substituido por toast de erro generico. |

## Decisoes

- `FramePrincipal` permanece como host visual nesta sprint para reduzir risco.
- O code-behind do Shell ainda faz ponte entre eventos atuais do `TreeView` e comandos do ViewModel.
- O comportamento de Enter avancando foco foi preservado no Shell.
- `MessageBox` foi removido do `MainWindow.xaml.cs`; ocorrencias em telas internas seguem para Sprint 5 em diante.
- Toasts usam cores semanticas alinhadas ao visual aprovado: azul, verde, ambar e vermelho.

## Validacao

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros e avisos herdados do codigo atual.
- `rg -n "MessageBox" ControleDeWebServices/MainWindow.xaml.cs`: sem ocorrencias.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/`: continua ignorada e sem arquivos rastreados.
- Execucao visual manual com `dotnet run`: nao realizada nesta sprint para evitar manter processo WPF interativo aberto no ambiente automatizado.

## Pendencias Para Sprint 5

- Migrar a tela piloto de Clientes para ViewModel, comandos e servicos de aplicacao.
- Trocar `MessageBox` de listas/cadastros simples por `IToastService` e `IConfirmDialogService`.
- Reduzir criacao direta de `DadosDBContext` nas telas migradas.
- Comecar padronizacao de validacoes e protecao contra selecao nula.
