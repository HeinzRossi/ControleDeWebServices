# Roadmap UI/UX: ControleDeWebServices

## Objetivo

Este documento descreve um plano futuro de modernizacao visual para o ControleDeWebServices. Ele nao altera a implementacao atual. A finalidade e orientar uma evolucao WPF profunda, com interface operacional limpa, consistente e mais confortavel para uso administrativo, sustentada por separacao futura entre View, ViewModels, Application, Domain e Infrastructure.

Atualizacao considerada: a solucao passou a incluir a tela auxiliar `ImportarParametrosDe.xaml`, o utilitario `ConverterListaGenerica.cs` e dependencias de execucao relacionadas a PostgreSQL/Npgsql, alem das dependencias ja existentes para Firebird.

Baseline Git considerado: branch `master`, worktree limpo, projeto ainda em WPF classico com `.NET Framework 4.8`, `.csproj` classico, `packages.config`, `HintPath`, `App.config` e dependencias copiadas para `Execucao`. O detalhamento tecnico sprint a sprint da migracao fica em `docs/Migration-Plan-DotNet8-WPF.md`.

## Diagnostico Atual

O projeto e uma aplicacao WPF em .NET Framework 4.8. A direcao tecnica futura aprovada e migrar para WPF em .NET moderno, preferencialmente .NET 8/9, atualizando todos os componentes necessarios. Componentes sem compatibilidade devem ser removidos e substituidos por alternativa similar moderna quando ainda forem necessarios. A solucao ja referencia MaterialDesignThemes, MaterialDesignColors e FluentWPF, que devem ser avaliados na fase de compatibilidade.

Pontos observados no XAML atual:

- Uso repetido de cores fixas em telas diferentes, como azul para acoes e headers, vermelho para acoes e verde para incluir.
- Fundos e textos definidos diretamente como `WhiteSmoke`, `Black` e `White`.
- Larguras e alturas fixas em janelas, paginas, tabelas, campos e botoes.
- DataGrids com estilos de cabecalho e colunas de acao repetidos em varias telas.
- Formularios estruturados com muitos `StackPanel`, medidas fixas e pouca flexibilidade de redimensionamento.
- Botoes com mistura de texto, icones e cores sem uma hierarquia visual documentada.
- Navegacao lateral baseada em `TreeView`, com capitalizacao e acentuacao inconsistentes.
- Mensagens de validacao e confirmacao pouco padronizadas.
- Ausencia de um design system proprio documentado para tokens, componentes e regras de uso.
- Nova janela de importacao com fluxo compacto de UF, Cliente, Sistema, Importar e Fechar, ainda seguindo o mesmo padrao visual antigo das demais telas.
- Cenarios de conexao mais amplos, agora incluindo Firebird e PostgreSQL/Npgsql, exigem que telas de configuracao tratem tipo de conexao como informacao operacional clara.

## Estudo Do Codigo

O estudo do code-behind mostra que a modernizacao visual precisa respeitar fluxos ja existentes, e nao apenas trocar cores e controles.

### Shell principal

- `MainWindow` inicializa `DadosDBContext`, registra Enter como atalho global para avancar foco em `TextBox`, `Button` e `ComboBox`, e carrega `WebServices` ao iniciar.
- A navegacao principal troca o conteudo de `FramePrincipal` para listas de cadastro, listas de vinculo e operacao WebServices.
- Os grupos `Cadastrar` e `Vincular` sao expandidos/recolhidos manualmente e removem a selecao do item-pai apos o clique.
- O shell atual usa janela sem borda, AcrylicWindow, menu lateral em `TreeView` e area central com frame.

### Listas e cadastros simples

- `ListaDeClientes`, `ListaDeSistemas`, `ListaDeServicos` e `ListaDeSecoes` seguem o mesmo padrao: carregam registros no construtor, deixam a consulta visivel e escondem `FrameInserirtEditar`.
- A acao `Incluir` esconde a consulta, mostra o frame interno e cria a pagina de cadastro correspondente.
- A acao `Editar` usa o item selecionado no DataGrid, abre o cadastro em modo edicao e passa o frame ativo para permitir retorno.
- A acao `Excluir` usa confirmacao bloqueante por `MessageBox`, remove o registro e recarrega a lista.
- Alguns cadastros validam campos obrigatorios com `MessageBox`, enquanto outros persistem sem a mesma validacao visual, o que pede padronizacao.

