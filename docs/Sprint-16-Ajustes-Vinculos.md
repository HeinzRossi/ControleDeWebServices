# Sprint 16: Ajustes De Vinculos E Grids Readonly

## Objetivo

Aplicar nas telas de vinculo o mesmo bloqueio de acoes concorrentes usado nos cadastros simples e tornar todas as `DataGrid` do projeto somente leitura.

## Escopo

Telas cobertas:

- WebServices.
- Clientes, Sistemas, Servicos e Secoes.
- Lista de Vinculo Cliente/Sistema.
- Lista de Vinculo Cliente/Servico.
- Configuracao de informacoes e grids de parametros em templates de vinculo.

Fora do escopo:

- Modelos em `ControleDeWebServices/Modelo`.
- Banco de dados, regras de negocio e publicacao.
- Mudanca no fluxo funcional de WebServices.

## Correcoes Aplicadas

- Todas as `DataGrid` XAML passaram a usar `IsReadOnly="True"`.
- ViewModels das listas de vinculo passaram a expor `IsBusyWithDetails`, `CanUseListActions` e `IsListEnabled`.
- Comandos `Incluir`, `Editar`, `Configurar` e `Remover` das listas de vinculo agora respeitam `CanUseListActions`.
- Chamadas diretas aos comandos durante edicao/configuracao retornam sem trocar o editor ou configuracao atual.
- Grids principais das listas de vinculo ficam desabilitadas durante edicao/configuracao, mas permanecem visiveis.
- Botoes `Adicionar` e `Remover` dos paines internos usam layout em duas colunas, mantendo o botao alinhado a direita.
- Textos dos itens internos usam `TextTrimming="CharacterEllipsis"` para evitar sobreposicao.

## Validacao Automatizada

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros e 0 avisos.
- `dotnet test ControleDeWebServices.sln --no-build`: executado com sucesso.

Testes adicionados:

- Inclusao em Vinculo Cliente/Sistema desabilita acoes do topo.
- Cancelamento em Vinculo Cliente/Sistema reabilita acoes do topo.
- Configuracao em Vinculo Cliente/Sistema desabilita acoes do topo.
- Nova inclusao durante edicao de Vinculo Cliente/Sistema nao troca editor atual.
- Inclusao em Vinculo Cliente/Servico desabilita acoes do topo.
- Cancelamento em Vinculo Cliente/Servico reabilita acoes do topo.
- Configuracao em Vinculo Cliente/Servico desabilita acoes do topo.
- Nova inclusao durante configuracao de Vinculo Cliente/Servico nao troca configuracao atual.

## Validacao Manual Recomendada

- Abrir Vinculo Cliente/Sistema.
- Clicar em `Incluir`, `Editar` e `Configurar`, confirmando que os demais botoes do topo ficam bloqueados.
- Repetir no Vinculo Cliente/Servico.
- Confirmar que grids principais nao entram em modo de edicao.
- Confirmar que os botoes `Adicionar` e `Remover` ficam alinhados a direita, sem cobrir o nome do item.
- Confirmar que salvar/cancelar volta a habilitar lista e acoes.

## Criterios De Aceite

- Nenhuma `DataGrid` permite edicao direta de celula.
- Nenhuma tela de vinculo permite iniciar nova inclusao, edicao ou configuracao enquanto outra dessas operacoes esta aberta.
- `ControleDeWebServices/Modelo` permanece sem alteracoes.
