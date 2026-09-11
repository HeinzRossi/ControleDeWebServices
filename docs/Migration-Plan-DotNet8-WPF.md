# Plano De Migracao: WPF .NET 8/9

## Objetivo

Este documento detalha as sprints de modernizacao do `ControleDeWebServices`, saindo de WPF em .NET Framework 4.8 para WPF em .NET moderno, preferencialmente .NET 8/9. A migracao deve atualizar todos os componentes possiveis, remover componentes sem uso real e substituir componentes incompativeis por similares modernos.

Esta etapa e documental. Nenhum codigo, XAML, `.csproj`, pacote, config ou dependencia deve ser alterado sem uma sprint de implementacao aprovada.

## Baseline Atual

- Branch de origem: `master`.
- Branch da Sprint 0: `codex/sprint-0-baseline`.
- Worktree limpo no momento da criacao da branch da Sprint 0.
- Projeto atual: WPF classico em `.NET Framework 4.8`.
- Formato atual: `.csproj` classico, `packages.config`, referencias por `HintPath` e `App.config`.
- UI atual: MaterialDesignThemes, MaterialDesignColors e FluentWPF.
- Dados e integracoes: Entity Framework 6, SQL Server, Firebird, PostgreSQL/Npgsql, arquivos INI/XML e DLL auxiliar `Comum.Utilitarios.dll`.
- Ponto critico de arquitetura: Views e code-behind ainda concentram consultas, validacoes, navegacao, feedback e parte da orquestracao operacional.
- Relatorio detalhado da Sprint 0: `docs/Sprint-0-Baseline.md`.
- Regra transversal: os modelos em `ControleDeWebServices/Modelo` nao devem ser alterados; evolucoes devem usar ViewModels, servicos, DTOs ou adapters ao redor deles.

## Estrategia Geral

A migracao deve ser feita em branch/prova tecnica propria antes de virar linha principal. A ordem recomendada e provar runtime e componentes primeiro, depois criar camadas, migrar uma tela piloto e so entao avancar para fluxos sensiveis como vinculos, configuracao, banco externo e execucao de WebServices.

Regras de decisao:

- Atualizar o componente quando houver versao compativel com WPF em .NET 8/9.
- Substituir por similar moderno quando o componente bloquear a migracao ou conflitar com a arquitetura desejada.
- Remover quando o componente nao tiver uso real ou puder ser substituido por recurso nativo.
- Isolar temporariamente quando houver DLL proprietaria ou regra sensivel que exija validacao separada.

## Sprint 0: Baseline, Build E Prova Tecnica

Objetivo: congelar o ponto de partida antes de qualquer alteracao real.

Entregaveis:

- Inventario de telas, XAMLs, code-behind, pacotes, DLLs, configs, resources e pasta `Execucao`.
- Checklist de paridade funcional minima.
- Registro de riscos tecnicos e dependencias criticas.
- Protocolo de build atual em WPF/.NET Framework 4.8.

Tasks:

- Confirmar branch `master` e worktree limpo antes da branch de migracao.
- Validar o build atual no ambiente correto de WPF/.NET Framework 4.8.
- Mapear referencias do `.csproj`, imports, `HintPath`, `packages.config`, `App.config` e arquivos copiados para saida.
- Inventariar MaterialDesignThemes, MaterialDesignColors, FluentWPF, Entity Framework 6, Firebird, Npgsql, Ninject, PeanutButter.INI, Microsoft.Xaml.Behaviors e `Comum.Utilitarios.dll`.
- Listar telas e fluxos: shell, WebServices, listas, cadastros, vinculos, configuracoes e `ImportarParametrosDe`.
- Definir paridade minima: app abre, WebServices carrega, menu navega, cadastros salvam, vinculos salvam, importacao copia parametros e WebServices executa fluxo atual.

Criterios de aceite:

- Baseline documentado antes de mexer em runtime.
- Dependencias classificadas inicialmente como atualizar, substituir, remover ou isolar.
- Fluxos criticos e riscos conhecidos.
- Existe checklist para comparar comportamento antes e depois da migracao.

## Sprint 1: Migracao Para .NET 8/9 WPF

Objetivo: planejar e executar a conversao estrutural para WPF moderno em prova tecnica.

