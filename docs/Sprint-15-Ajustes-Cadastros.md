# Sprint 15: Ajustes De Cadastros

## Objetivo

Corrigir a interação das telas de cadastro simples para impedir ações conflitantes durante inclusão/edição e garantir que as grids sejam somente leitura.

## Escopo

Telas cobertas:

- Clientes.
- Sistemas.
- Serviços.
- Seções.

Fora do escopo:

- Modelos em `ControleDeWebServices/Modelo`.
- Banco de dados, regras de negócio, publicação e WebServices.
- Vínculos, configuração e importação.
- Design system global.

## Correção Aplicada

- ViewModels de lista passaram a expor `CanUseListActions` e `IsListEnabled`.
- Comandos `Incluir`, `Editar` e `Excluir` agora só podem executar quando `IsEditing == false`.
- Chamadas diretas aos métodos de ação retornam sem efeito quando já existe formulário aberto.
- Ao alterar `IsEditing`, os comandos são notificados para atualizar `CanExecute`.
- Botão `Incluir` fica desabilitado durante inclusão/edição.
- Grids ficam desabilitadas durante inclusão/edição, mas continuam visíveis para preservar contexto.
- Grids de cadastros simples passaram a usar `IsReadOnly="True"`.
- Botões `Editar` e `Excluir` de linha ficam desabilitados durante inclusão/edição.

## Validação Automatizada

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros e 0 avisos.
- `dotnet test ControleDeWebServices.sln --no-build`: executado com sucesso, 17 testes aprovados.

Testes adicionados:

- Inclusão desabilita ações de lista em Clientes.
- Cancelamento reabilita ações de lista em Clientes.
- Nova inclusão durante edição não substitui o editor atual em Clientes.
- Inclusão desabilita ações de lista em Sistemas.
- Cancelamento reabilita ações de lista em Sistemas.

## Validação Manual Recomendada

- Abrir Clientes, Sistemas, Serviços e Seções.
- Clicar em `Editar` e confirmar que `Incluir`, grid e ações de linha ficam desabilitados.
- Clicar em `Incluir` e confirmar que a grid não aceita seleção/edição direta.
- Confirmar que células da grid não entram em modo de edição.
- Clicar em `Salvar` ou `Cancelar` e confirmar que lista e ações voltam a habilitar.

## Critérios De Aceite

- Nenhum cadastro simples permite iniciar nova inclusão durante edição.
- Nenhum cadastro simples permite editar/excluir linha enquanto formulário está aberto.
- Nenhuma grid de cadastro simples permite edição direta de célula.
- `ControleDeWebServices/Modelo` permanece sem alterações.
