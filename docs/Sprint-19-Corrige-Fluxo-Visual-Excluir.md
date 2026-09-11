# Sprint 19: Corrige Fluxo Visual De Exclusao

## Resumo

Esta sprint corrige o fluxo visual do botao `Excluir`. O problema identificado estava no overlay do dialogo customizado: o `Grid` tinha `Visibility="Collapsed"` como valor local e tambem usava trigger de estilo para abrir quando `ConfirmDialogService.IsOpen` fosse `true`.

Em WPF, o valor local de `Visibility` tem precedencia sobre o setter/trigger do estilo. Com isso, o comando de exclusao podia chamar `ConfirmAsync`, mas o dialogo nao aparecia visualmente, deixando a acao com aparencia de botao sem funcionamento.

## Ajustes Realizados

- Removido o `Visibility="Collapsed"` local do overlay de confirmacao em `MainWindow.xaml`.
- Mantida a visibilidade controlada pelo `DataTrigger` ligado a `ConfirmDialogService.IsOpen`.
- Adicionados testes do `ConfirmDialogService` para confirmar/cancelar a operacao.
- Adicionado teste de XAML para impedir que o overlay volte a ter `Visibility` local antes do `Grid.Style`.

## Validacao Esperada

- Clicar em `Excluir` abre o dialogo customizado.
- Clicar em `Excluir` dentro do dialogo confirma a operacao.
- Clicar em `Cancelar` cancela sem executar exclusao.
- O overlay nao aparece na abertura do sistema.
- `Remover` permanece apenas para acoes internas de desvinculo.

## Fora De Escopo

- Alterar modelos em `ControleDeWebServices/Modelo`.
- Alterar regra de negocio, banco, publicacao ou design global.
- Reintroduzir `MessageBox`.