Status da execucao inicial: concluida na branch `codex/sprint-1-dotnet-wpf`, com relatorio em `docs/Sprint-1-DotNet-WPF.md`. O projeto foi convertido para SDK-style, `packages.config` foi substituido por `PackageReference`, o target inicial ficou em `net8.0-windows` e o build final concluiu com 0 erros e 1 aviso ativo documentado para Sprint 2.

Entregaveis:

- Projeto convertido para `.csproj` SDK-style na branch de migracao.
- Estrategia de `UseWPF`, `PackageReference`, resources, icone, configs e saida.
- Relatorio dos primeiros erros reais de compatibilidade, se houver.

Tasks:

- Converter o `.csproj` classico para SDK-style com `UseWPF`.
- Substituir `packages.config` por `PackageReference`.
- Remover imports legados de NuGet e referencias por `HintPath` quando houver pacote moderno equivalente.
- Preservar namespace raiz, assembly, icone, resources, XAMLs e configs necessarias.
- Definir como `Config.ini`, configs auxiliares e DLLs necessarias serao copiadas para a saida.
- Ajustar somente o minimo necessario para o projeto restaurar e chegar ao build do runtime alvo.

Criterios de aceite:

- Projeto restaura pacotes pelo formato moderno.
- Build em .NET 8/9 WPF compila ou para em incompatibilidades reais documentadas.
- XAMLs, resources e icone continuam reconhecidos pelo build.
- Nenhuma regra de negocio e refatorada nesta sprint alem do necessario para compilar.

## Sprint 2: Atualizacao De Componentes

Objetivo: atualizar o ecossistema de dependencias e remover o que nao deve seguir para o runtime moderno.

Status da execucao inicial: concluida na branch `codex/sprint-2-componentes`, com relatorio em `docs/Sprint-2-Componentes.md`. Componentes principais foram atualizados, FluentWPF e dependencias diretas sem uso foram removidos, `System.Data.SqlClient` direto foi substituido por `Microsoft.Data.SqlClient` no codigo externo e o build final ficou com 0 erros. O aviso `NU1701` foi removido; avisos herdados de `MessageBox`/dialogs WinForms e codigo legado ficam para as sprints seguintes.

Entregaveis:

- Matriz final de componentes com decisao: atualizar, substituir, remover ou isolar.
- Pacotes atualizados/substituidos no projeto de migracao.
- Lista de impactos por componente.

Tasks:

- Atualizar MaterialDesignThemes e MaterialDesignColors para versoes compativeis com WPF em .NET 8/9.
- Avaliar FluentWPF; remover se `AcrylicWindow` ou resources bloquearem a migracao, substituindo por Window WPF customizada ou biblioteca compativel.
- Validar Entity Framework 6; se bloquear a migracao, planejar fase propria para EF Core.
- Atualizar providers SQL Server, Firebird e Npgsql, mantendo suporte aos tres bancos.
- Remover Ninject se continuar sem uso evidente; se DI for necessario, padronizar `Microsoft.Extensions.DependencyInjection`.
- Atualizar PeanutButter.INI ou substituir por parser INI compativel/implementacao interna pequena.
- Remover dependencia visual de `System.Windows.Forms` usada por `MessageBox`.
- Validar `Comum.Utilitarios.dll`; se incompativel, isolar criptografia/auxiliares atras de interfaces.
- Remover referencias manuais a pacotes `System.*` que se tornarem transitivos ou nativos no .NET moderno.

Criterios de aceite:

- Nenhum componente incompativel permanece sem decisao.
- Componentes sem uso real foram removidos do plano de continuidade.
- Componentes essenciais possuem alternativa compativel ou estrategia de isolamento.
- Build volta a ser validado apos a atualizacao dos componentes.

## Sprint 3: Arquitetura Em Camadas

Objetivo: separar View, ViewModels, Application, Domain e Infrastructure antes da migracao ampla das telas.

Status da execucao inicial: concluida na branch `codex/sprint-3-arquitetura-camadas`, com relatorio em `docs/Sprint-3-Arquitetura-Camadas.md`. Foram criadas as camadas logicas, contratos base, DI com `Microsoft.Extensions.DependencyInjection` e base MVVM com `CommunityToolkit.Mvvm`, sem migrar telas e sem alterar modelos.

