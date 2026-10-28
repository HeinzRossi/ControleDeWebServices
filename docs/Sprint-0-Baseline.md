# Sprint 0: Baseline, Build E Prova Tecnica

## Objetivo

Registrar o estado atual do `ControleDeWebServices` antes da modernizacao para WPF em .NET 8/9. Esta sprint cria um retrato de referencia para as proximas etapas, sem alterar codigo funcional, XAML, modelos, projeto, pacotes ou configuracoes.

## Estado Do Git

- Data do levantamento: 2026-09-10.
- Branch de trabalho: `codex/sprint-0-baseline`.
- Branch de origem: `master`.
- Estado inicial observado apos troca de branch: worktree limpo.
- Commits recentes observados:
  - `c2a31b7 remover pasta execução`
  - `47c1a24 Atualizar documentação`
  - `8d600f2 Adicionar arquivos de projeto.`

Observacao: os documentos de planejamento, mockups conceituais e pacote de icones ja fazem parte do baseline atual. O novo icone permanece em `docs/Icones` e nao substitui o `agt_web.ico` do aplicativo.

## Regra De Preservacao Dos Modelos

Os modelos em `ControleDeWebServices/Modelo` nao devem ser alterados durante a modernizacao:

- nao renomear classes, propriedades ou enums existentes;
- nao mudar relacionamentos, atributos de tabela/chave ou estrutura persistida;
- nao mover regra de banco para dentro dos modelos;
- usar ViewModels, DTOs, adapters e servicos ao redor dos modelos quando a UI ou arquitetura precisar evoluir.

Esta regra vale para todas as sprints, inclusive migracao para .NET 8/9, separacao View/ViewModel/Services e refatoracoes de banco.

## Inventario Da Solucao

### Projeto E Runtime

- Solucao: `ControleDeWebServices.sln`.
- Projeto principal: `ControleDeWebServices/ControleDeWebServices.csproj`.
- Tipo atual: WPF classico.
- Runtime atual: `.NET Framework 4.8`.
- Formato atual: `.csproj` classico com `packages.config`, `HintPath`, imports legados e `App.config`.
- Saida Debug atual: `ControleDeWebServices/Execucao`.
- Icone atual do app: `ControleDeWebServices/agt_web.ico`.

### Telas E Code-Behind

- Shell:
  - `MainWindow.xaml`
  - `MainWindow.xaml.cs`
- Operacao:
  - `View/Webservices.xaml`
  - `View/WebServices.xaml.cs`
- Cadastros e listas:
  - `ListaDeClientes`, `Clientes`
  - `ListaDeSistemas`, `Sistema`
  - `ListaDeServicos`, `Servico`
  - `ListaDeSecoes`, `Secoes`
  - `CadastroDeInformacoes`
  - `CadastroDeInformacoesServico`
  - `ImportarParametrosDe`
- Vinculos:
  - `ListaVinculosSistema`
  - `VinculoClienteSistema`
  - `ListaVinculoServico`
  - `VinculoClienteServico`

### Modelos

- `ClasseBaseConexao`
- `ClasseBaseVinculo`
- `Cliente`
- `ClienteSistemas`
- `ClienteServicos`
- `ParametrosSistema`
- `Secao`
- `Servicos`
- `Sistemas`

### Configuracoes, Resources E Assets

- `App.config`
- `Configuracoes.config`
- `Configuracoes .config`
- `SeloDigital.GO.Servico.dll.config`
- `Properties/Resources.resx`
- `Properties/Settings.settings`
- `Imagens/agt_web.ico`
- SVGs antigos em `Imagens`
- Prototipo antigo em `Pencil`
- Mockups conceituais em `docs/Telas Conceituais`
- Novo pacote de icones em `docs/Icones`

### Execucao

A pasta `ControleDeWebServices/Execucao` era usada como saida Debug e continha binarios rastreados pelo Git. No fechamento da Sprint 0, ela foi removida do indice e passou a ser ignorada por `.gitignore`.