### Vinculos

- `ListaVinculosSistema` lista clientes que ja possuem sistemas vinculados; ao selecionar um cliente, carrega os sistemas vinculados em outra grade.
- `ListaVinculoServico` usa fluxo em cadeia: Cliente selecionado carrega Sistemas; Sistema selecionado carrega Servicos.
- `VinculoClienteSistema` e `VinculoClienteServico` trabalham com colecoes de disponiveis e vinculados, movendo itens entre listas por `Adicionar` e `Remover`.
- Em modo insercao, os filtros aparecem progressivamente conforme ha dados disponiveis. Em modo edicao, UF/Cliente/Sistema podem ficar bloqueados.
- As remocoes em massa de vinculos usam `MessageBox` e precisam de dialogo customizado mais claro sobre impacto.

### Configuracoes e parametros

- `CadastroDeInformacoes` edita informacoes do sistema vinculado: arquivo executavel, arquivo de configuracao, servidor, porta, banco, tipo de conexao, usuario, senha, secao e criptografia.
- A tela tambem carrega parametros existentes, permite incluir novos parametros e marca itens para exclusao via `StatusServico`.
- `CadastroDeInformacoesServico` replica parte do fluxo para servicos, com arquivos, conexao, usuario e senha.
- Os tipos de conexao ja incluem Firebird, SQL Server e PostgreSQL, e a documentacao visual deve tratar isso como dado operacional importante.
- A porta possui restricao numerica no input, mas o feedback visual de erro/validacao ainda deve ser padronizado.

### Operacao WebServices

- `WebServices` carrega os 10 sistemas mais acessados, lista UFs validas e permite busca por codigo do cliente, UF e cliente.
- O duplo clique em `Grid10Mais` ou `GridSistemas` executa uma cadeia operacional: validar configuracoes, atualizar arquivos, atualizar parametros, derrubar servicos, executar servicos e atualizar quantidade de acessos.
- Os botoes de URL usam `AtualizarPadroes.AtualizarURL` sobre a linha selecionada.
- Por ser uma operacao com efeitos externos, a UI futura deve ter feedback de progresso, sucesso e erro por toast/status, alem de affordance explicita para a acao principal.

### Importacao de parametros

- `ImportarParametrosDe` abre como janela auxiliar a partir de `CadastroDeInformacoes`.
- O fluxo seleciona UF, Cliente e Sistema de origem, busca sistemas com parametros e copia esses parametros para o sistema atual.
- A mensagem final atual fala "URL atualizada com Sucesso!", mas o fluxo real e importacao de parametros. A copy correta deve ser "Parametros importados com sucesso".
- Importar deve ser acao primaria/positiva; nunca acao vermelha ou destrutiva.

## Direcao Visual Recomendada

A direcao recomendada e **operacional limpo**: uma interface clara, profissional, densa e previsivel. O produto deve priorizar velocidade de leitura, confianca em acoes administrativas, consistencia entre telas e baixo esforco cognitivo.

Principios:

- Clareza acima de decoracao.
- Padrao unico para botoes, tabelas, formularios e navegacao.
- Cores semanticas: azul para acao primaria, verde para sucesso/criacao, vermelho para risco, ambar para alerta.
- Componentes compartilhados antes de ajustes isolados.
- Layouts redimensionaveis onde isso reduzir fragilidade.
- Views focadas em layout, bindings e estados visuais, sem regras de negocio no code-behind.
- Estados visuais completos: default, hover, focus, active, disabled, loading, error e success.
- Fluxos auxiliares, como importacao de parametros, devem ser curtos, guiados e visualmente consistentes com o restante do produto.
- Mensagens informativas, sucesso, aviso e erro devem ser exibidas por toast. `MessageBox` nao deve ser usado como padrao visual.
- Confirmacoes bloqueantes devem ser dialogos customizados do app, com foco preso e botoes claros.

## Modelos Visuais Atualizados