Entregaveis:

- Estrutura de camadas criada na solucao/projeto de migracao.
- Contratos base para ViewModels, comandos, navegacao, feedback e contexto.
- Regra de dependencia documentada e aplicada na tela piloto seguinte.

Tasks:

- Criar camadas logicas: Presentation, ViewModels, Application, Domain e Infrastructure.
- Configurar DI com `Microsoft.Extensions.DependencyInjection`.
- Definir `ViewModelBase` com notificacao de propriedades e estados comuns.
- Definir `RelayCommand` ou comando equivalente para `ICommand`.
- Definir `INavigationService` para substituir criacao direta de paginas e uso disperso de `FrameActive`.
- Definir `IToastService` e `IConfirmDialogService`.
- Definir factory/provider para `DadosDBContext`, sem criacao direta na View.
- Definir interfaces de servicos de aplicacao e adaptadores de infraestrutura.

Criterios de aceite:

- Views nao criam `DadosDBContext`.
- Views nao chamam `MessageBox`.
- Views nao executam consultas, persistencia, arquivos, processos ou regras de negocio.
- Code-behind fica limitado a `InitializeComponent` e comportamento visual inevitavel.

## Sprint 4: Shell, Navegacao E Feedback

Objetivo: modernizar a base de interacao do app e remover o padrao visual de `MessageBox`.

Status da execucao inicial: concluida na branch `codex/sprint-4-shell-feedback`, com relatorio em `docs/Sprint-4-Shell-Feedback.md`. O Shell passou a usar `MainWindowViewModel`, navegacao centralizada por `WpfNavigationService`, host de toasts/dialogo customizado e excecao global por toast, mantendo as telas internas para sprints futuras.

Entregaveis:

- Shell com ViewModel de navegacao.
- Host de toasts e dialogos customizados.
- Padrao de item ativo, menu lateral e feedback centralizado.

Tasks:

- Criar ViewModel para `MainWindow` com item ativo e comandos de navegacao.
- Migrar navegacao do `FramePrincipal` para `INavigationService`.
- Substituir `FrameActive` por retorno/navegacao controlados pelo shell ou servico.
- Manter Enter avancando foco como comportamento visual global.
- Criar toasts de sucesso, erro, aviso e informacao.
- Criar dialogo customizado para confirmacoes destrutivas.
- Redirecionar excecoes globais para feedback amigavel sem expor detalhes sensiveis.

Criterios de aceite:

- Navegacao e feedback ficam centralizados.
- Nenhum fluxo principal usa `MessageBox` como experiencia visual.
- Dialogos destrutivos usam overlay/modal proprio do app.
- Shell preserva abertura inicial em WebServices e navegacao dos grupos atuais.

## Sprint 5: Tela Piloto Clientes

Objetivo: validar arquitetura, UX e padrao visual em um fluxo simples antes de replicar.

Status da execucao inicial: concluida na branch `codex/sprint-5-clientes-piloto`, com relatorio em `docs/Sprint-5-Clientes-Piloto.md`. Clientes foi migrado como tela piloto para ViewModel, servico de aplicacao, feedback por toast/dialogo customizado e visual operacional limpo, sem alterar modelos.

Entregaveis:

- Tela de Clientes migrada para ViewModel, comandos e servicos.
- Padrao reaplicavel para Sistemas, Servicos e Secoes.
- Validacoes e feedback sem `MessageBox`.

Tasks:

- Criar `ClientesListViewModel` para consulta, selecao, incluir, editar e excluir.
- Criar `ClienteEditViewModel` para campos, validacoes, salvar e cancelar.
- Criar caso de uso/servico de aplicacao para salvar e excluir Cliente.
- Proteger acoes quando nenhuma linha estiver selecionada.
- Exibir validacoes por toast ou mensagem proxima ao campo.
- Manter retorno para lista apos salvar ou cancelar.
- Aplicar DataGrid, botoes e formulario conforme design system.

Criterios de aceite:

- Fluxo Clientes consulta, inclui, edita, salva, cancela e exclui com paridade.
- Acoes passam por `ICommand`.
- View nao acessa `DadosDBContext`.
- Padrao pode ser reutilizado nas demais telas simples.

