# Sprint 6: Cadastros, Listas E Vinculos

## Objetivo

Migrar Sistemas, Servicos, Secoes e os fluxos de vinculo Cliente/Sistema e Cliente/Servico para o padrao validado na Sprint 5: ViewModels, servicos de aplicacao, comandos, feedback por toast/dialogo customizado e visual operacional limpo.

## Estado

- Branch: `codex/sprint-6-cadastros-vinculos`.
- Base: `codex/sprint-5-clientes-piloto`.
- Target mantido: `net8.0-windows`.
- Escopo aplicado: cadastros simples restantes e vinculos.
- Regra transversal mantida: nao alterar `ControleDeWebServices/Modelo`.

## Entregas

| Area | Resultado |
| --- | --- |
| Cadastros | Criados contratos, DTOs/editors e servicos para Sistemas, Servicos e Secoes. |
| ViewModels | Criados ViewModels de lista/edicao para Sistemas, Servicos e Secoes. |
| Vinculos | Criados contratos, DTOs, servicos e ViewModels para Cliente/Sistema e Cliente/Servico. |
| Views | Listas migradas para bindings, commands, formulario/editor embutido e DataGrid operacional. |
| Feedback | Validacoes usam toast; exclusoes/remocoes em massa usam dialogo customizado. |
| Code-behind | Telas migradas ficaram restritas a `InitializeComponent`, DI e carga inicial. |

## Decisoes

- Os modelos EF existentes continuam intactos e sao usados apenas na Infrastructure.
- Vistas migradas nao instanciam `DadosDBContext` e nao exibem `MessageBox`.
- A regra de alteracoes pendentes em Cliente/Sistema foi corrigida no novo fluxo usando colecoes vinculadas como estado final.
- A gravacao dos vinculos adiciona apenas novos registros e remove apenas registros ausentes da lista final, preservando dados configurados nos vinculos que permanecem.
- Configuracao, importacao e banco externo continuam fora desta sprint e seguem para Sprint 7.

## Validacao

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros.
- Avisos restantes: herdados de telas ainda nao migradas, principalmente configuracao/importacao e `AtualizarPadroes`.
- `rg` nas telas migradas para `MessageBox`, `new DadosDBContext`, `FrameInserirtEditar`, `FrameActive`, `Click=` e `SelectionChanged=`: sem ocorrencias esperadas.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/`: continua ignorada e sem arquivos rastreados.

## Pendencias Para Sprint 7

- Migrar `CadastroDeInformacoes`, `CadastroDeInformacoesServico` e `ImportarParametrosDe`.
- Remover `MessageBox` e dialogs WinForms dos fluxos de configuracao/importacao.
- Isolar arquivos INI/XML, credenciais, criptografia e parametros em servicos de aplicacao/infraestrutura.
- Parametrizar SQL em `AtualizarPadroes` e revisar ciclo de vida de conexoes.