Os modelos de tela do projeto devem ser organizados por tipo de fluxo:

- **Lista / Consulta:** Clientes, Sistemas, Serviços, Seções, listas de Vínculos e WebServices. Deve usar toolbar superior, filtros compactos, DataGrid padronizado, acoes de linha e rodape com status/paginacao quando necessario.
- **Cadastro Simples:** Cliente, Sistema, Serviço e Seção. Deve usar formulario curto, grupos claros, acoes Salvar/Cancelar e validacao proxima ao campo.
- **Configuração / Informações:** CadastroDeInformacoes, CadastroDeInformacoesServico e ImportarParametrosDe. Deve explicitar origem, arquivos, conexao, credenciais, parametros e resultado esperado.
- **Vínculo Duplo Painel:** VinculoClienteSistema e VinculoClienteServico. Deve separar disponiveis e vinculados, com acoes diretas Adicionar/Remover.
- **Operação WebServices:** consulta operacional, atualizacao de URL e configuracao, usando o mockup aprovado como referencia mestre.

## Padrao De Feedback E Mensagens

O padrao futuro deve substituir `MessageBox` por componentes visuais do proprio app.

- **Success toast:** usado apos salvar, atualizar URL, importar parametros, vincular ou desvincular com sucesso.
- **Error toast:** usado quando uma operacao falha, com texto curto e orientacao de recuperacao.
- **Warning toast:** usado para pendencias de selecao, campos obrigatorios e estados sem dados.
- **Info toast:** usado para feedback neutro, como configuracoes carregadas ou nenhum registro encontrado.
- **Confirm dialog custom:** usado para decisoes destrutivas, como excluir registro, excluir vinculos do cliente ou excluir vinculos do sistema.

Regras:

- Toasts ficam no canto superior direito da area de conteudo, com opcao de fechar e pilha limitada.
- Mensagens repetidas atualizam o toast existente em vez de criar uma lista longa.
- Dialogos customizados devem ter overlay, foco preso, Escape para cancelar e acao destrutiva em vermelho.
- Nenhum fluxo futuro deve apresentar `MessageBox` como experiencia visual padrao.
- A copy deve falar da acao real. Exemplo: importacao deve dizer "Parametros importados com sucesso", nao "URL atualizada com Sucesso!".

## Melhorias De Codigo

Esta etapa e somente documental. Nenhum arquivo C#, XAML, `.csproj`, pacote, configuracao ou estilo deve ser alterado a partir desta secao sem uma sprint de implementacao aprovada.

O estudo do codigo real tambem apontou melhorias tecnicas importantes para sustentar a evolucao visual e reduzir risco operacional:

- `MessageBox` aparece em validacoes, exclusoes, excecoes globais, atualizacao de URL e importacao de parametros. O padrao futuro deve ser toast para feedback e dialogo customizado para confirmacoes bloqueantes.
- `DadosDBContext` e criado diretamente em varias paginas e classes de operacao, dificultando teste, descarte consistente e controle de transacoes.
- Listas e cadastros de Clientes, Sistemas, Serviços e Seções repetem fluxo de consulta, frame interno, incluir, editar, excluir, salvar e cancelar.
- `WebServices.xaml.cs` concentra consultas, selecao, atualizacao de URL e execucao operacional longa no code-behind.
- `AtualizarPadroes` monta SQL por concatenacao de strings e abre/fecha conexoes dentro de loops, com risco de erro, injecao e manutencao dificil.
- `ExecutarSistemaServico` atualiza arquivos, altera parametros, encerra processos, inicia executaveis e incrementa acessos em uma cadeia unica, sem feedback de progresso granular.
- `VinculoClienteSistema` possui bug provavel ao registrar alteracoes: `IncluirListaDeSistemas` adiciona o item somente quando a lista ja contem o mesmo item.
- `ImportarParametrosDe` conclui importacao com a mensagem incorreta "URL atualizada com Sucesso!", embora a acao real seja copiar parametros.
- A separacao futura deve mover regras de tela para ViewModels, casos de uso para Application e banco/arquivos/processos para Infrastructure.
- O plano especifico de migracao para WPF em .NET 8/9 fica documentado em `docs/Migration-Plan-DotNet8-WPF.md`.

