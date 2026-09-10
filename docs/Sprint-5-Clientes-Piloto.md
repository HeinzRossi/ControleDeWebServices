# Sprint 5: Tela Piloto Clientes

## Objetivo

Migrar Clientes como primeira tela piloto para MVVM, servicos de aplicacao, feedback por toast/dialogo customizado e visual operacional limpo, sem alterar modelos, banco, migrations, SQL externo, importacao ou execucao de WebServices.

## Estado

- Branch: `codex/sprint-5-clientes-piloto`.
- Base: `codex/sprint-4-shell-feedback`.
- Target mantido: `net8.0-windows`.
- Escopo aplicado: fluxo Clientes.
- Regra transversal mantida: nao alterar `ControleDeWebServices/Modelo`.

## Entregas

| Area | Resultado |
| --- | --- |
| Application | Criados `IClientesService`, `ClienteListItem` e `ClienteEditor`. |
| Infrastructure | Criado `ClientesService` usando `IDadosDbContextFactory`. |
| ViewModels | Criados `ClientesListViewModel` e `ClienteEditViewModel`. |
| Lista | `ListaDeClientes` passou a usar bindings, commands, selecao e formulario embutido. |
| Formulario | `Clientes` ficou como formulario bindavel simples, sem code-behind funcional. |
| Feedback | Validacoes usam toast; exclusao usa dialogo customizado destrutivo. |
| Visual | Tela Clientes recebeu cabecalho contextual, superficie clara, toolbar, DataGrid denso, acoes semanticas e estado vazio. |

## Decisoes

- `Cliente` continua sendo o modelo EF de persistencia e nao foi alterado.
- ViewModels usam DTOs/editors proprios para proteger a UI de detalhes do modelo.
- `ListaDeClientes` e a tela piloto principal; o antigo `FrameInserirtEditar` foi removido do fluxo.
- Mensagens do fluxo Clientes nao usam `MessageBox`.
- Acoes sem selecao mostram toast de aviso.
- O padrao criado deve orientar Sistemas, Servicos e Secoes na Sprint 6.

## Validacao

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros e avisos herdados de outras telas.
- `rg -n "MessageBox|new DadosDBContext|FrameInserirtEditar|FrameActive" ControleDeWebServices/View/Cadastro/ListaDeClientes.xaml* ControleDeWebServices/View/Cadastro/Clientes.xaml*`: sem ocorrencias esperadas.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/`: continua ignorada e sem arquivos rastreados.
- Validacao manual interativa fica recomendada no Visual Studio ou execucao local do app para confirmar fluxo com banco real.

## Pendencias Para Sprint 6

- Replicar o padrao para Sistemas, Servicos e Secoes.
- Migrar listas/cadastros restantes para ViewModels, commands e servicos de aplicacao.
- Remover `MessageBox`, `DadosDBContext` direto e `FrameActive` dos cadastros simples restantes.
- Consolidar estilos repetidos em ResourceDictionary quando mais telas usarem o mesmo visual.