Itens encontrados nesse diretorio antes da remocao do controle de versao:

- executavel e config gerados: `ControleDeWebServices.exe`, `ControleDeWebServices.exe.config`;
- DLL proprietaria: `Comum.Utilitarios.dll`;
- UI: `MaterialDesignThemes.Wpf.dll`, `MaterialDesignColors.dll`, `FluentWPF.dll`;
- dados: `EntityFramework.dll`, `EntityFramework.SqlServer.dll`, `FirebirdSql.Data.FirebirdClient.dll`, `Npgsql.dll`;
- infraestrutura auxiliar: `Ninject.dll`, `PeanutButter.INI.dll`, `Microsoft.Xaml.Behaviors.dll`;
- dependencias `System.*` e `Microsoft.*` copiadas para execucao.

Como esses arquivos nao estao mais versionados, o build automatico passa a ser possivel sem risco de sobrescrever artefatos rastreados. Ainda assim, qualquer diferenca de build deve ser revisada antes da Sprint 1.

## Componentes E Decisao Inicial

| Componente | Uso observado | Decisao inicial |
| --- | --- | --- |
| Entity Framework 6.5.1 | `DadosDBContext`, migrations e acesso principal ao banco | Atualizar se compativel; substituir por EF Core somente se bloquear .NET 8/9 |
| MaterialDesignThemes / MaterialDesignColors | Estilos, `PackIcon`, botoes e controles WPF | Atualizar para versao WPF moderna |
| FluentWPF | `AcrylicWindow` no shell | Substituir/remover se bloquear migracao ou conflitar com visual limpo |
| FirebirdSql.Data.FirebirdClient | Atualizacao externa Firebird em `AtualizarPadroes` | Atualizar provider |
| Npgsql | Atualizacao externa PostgreSQL | Atualizar provider |
| SQL Server provider | Banco principal e comandos externos | Avaliar troca para `Microsoft.Data.SqlClient` |
| PeanutButter.INI | Leitura/escrita de INI | Atualizar ou substituir por parser compativel |
| Ninject | Referencia no projeto, uso efetivo nao evidente | Remover se sem uso; preferir `Microsoft.Extensions.DependencyInjection` |
| Microsoft.Xaml.Behaviors | Referencia WPF | Atualizar se houver uso real; remover se o inventario confirmar inutilidade |
| System.Windows.Forms | Usado para `MessageBox` em alguns pontos | Remover como dependencia visual; substituir por toast/dialog WPF |
| `Comum.Utilitarios.dll` | Criptografia e auxiliares proprietarios | Isolar atras de interfaces e validar compatibilidade |

## Mapa Dos Fluxos Reais

### Shell

`MainWindow` cria `DadosDBContext`, registra Enter para avancar foco em `TextBox`, `Button` e `ComboBox`, trata excecoes globais com `MessageBox` e carrega `WebServices` ao iniciar. A navegacao atual cria paginas diretamente e troca `FramePrincipal.Content`.

### Listas E Cadastros

Clientes, Sistemas, Servicos e Secoes seguem um fluxo repetido: lista carrega dados, `Incluir` abre um formulario no frame embutido, `Editar` usa a linha selecionada, `Excluir` confirma por `MessageBox` e a tela recarrega a consulta apos salvar/cancelar.

### Vinculos

Os vinculos usam selecao progressiva por UF, Cliente e Sistema, com listas de disponiveis/vinculados. O fluxo precisa manter alteracoes pendentes visiveis e tratar selecao nula antes de adicionar, remover ou salvar.

### Configuracoes

`CadastroDeInformacoes` e `CadastroDeInformacoesServico` tratam executavel, arquivo de configuracao, servidor, porta, banco, tipo de conexao, usuario, senha, secao, criptografia e parametros.

### Importacao

`ImportarParametrosDe` seleciona UF, Cliente e Sistema de origem, copia parametros para o sistema atual e hoje mostra a mensagem incorreta "URL atualizada com Sucesso!". A copy correta futura e "Parametros importados com sucesso".

