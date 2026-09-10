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

## Assumptions

- O destino preferencial e WPF em .NET 8/9.
- O app continua desktop WPF, sem migrar para web, MAUI ou WinUI.
- Todos os componentes devem ser atualizados quando compativeis.
- Componentes incompativeis devem ser substituidos por similares modernos ou removidos quando desnecessarios.
- A separacao profunda de View, ViewModels, Application, Domain e Infrastructure e requisito da modernizacao.
- Cada sprint deve preservar paridade funcional antes de avancar para a proxima.
- Os modelos atuais devem ser preservados sem mudancas de classes, propriedades, enums, atributos ou relacionamentos.