## Sprint 6: Cadastros, Listas E Vinculos

Objetivo: aplicar o padrao da tela piloto aos fluxos administrativos e de vinculo.

Status da execucao inicial: concluida na branch `codex/sprint-6-cadastros-vinculos`, com relatorio em `docs/Sprint-6-Cadastros-Vinculos.md`. Sistemas, Servicos, Secoes e vinculos Cliente/Sistema e Cliente/Servico foram migrados para ViewModels, servicos, commands e feedback centralizado, mantendo modelos intactos.

Entregaveis:

- Cadastros/listas de Sistemas, Servicos e Secoes migrados.
- ViewModels para vinculos cliente/sistema e cliente/servico.
- Tratamento de selecao progressiva e alteracoes pendentes.

Tasks:

- Migrar Sistemas, Servicos e Secoes usando o padrao da Sprint 5.
- Criar ViewModels de lista e edicao para cada cadastro.
- Criar ViewModels para `VinculoClienteSistema` e `VinculoClienteServico`.
- Modelar listas de disponiveis/vinculados e comandos Adicionar/Remover.
- Corrigir o bug provavel de `IncluirListaDeSistemas`, onde a condicao de `Contains` parece invertida.
- Proteger selecoes nulas em grids e combos.
- Exibir estado de alteracoes pendentes antes de salvar.
- Substituir confirmacoes de remocao em massa por dialog customizado.

Criterios de aceite:

- Cadastros simples seguem o mesmo padrao de ViewModel/commands.
- Vinculos indicam pendencias e preservam selecao progressiva por UF/Cliente/Sistema.
- Nenhuma acao falha por selecao ausente.
- Remocoes usam dialogo customizado e feedback por toast.

## Sprint 7: Configuracao, Importacao E Banco

Objetivo: isolar configuracao tecnica, importacao de parametros e acesso a bancos externos.

Status da execucao inicial: concluida na branch `codex/sprint-7-config-importacao-banco`, com relatorio em `docs/Sprint-7-Config-Importacao-Banco.md`. Configuracoes de sistema/servico, importacao de parametros e `AtualizarPadroes` foram migrados para ViewModels, servicos, picker isolado, toast/dialogo customizado e SQL parametrizado, mantendo modelos intactos.

Entregaveis:

- Servicos de configuracao de sistemas/servicos.
- Servico de importacao de parametros.
- Adaptadores SQL Server, Firebird e PostgreSQL.
- SQL parametrizado para atualizacao de parametros e URL.

Tasks:

- Mover regras de `CadastroDeInformacoes` e `CadastroDeInformacoesServico` para ViewModels e casos de uso.
- Isolar leitura/escrita de arquivos INI/XML na Infrastructure.
- Isolar criptografia atras de interface propria.
- Criar caso de uso para importar parametros de outro sistema vinculado.
- Corrigir copy para "Parametros importados com sucesso".
- Parametrizar SQL em `AtualizarPadroes`.
- Usar `using`/descarte correto para conexoes, comandos e readers.
- Abrir conexao uma vez por lote de parametros quando possivel.
- Criar adaptadores por banco: SQL Server, Firebird e PostgreSQL.

Criterios de aceite:

- Configuracao e importacao nao executam regra de negocio no code-behind.
- SQL nao concatena valores de usuario diretamente.
- Conexoes e comandos sao descartados corretamente.
- Importacao comunica origem, quantidade/resultado e sucesso/falha por toast.

## Sprint 8: WebServices, Execucao E QA Final

Objetivo: tornar o fluxo operacional observavel, seguro e validado no runtime novo.

Status da execucao inicial: concluida na branch `codex/sprint-8-webservices-execucao-qa`, com relatorio em `docs/Sprint-8-WebServices-Execucao-QA.md`. `WebServices` foi migrado para ViewModel, consulta e execucao operacional foram movidas para servicos, processos foram isolados e o build final ficou limpo.

Entregaveis:

- Caso de uso de execucao de WebServices por etapas.
- Progresso operacional e logs.
- Checklist final de paridade e limpeza pos-migracao.

Tasks:

- Criar ViewModel para WebServices com 10 mais acessados, busca por codigo/UF/cliente, selecao, atualizar URL e executar.
- Preservar duplo clique como atalho, com acao explicita equivalente por botao/comando.
- Separar execucao em etapas: validar configuracoes, atualizar arquivos, atualizar parametros, derrubar servicos, executar servicos e registrar acessos.
- Revisar encerramento de processos por prefixo e registrar falhas.
- Exibir progresso e resultado por status/toast.
- Rodar checklist de paridade funcional.
- Remover referencias quebradas, pacotes antigos, bindings obsoletos, DLLs duplicadas e dependencias visuais legadas.
- Atualizar documentacao com decisoes reais tomadas durante a migracao.

Criterios de aceite:

- Build limpo em .NET 8/9 WPF.
- App abre e carrega WebServices como tela inicial.
- Fluxos principais preservam comportamento esperado.
- Usuario enxerga progresso e resultado da execucao.
- UI nao usa `MessageBox` como experiencia visual.
- Dependencias incompatíveis foram atualizadas, substituidas, removidas ou isoladas com justificativa.

## Sprint 9: Design System WPF E Polimento Visual

Objetivo: aplicar a identidade visual final aprovada com ResourceDictionaries globais, reduzindo estilos repetidos e consolidando o padrão operacional limpo.

Status da execução inicial: concluída na branch `codex/sprint-9-design-system-visual`, com relatório em `docs/Sprint-9-Design-System-Visual.md`. Foram criados tokens e componentes WPF próprios, `App.xaml` passou a carregar o design system, as telas migradas passaram a usar chaves `App.*`, `MessageBox` foi removido da experiência visual restante e `UseWindowsForms=true` saiu do projeto.

Entregáveis:

- ResourceDictionaries globais de tokens e componentes.
- Shell, tabelas, formulários, toasts e diálogos alinhados ao visual aprovado.
- Remoção de aliases locais de botão/header nas Views migradas.
- Documentação do design system aplicado.

Critérios de aceite:

- Build limpo em .NET 8 WPF.
- Views usam estilos globais `App.*` para componentes principais.
- Não há `MessageBox`, `System.Windows.Forms` ou `UseWindowsForms` como experiência visual.
- Modelos permanecem intocados.

## Sprint 10: Testes, Logs E Hardening Operacional

Objetivo: criar a base automatizada de testes e tornar falhas operacionais observáveis sem alterar regras de negócio.

Status da execução inicial: concluída na branch `codex/sprint-10-testes-logs-hardening`, com relatório em `docs/Sprint-10-Testes-Logs-Hardening.md`. Foi criado projeto de testes xUnit, adicionados logs silenciosos por padrão e endurecido o fluxo de execução para tratar exceções inesperadas por toast.

Entregáveis:

- Projeto `ControleDeWebServices.Tests`.
- Logging via `ILogger<>` com `NullLogger` inicial.
- Resultado estruturado para encerramento de processos.
- Testes unitários iniciais para ViewModels e contrato operacional.

Critérios de aceite:

- Build e testes passam sem erros.
- Nenhum `MessageBox` ou dependência visual WinForms retorna.
- Falhas operacionais sensíveis possuem caminho de log ou resultado estruturado.
- Modelos permanecem intocados.

## Sprint 11: Observabilidade, Testes De Serviços E QA Manual

Objetivo: configurar logging real em arquivo local, ampliar testes e formalizar QA manual com banco/configuração real.

Status da execução inicial: concluída na branch `codex/sprint-11-observabilidade-qa`, com relatório em `docs/Sprint-11-Observabilidade-QA.md`. Foi criado logger local por data, sanitização básica de dados sensíveis, checklist manual e ampliação dos testes automatizados para 12 cenários.

Entregáveis:

- Logger local em `Logs/ControleDeWebServices-YYYYMMDD.log`.
- Provider/factory de logging integrado ao `Microsoft.Extensions.Logging`.
- Checklist manual em `docs/QA-Manual-Checklist.md`.
- Testes adicionais para logging, cadastros, importação e operação.

Critérios de aceite:

- Build e testes passam sem erros.
- Logs são criados sem quebrar startup.
- Logs não registram senha ou connection string completa.
- Modelos permanecem intocados.

