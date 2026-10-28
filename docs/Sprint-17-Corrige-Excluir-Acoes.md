# Sprint 17: Corrige Copy E Acoes De Exclusao

## Resumo

Esta sprint corrige a nomenclatura e a acionabilidade dos botoes destrutivos apos os ajustes de cadastros e vinculos.

Regra aplicada:

- `Excluir`: acao destrutiva de toolbar, parametro ou exclusao em massa.
- `Remover`: acao interna de desvincular/mover item em listas de vinculo.

## Ajustes Realizados

- Botoes destrutivos de topo nas telas de vinculo passam a exibir `Excluir`.
- Botoes de exclusao de parametros passam a exibir `Excluir`.
- Botoes internos dos paineis de vinculo continuam como `Remover`, pois retiram item da lista de vinculados.
- Dialogos e toasts de exclusao de vinculos passam a usar copy de `Excluir`.
- Testes passam a executar `ExcluirCommand` diretamente, garantindo que o clique via binding aciona o servico.

## Validacao

- `ExcluirCommand` fica disponivel quando a lista nao esta em edicao/configuracao.
- Com selecao valida e confirmacao aprovada, o servico de exclusao e chamado.
- Sem selecao valida, o comando mostra toast de aviso.
- Durante edicao/configuracao, o comando permanece bloqueado.
- `Remover` permanece apenas nos botoes internos de desvincular itens.

## Fora De Escopo

- Alterar modelos em `ControleDeWebServices/Modelo`.
- Alterar regra de negocio, banco, publicacao ou design system global.
- Trocar nomes internos de comandos que representam implementacao ja existente, como `RemoverParametroCommand`.