### WebServices

`WebServices` carrega os 10 mais acessados, filtra por codigo/UF/cliente, atualiza URL e executa por duplo clique. A execucao chama uma cadeia operacional: validar configuracoes, atualizar arquivos INI/XML, atualizar parametros, derrubar processos, iniciar servicos e registrar acesso.

## Riscos Tecnicos Conhecidos

- `MessageBox` aparece em validacoes, exclusoes, erros globais, importacao e atualizacao de URL.
- `DadosDBContext` e criado diretamente em views e classes de operacao.
- `DadosDBContext` le `Config.ini`, monta string de conexao, inicializa migrations e exibe erro visual no construtor.
- `AtualizarPadroes` monta SQL por concatenacao e abre/fecha conexoes dentro de loops.
- `ExecutarSistemaServico` mistura INI, XML, criptografia, processos, banco e contagem de acessos.
- `DerrubarServicos` encerra processos diretamente por prefixo de nome.
- `WebServices` executa operacao sensivel por duplo clique, sem progresso visivel por etapa.
- `VinculoClienteSistema` possui bug provavel em lista de alteracoes, com `Contains` aparentemente invertido.
- Existem arquivos duplicados/legados, como `Configuracoes.config` e `Configuracoes .config`.
- A pasta `Execucao` versionava binarios que podiam ser sobrescritos pelo build; no fechamento da Sprint 0, a pasta foi removida do indice e ignorada.

## Protocolo De Build Atual

O projeto atual e WPF em `.NET Framework 4.8` com `.csproj` classico. O build recomendado para baseline e Visual Studio/MSBuild compativel com .NET Framework 4.8.

No inicio da Sprint 0, o build automatico nao foi executado porque:

- a saida Debug aponta para `ControleDeWebServices/Execucao`;
- muitos arquivos em `Execucao` ainda eram rastreados pelo Git;
- executar o build poderia sobrescrever DLLs, `.exe`, `.pdb` e `.config` versionados.

Depois do commit `c2a31b7 remover pasta execução`, `ControleDeWebServices/Execucao/` passou a ser ignorada e `git ls-files ControleDeWebServices/Execucao` retorna zero arquivos. Assim, o build automatico fica liberado para a Sprint 1, desde que as alteracoes continuem revisadas pelo Git.

Validacao manual recomendada:

1. Abrir `ControleDeWebServices.sln` no Visual Studio com suporte a .NET Framework 4.8.
2. Restaurar pacotes NuGet se necessario.
3. Compilar em Debug.
4. Conferir que `Execucao` permanece ignorada pelo Git.
5. Registrar qualquer erro de build antes da Sprint 1.

## Checklist De Paridade Funcional

- Aplicacao abre sem erro inicial.
- Tela `WebServices` carrega ao iniciar.
- Menu lateral navega para cadastros, vinculos e operacao.
- Enter continua avancando foco.
- Clientes, Sistemas, Servicos e Secoes consultam, incluem, editam, salvam, cancelam e excluem.
- Vinculos selecionam UF/Cliente/Sistema, movem itens entre listas e salvam alteracoes.
- Configuracoes gravam arquivos, conexao, credenciais, secao, criptografia e parametros.
- Importacao copia parametros de outro sistema vinculado.
- WebServices lista 10 mais acessados, busca por codigo/UF/cliente, atualiza URL e executa o fluxo operacional.

## Criterios De Aceite Da Sprint 0

- Baseline documentado antes de qualquer migracao real.
- Branch `codex/sprint-0-baseline` criada.
- Componentes criticos classificados inicialmente.
- Riscos tecnicos conhecidos registrados.
- Protocolo de build atual documentado com decisao final sobre `Execucao` ignorada.
- Regra de nao alterar modelos documentada.
- Proxima sprint pode iniciar pela prova de migracao para .NET 8/9 WPF.