## Sprint 12: QA Real, Hardening Fino E Preparacao De Publicacao

Objetivo: validar a base modernizada, preparar publicacao/teste em maquina limpa e registrar pendencias que dependem de banco/configuracao real.

Status da execucao inicial: concluida na branch `codex/sprint-12-qa-real-hardening`, com relatorio em `docs/Sprint-12-QA-Real-Hardening.md`. Nao houve falha automatica reproduzida que justificasse alteracao de codigo; foi criado checklist de publicacao em `docs/Publish-Checklist.md`.

Entregaveis:

- Relatorio de QA tecnico da Sprint 12.
- Checklist de publicacao/teste em maquina limpa.
- Validacao automatizada Debug, Release, testes, startup e logs.
- Registro das pendencias reais para ambiente com banco/configuracao real.

Criterios de aceite:

- Build Debug/Release e testes passam sem erros.
- App inicia e cria log local.
- QA manual com banco real fica registrado como pendencia controlada quando nao executavel localmente.
- Modelos permanecem intocados.

## Sprint 13: Publicacao Controlada E Validacao Em Pasta Limpa

Objetivo: gerar e validar uma pasta publicada em Release antes de escolher instalador final.

Status da execucao inicial: concluida na branch `codex/sprint-13-publicacao-local`, com relatorio em `docs/Sprint-13-Publicacao-Local.md`. O publish local foi gerado em `artifacts/publish/ControleDeWebServices`, iniciou com sucesso por curto periodo e criou log local.

Entregaveis:

- Pasta publicada local ignorada pelo Git.
- Checklist de publicacao atualizado com resultado real.
- Inventario dos artefatos principais copiados pelo publish.
- Registro das pendencias para validacao em maquina limpa.

Criterios de aceite:

- Publish Release executa sem erro.
- Pasta publicada contem executavel, configs, DLL auxiliar, imagens e dependencias.
- App publicado inicia e cria log local.
- Modelos permanecem intocados.

## Sprint 14: Pacote ZIP Portavel Para Validacao

Objetivo: criar um pacote `.zip` portavel a partir do publish Release validado, sem incluir credenciais ou logs.

Status da execucao inicial: concluida na branch `codex/sprint-14-pacote-zip`, com relatorio em `docs/Sprint-14-Pacote-Zip.md`. O ZIP foi gerado em `artifacts/packages/ControleDeWebServices-net8-windows.zip`, extraido para validacao e iniciado com sucesso por curto periodo.

Entregaveis:

- Pacote ZIP portavel gerado em `artifacts/`.
- Checksum SHA256 registrado.
- Template seguro de `Config.ini`.
- Checklist de publicacao atualizado com o fluxo oficial de ZIP.

Criterios de aceite:

- ZIP nao inclui `Config.ini` real nem logs.
- App extraido inicia e cria log local.
- Artefatos publicados permanecem ignorados pelo Git.
- Modelos permanecem intocados.

## Sprint 15: Ajustes De Cadastros

Objetivo: corrigir interação das telas de cadastro simples durante inclusão/edição e tornar as grids somente leitura.

Status da execução inicial: concluída na branch `codex/sprint-15-ajustes-cadastros`, com relatório em `docs/Sprint-15-Ajustes-Cadastros.md`. Clientes, Sistemas, Serviços e Seções passaram a bloquear ações de lista quando o formulário está aberto.

Entregáveis:

- Ações de lista condicionadas por `CanUseListActions`.
- Grids de cadastro simples com `IsReadOnly="True"`.
- Grids e botões de linha desabilitados durante inclusão/edição.
- Testes de regressão para bloqueio de ações.

Critérios de aceite:

- Não é possível iniciar inclusão/edição concorrente.
- Não é possível editar célula diretamente na grid.
- Salvar/cancelar reabilita lista e ações.
- Modelos permanecem intocados.

## Sprint 16: Ajustes De Vinculos E Grids Readonly

Objetivo: aplicar bloqueio de acoes concorrentes nas telas de vinculo e tornar todas as `DataGrid` somente leitura.

Status da execucao inicial: concluida na branch `codex/sprint-16-ajustes-vinculos`, com relatorio em `docs/Sprint-16-Ajustes-Vinculos.md`. As listas de vinculo agora bloqueiam acoes do topo durante edicao/configuracao, e as grids do projeto foram padronizadas como readonly.

