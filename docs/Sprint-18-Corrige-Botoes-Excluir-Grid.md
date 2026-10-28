# Sprint 18: Corrige Botoes Excluir Nas Grids

## Resumo

Esta sprint corrige os botoes `Excluir` sem acao nas grids de cadastros simples. A causa provavel era o binding do comando dentro de `DataGridTemplateColumn.CellTemplate` buscando `DataContext` no `Page`, que pode nao ser resolvido de forma confiavel pelo template em runtime.

## Ajustes Realizados

- Botoes `Editar` e `Excluir` das grids de Clientes, Sistemas, Servicos e Secoes passam a buscar comandos pelo `DataGrid`.
- `CommandParameter="{Binding}"` foi preservado para enviar o item da linha ao ViewModel.
- Botoes `Excluir` continuam desabilitados durante inclusao/edicao via `CanUseListActions`.
- Botoes de vinculos e configuracao foram auditados sem mudanca de fluxo.

## Validacao

- Testes de Clientes e Sistemas exercitam `ExcluirCommand` com item da linha.
- Confirmacao aprovada chama o service de exclusao.
- Confirmacao cancelada nao chama o service.
- Sem item selecionado, o comando mostra toast de aviso.
- Grids permanecem somente leitura.

## Fora De Escopo

- Alterar modelos em `ControleDeWebServices/Modelo`.
- Alterar regras de negocio, banco, publicacao ou design system global.
- Alterar os botoes internos `Remover` usados para desvincular itens.