Roadmap tecnico recomendado:

- **Fase 0: Avaliacao de migracao para .NET 8/9 WPF.** Inventariar dependencias, validar SDK-style, mapear APIs legadas e classificar componentes em atualizar, substituir, remover ou isolar.
- **Fase 1: Camadas, DI e feedback padronizado.** Criar base Presentation/ViewModels/Application/Domain/Infrastructure, padrao de toast e dialogo customizado para substituir `MessageBox`.
- **Fase 2: Validacoes e selecao segura.** Padronizar validacoes de campos obrigatorios, proteger acoes sem linha selecionada e evitar acessos nulos em combos/grids.
- **Fase 3: ViewModels e servicos de aplicacao.** Extrair regras de cadastros, vinculos e WebServices para ViewModels e servicos, reduzindo code-behind.
- **Fase 4: Banco e conexoes.** Parametrizar comandos SQL, usar `using` para conexoes/comandos/readers, evitar abrir conexao por parametro e padronizar suporte a Firebird, SQL Server e PostgreSQL/Npgsql.
- **Fase 5: Execucao, logs e testes.** Isolar encerramento/inicio de processos, adicionar progresso operacional, registrar falhas e cobrir fluxos criticos com testes ou checklist manual repetivel.

Criterios de aceite da documentacao tecnica:

- O roadmap cobre `MainWindow`, `WebServices`, cadastros, vinculos, configuracoes, `AtualizarPadroes`, `ExecutarSistemaServico`, `DadosDBContext` e `ImportarParametrosDe`.
- A prioridade inicial permanece estabilizacao UX: primeiro feedback, validacao e confirmacoes consistentes.
- O destino tecnico preferencial fica documentado como .NET 8/9 WPF.
- A separacao profunda entre View, ViewModel, Application, Domain e Infrastructure fica registrada como direcao oficial.
- A modernizacao visual deve acompanhar a migracao de runtime e a separacao View/ViewModel/Services, sem depender de code-behind para regra de negocio.
- Nenhuma recomendacao desta secao instrui alteracao imediata no projeto.
- O documento tecnico detalhado fica em `docs/Code-Improvement-Roadmap.md`.

## Sprint 0: Diagnostico, Runtime E Validacao Do Ambiente

Objetivo: preparar a base para uma modernizacao segura, incluindo avaliacao de migracao para .NET 8/9 WPF, sem iniciar alteracoes visuais diretamente.

Tasks:

- Confirmar o fluxo correto de build para WPF .NET Framework 4.8, preferencialmente via Visual Studio/MSBuild compativel.
- Registrar a falha do `dotnet build` moderno como limitacao de validacao, ja que ele nao gerou os artefatos XAML esperados.
- Inventariar dependencias e componentes que precisam de atualizacao, substituicao, remocao ou isolamento para WPF em .NET 8/9.
- Planejar conversao futura do projeto para `.csproj` SDK-style com `UseWPF`.
- Planejar estrutura futura em camadas: Presentation, ViewModels, Application, Domain e Infrastructure.
- Inventariar telas principais: janela principal, cadastros, listas, vinculos e WebServices.
- Incluir `ImportarParametrosDe.xaml` no inventario de telas auxiliares de configuracao/importacao.
- Mapear componentes repetidos: botoes de acao, headers de DataGrid, campos, combos, paineis, mensagens e navegacao.
- Mapear valores hard-coded que devem virar recursos compartilhados.
- Registrar tipos de conexao suportados visualmente, incluindo Firebird e PostgreSQL/Npgsql.
- Definir ordem de migracao por risco: primeiro estilos compartilhados, depois telas simples, depois fluxos de vinculo e WebServices.

Criterios de aceite:

- Ambiente de validacao definido e documentado.
- Destino de runtime registrado como .NET 8/9 WPF.
- Dependencias criticas para migracao classificadas como atualizar, substituir, remover ou isolar.
- Lista de telas e padroes repetidos concluida.
- Nova tela de importacao classificada no modelo visual correto.
- Riscos de build e compatibilidade conhecidos antes de qualquer refatoracao.
- Direcao de separacao entre View e codigo documentada antes de iniciar telas piloto.