Entregaveis:

- Todas as `DataGrid` com `IsReadOnly="True"`.
- Acoes de vinculo condicionadas por `CanUseListActions`.
- Grids principais de vinculo desabilitadas durante edicao/configuracao.
- Layout dos botoes `Adicionar` e `Remover` alinhado a direita nos paines internos.

Criterios de aceite:

- Nao e possivel iniciar inclusao, edicao ou configuracao concorrente em vinculos.
- Nao e possivel editar celulas diretamente nas grids.
- Botoes internos nao sobrepoem o texto principal.
- Modelos permanecem intocados.

## Sprint 17: Corrige Copy E Acoes De Exclusao

Objetivo: alinhar a nomenclatura dos botoes destrutivos e garantir que os comandos de exclusao sejam acionaveis por binding.

Status da execucao inicial: concluida na branch `codex/sprint-17-corrige-excluir-acoes`, com relatorio em `docs/Sprint-17-Corrige-Excluir-Acoes.md`.

Entregaveis:

- Botoes destrutivos exibindo `Excluir`.
- `Remover` preservado apenas nos paineis internos de desvinculo.
- Dialogos e toasts de vinculos usando copy de exclusao.
- Testes exercitando `ExcluirCommand` diretamente.

Criterios de aceite:

- Excluir com selecao valida chama o servico correto.
- Excluir sem selecao valida mostra toast de aviso.
- Excluir fica bloqueado durante edicao/configuracao.
- Modelos permanecem intocados.

## Checklist De Paridade Funcional

- App abre e carrega WebServices como tela inicial.
- Menu lateral navega para cadastros, vinculos e operacao.
- Enter continua avancando foco nos campos.
- Clientes, Sistemas, Serviços e Seções permitem consultar, incluir, editar, salvar, cancelar e excluir.
- Vinculos permitem selecionar UF/Cliente/Sistema, adicionar/remover itens e salvar alteracoes.
- Configuracoes permitem editar arquivos, conexao, credenciais, secao, criptografia e parametros.
- Importacao copia parametros de outro sistema vinculado e mostra mensagem correta.
- WebServices lista 10 mais acessados, busca por codigo/UF/cliente, atualiza URL e executa fluxo operacional.

## Ordem Recomendada De Execucao

1. Sprint 0: Baseline, Build E Prova Tecnica.
2. Sprint 1: Migracao Para .NET 8/9 WPF.
3. Sprint 2: Atualizacao De Componentes.
4. Sprint 3: Arquitetura Em Camadas.
5. Sprint 4: Shell, Navegacao E Feedback.
6. Sprint 5: Tela Piloto Clientes.
7. Sprint 6: Cadastros, Listas E Vinculos.
8. Sprint 7: Configuracao, Importacao E Banco.
9. Sprint 8: WebServices, Execucao E QA Final.
10. Sprint 9: Design System WPF E Polimento Visual.
11. Sprint 10: Testes, Logs E Hardening Operacional.
12. Sprint 11: Observabilidade, Testes De Serviços E QA Manual.
13. Sprint 12: QA Real, Hardening Fino E Preparacao De Publicacao.
14. Sprint 13: Publicacao Controlada E Validacao Em Pasta Limpa.
15. Sprint 14: Pacote ZIP Portavel Para Validacao.
16. Sprint 15: Ajustes De Cadastros.
17. Sprint 16: Ajustes De Vinculos E Grids Readonly.
18. Sprint 17: Corrige Copy E Acoes De Exclusao.

## Assumptions

- O destino preferencial e WPF em .NET 8/9.
- O app continua desktop WPF, sem migrar para web, MAUI ou WinUI.
- Todos os componentes devem ser atualizados quando compativeis.
- Componentes incompativeis devem ser substituidos por similares modernos ou removidos quando desnecessarios.
- A separacao profunda de View, ViewModels, Application, Domain e Infrastructure e requisito da modernizacao.
- Cada sprint deve preservar paridade funcional antes de avancar para a proxima.
- Os modelos atuais devem ser preservados sem mudancas de classes, propriedades, enums, atributos ou relacionamentos.
