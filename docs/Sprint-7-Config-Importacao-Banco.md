# Sprint 7: Configuracao, Importacao E Banco

## Objetivo

Migrar configuracao tecnica, importacao de parametros e atualizacao de padroes externos para a arquitetura criada nas sprints anteriores: ViewModels, servicos de aplicacao, infraestrutura isolada, feedback por toast/dialogo customizado e SQL parametrizado.

## Estado

- Branch: `codex/sprint-7-config-importacao-banco`.
- Base: `codex/sprint-6-cadastros-vinculos`.
- Target mantido: `net8.0-windows`.
- Escopo aplicado: configuracao de sistemas, configuracao de servicos, importacao de parametros e `AtualizarPadroes`.
- Regra transversal mantida: nao alterar `ControleDeWebServices/Modelo`.

## Entregas

| Area | Resultado |
| --- | --- |
| Configuracao de sistema | Criados DTOs, contrato, service e ViewModel para arquivos, conexao, credenciais, secao, criptografia e parametros. |
| Configuracao de servico | Criados DTO, contrato, service e ViewModel para arquivos, conexao e credenciais. |
| Importacao | Criado caso de uso para escolher UF, cliente e sistema de origem, com copia por upsert de parametros. |
| Banco externo | `AtualizarPadroes` passou a usar comandos parametrizados, builders de conexao e descarte por `using`. |
| Feedback | Fluxos migrados usam toast/dialog customizado; `MessageBox` saiu das telas migradas e de `AtualizarPadroes`. |
| Arquivos | Selecao de arquivos passou para `IFilePickerService`, com implementacao WPF em Presentation. |

## Decisoes

- Configuracoes foram acopladas ao fluxo de vinculos por editor embutido, mantendo contexto do cliente/sistema/servico selecionado.
- Importacao de parametros faz upsert por `Secao` + `Parametro`: insere quando nao existe e atualiza `Valor` quando ja existe.
- `AtualizarPadroes` manteve compatibilidade com a interface legada, mas tambem implementa contrato novo de Application.
- SQL Server/MSSQL, Firebird e PostgreSQL usam comandos separados por provider, com parametros para valores vindos do usuario.
- `OpenFileDialog` foi mantido como recurso WPF de Presentation, isolado por `IFilePickerService`.

## Validacao

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros.
- Aviso restante esperado: `CS8981` em `Auxiliar/LerArquivoDeConfiguracao.cs`, fora do escopo da Sprint 7.
- `rg` nas telas migradas e em `AtualizarPadroes` para `MessageBox`, `new DadosDBContext`, `FrameActive`, `Click=`, `SelectionChanged=` e `OpenFileDialog`: sem ocorrencias no escopo migrado.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/`: continua ignorada e sem arquivos rastreados.

## Pendencias Para Sprint 8

- Migrar `WebServices.xaml.cs` para ViewModel/caso de uso.
- Isolar `ExecutarSistemaServico`, arquivos INI/XML, criptografia operacional, processos e logs.
- Exibir progresso por etapa ao executar WebServices.
- Substituir chamadas diretas restantes de operacao por servicos injetados.
- Revisar warnings e dependencias visuais remanescentes apos migracao operacional.