## Sprint 1: Documentacao Do Design System

Objetivo: transformar a direcao visual em um sistema reutilizavel para WPF.

Tasks:

- Criar ou manter `DESIGN.md` como referencia visual oficial.
- Definir tokens de cor, tipografia, espacamento, borda, estados e elevacao.
- Especificar estilos compartilhados para botoes: primary, secondary, danger, success, icon-only e table-action.
- Especificar estilos compartilhados para DataGrid: header, row, selection, action column e empty state.
- Especificar estilos para TextBox, PasswordBox, ComboBox, GroupBox, Card e areas de feedback.
- Especificar estilo de janela auxiliar para importacao, selecao de origem e confirmacao.
- Definir padrao de linguagem para acoes, mensagens e confirmacoes.
- Definir regras para quando usar MaterialDesignThemes e FluentWPF.
- Definir componente visual de toast para sucesso, erro, aviso e informacao.
- Definir dialogo customizado para confirmacoes bloqueantes/destrutivas, substituindo `MessageBox`.

Criterios de aceite:

- `DESIGN.md` descreve o sistema visual proposto de forma aplicavel ao WPF.
- Cada componente comum tem uso, estados e restricoes documentados.
- Fluxos auxiliares possuem padrao proprio sem parecerem telas desconectadas do shell principal.
- `MessageBox` esta documentado como padrao a substituir.
- Toasts e dialogos customizados possuem regras claras de uso.
- As cores passam a ter papel semantico claro.

## Sprint 2: Shell, Navegacao E Janela Principal

Objetivo: redesenhar conceitualmente a estrutura principal da aplicacao.

Tasks:

- Planejar um shell com menu lateral mais limpo, item ativo evidente e grupos de navegacao claros.
- Padronizar titulo da aplicacao, titulo da pagina atual e area de conteudo.
- Definir comportamento de redimensionamento, evitando `ResizeMode="NoResize"` se a evolucao permitir.
- Documentar alternativa para navegacao: manter `TreeView` estilizado ou substituir por lista de navegacao dedicada.
- Corrigir padroes de texto: `Secao` para `Seção`, `serviços` para `Serviços`, `Tipo Conexao` para `Tipo de Conexão`.
- Definir regras para animacao do menu: curta, funcional e sem atrasar o usuario.

Criterios de aceite:

- Estrutura visual principal documentada.
- Navegacao com estados default, hover, selected e focus especificados.
- Comportamento esperado para resize e conteudo documentado.

## Sprint 3: Listas E Tabelas

Objetivo: padronizar as telas de consulta e manutencao.

Tasks:

- Definir template de tela de lista com toolbar superior, acao primaria e DataGrid.
- Padronizar acoes de linha: editar, excluir, configurar e atualizar URL.
- Definir quando a acao deve ter texto, icone ou ambos.
- Especificar alinhamento e largura de colunas: codigos compactos, nomes expansivos e acoes com largura controlada.
- Definir estados de tabela: carregando, sem dados, erro, selecao e hover.
- Planejar migracao das listas de Clientes, Sistemas, Serviços, Seções e Vínculos.

Criterios de aceite:

- Existe um modelo visual unico para telas de lista.
- Colunas de acao seguem uma regra comum.
- Estados vazios e de erro estao especificados.

## Sprint 4: Formularios E Vinculos

Objetivo: melhorar clareza, agrupamento e ergonomia dos fluxos de cadastro e vinculo.

Tasks:

- Definir template de formulario com toolbar, agrupamento de campos e area de feedback.
- Separar conceitualmente grupos como dados basicos, conexao, arquivos e parametros.
- Padronizar campos obrigatorios, erros e mensagens de validacao.
- Planejar telas de vinculo com dois paineis: disponiveis e vinculados.
- Definir acoes de adicionar/remover com botao consistente e feedback imediato.
- Documentar comportamento para listas vazias, cliente sem sistema, sistema sem servico e falhas de carregamento.
- Planejar importacao de parametros como fluxo de apoio: escolher UF, Cliente e Sistema; confirmar Importar; apresentar resultado ou erro.
- Garantir que configuracoes de conexao representem Firebird e PostgreSQL/Npgsql sem criar estilos diferentes por banco.

