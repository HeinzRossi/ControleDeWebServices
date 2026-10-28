# QA Manual: ControleDeWebServices

## Objetivo

Validar manualmente os fluxos principais em ambiente com banco/configuração real, após a migração para WPF em .NET 8, MVVM, feedback por toast/dialog customizado, design system e logging local.

## Preparação

- Confirmar branch/build em teste.
- Confirmar `Config.ini` válido no diretório de execução.
- Confirmar acesso ao banco principal.
- Confirmar executáveis e arquivos de configuração usados nos vínculos de teste.
- Confirmar que a pasta `Logs` pode ser criada no diretório da aplicação.

## Shell E Navegação

- Abrir aplicação e confirmar que nenhum diálogo aparece no startup.
- Confirmar abertura inicial em `WebServices`.
- Navegar para Clientes, Sistemas, Serviços, Seções, Vínculo Cliente/Sistema e Vínculo Cliente/Serviço.
- Confirmar que Enter avança foco em campos, combos e botões.
- Confirmar que o menu lateral mantém leitura clara e não bloqueia a área de conteúdo.

## Feedback

- Gerar uma validação sem seleção e confirmar toast de atenção.
- Salvar um registro válido e confirmar toast de sucesso.
- Forçar erro controlado e confirmar toast de erro sem `MessageBox`.
- Executar exclusão/remover vínculo e confirmar diálogo customizado com ação destrutiva clara.
- Cancelar diálogo e confirmar que nada é alterado.

## Cadastros

- Clientes: listar, incluir, editar, cancelar, salvar e excluir.
- Sistemas: listar, incluir, editar, cancelar, salvar e excluir.
- Serviços: listar, incluir, editar, cancelar, salvar e excluir.
- Seções: listar, incluir, editar, cancelar, salvar e excluir.
- Em todos os cadastros, validar ações sem seleção e campos obrigatórios.

## Vínculos

- Cliente/Sistema: selecionar UF, cliente, adicionar sistema, remover sistema, cancelar e salvar.
- Cliente/Serviço: selecionar UF, cliente, sistema, adicionar serviço, remover serviço, cancelar e salvar.
- Confirmar indicação de alterações pendentes.
- Confirmar que remoções usam diálogo customizado.

## Configuração E Importação

- Abrir configuração de sistema vinculado.
- Editar executável, arquivo de configuração, servidor, porta, banco, tipo de conexão, usuário, senha, seção, criptografia e parâmetros.
- Salvar e cancelar mantendo retorno correto ao fluxo de vínculo.
- Importar parâmetros escolhendo UF, cliente e sistema de origem.
- Confirmar toast “Parâmetros importados com sucesso.”

## WebServices

- Confirmar lista de 10 mais acessados.
- Buscar por código de cliente.
- Filtrar por UF e cliente.
- Atualizar URL e confirmar toast de sucesso ou erro controlado.
- Executar WebService por botão explícito.
- Confirmar progresso por etapas e registro de acesso.
- Confirmar falha de configuração com toast claro, sem expor senha.

## Logs

- Confirmar criação de `Logs/ControleDeWebServices-YYYYMMDD.log`.
- Confirmar entrada de startup.
- Confirmar logs de execução, importação, atualização externa e processos.
- Conferir que o log não contém senha, connection string completa ou valores sensíveis.
- Confirmar que falha de escrita do log não impede uso da aplicação.

## Aceite

- Build e testes automatizados passam.
- Fluxos manuais críticos preservam comportamento esperado.
- Nenhum fluxo usa `MessageBox`.
- Logs ajudam diagnóstico sem vazar dado sensível.
- Modelos em `ControleDeWebServices/Modelo` permanecem intocados.