Criterios de aceite:

- Formularios possuem hierarquia previsivel.
- Fluxos de vinculo deixam claro o que esta disponivel e o que ja esta associado.
- Mensagens e validacoes usam linguagem consistente.
- Janela de importacao tem hierarquia clara, acao primaria correta e fechamento secundario.

## Sprint 5: Acessibilidade, Estados E Polimento

Objetivo: fechar a modernizacao futura com qualidade visual e funcional.

Tasks:

- Revisar contraste de textos, botoes, headers e estados de selecao.
- Verificar navegacao por teclado, ordem de tabulacao e foco visivel.
- Garantir estados default, hover, focus, active, disabled, loading, error e success nos componentes comuns.
- Revisar densidade de linhas, paddings, alinhamentos e tamanhos minimos.
- Padronizar icones e tooltips.
- Revisar mensagens de erro, sucesso e confirmacao.
- Testar fluxos principais: incluir, editar, excluir, salvar, cancelar, vincular, desvincular, atualizar URL e buscar.
- Testar fluxo auxiliar de importacao: selecionar UF, cliente, sistema, importar e fechar.
- Validar toasts para sucesso, erro, aviso e informacao.
- Validar dialogos customizados para exclusoes e remocoes de vinculos.

Criterios de aceite:

- Interface consistente nas telas principais.
- Fluxos criticos testados manualmente.
- Nenhuma acao destrutiva fica ambigua.
- Nenhuma mensagem padrao depende de `MessageBox`.
- Tabelas e formularios permanecem legiveis em diferentes tamanhos de janela suportados.

## Ordem Recomendada De Execucao Futura

1. Avaliar migracao para .NET 8/9 WPF, build e compatibilidade de componentes.
2. Atualizar componentes compativeis e substituir/remover componentes incompativeis em uma prova tecnica.
3. Criar estrutura de camadas, DI, navegacao, feedback e contratos base.
4. Criar ResourceDictionaries e estilos compartilhados.
5. Aplicar estilos e ViewModel primeiro em uma tela simples de lista.
6. Aplicar estilos e ViewModel em uma tela simples de formulario.
7. Migrar demais listas e formularios por repeticao controlada.
8. Modernizar shell e navegacao.
9. Refinar vinculos, importacao e WebServices.
10. Fazer QA visual, acessibilidade e documentacao final.

Para execucao tecnica, usar o detalhamento de sprints em `docs/Migration-Plan-DotNet8-WPF.md`, que separa baseline, migracao do runtime, componentes, camadas, shell/feedback, tela piloto, cadastros/vinculos, configuracao/banco e WebServices/QA final.

## Fora De Escopo Neste Documento

- Alterar codigo, XAML, projeto, configs ou pacotes.
- Atualizar MaterialDesignThemes, FluentWPF ou qualquer dependencia nesta etapa documental.
- Migrar runtime nesta etapa documental.
- Migrar para WinUI, MAUI ou web.
- Definir wireframes finais de cada tela.
- Executar refatoracao de componentes.

## Checklist De Pronto Para Implementar

- Build WPF validado no ambiente correto.
- Decisao sobre atualizar, substituir, remover ou isolar bibliotecas tomada.
- Plano de migracao para .NET 8/9 WPF validado em branch/prova propria.
- Estrutura de camadas e estrategia de separacao View/codigo aprovadas.
- `DESIGN.md` aprovado como referencia visual.
- Primeira tela piloto escolhida.
- Modelo visual para Importar Parametros De aprovado junto com configuracoes.
- Criterios de regressao definidos para dados, navegacao e acoes.
- Backup ou branch de trabalho criado antes de alteracoes futuras.

## Observacoes

Ha mudancas ja existentes no repositorio em arquivos de configuracao e projeto. Qualquer implementacao futura deve preservar essas alteracoes e evitar reversoes acidentais.

Este roadmap assume modernizacao WPF profunda e incremental, com destino tecnico preferencial em .NET 8/9 WPF. Migracoes para WinUI, MAUI ou web ficam fora deste documento.
