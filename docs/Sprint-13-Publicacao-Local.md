# Sprint 13: Publicacao Controlada E Validacao Em Pasta Limpa

## Objetivo

Validar uma publicacao local em Release para o `ControleDeWebServices`, sem criar instalador final e sem alterar modelos, regra de negocio ou layout visual.

## Resultado

- Branch de execucao: `codex/sprint-13-publicacao-local`.
- Publicacao local gerada em `artifacts/publish/ControleDeWebServices`.
- A pasta `artifacts/` ja estava ignorada pelo `.gitignore` e nao deve ser versionada.
- Nenhuma alteracao de `.csproj` foi necessaria para incluir os artefatos principais.
- Aplicacao publicada iniciou a partir da propria pasta de publish e criou log local.
- Instalador MSI, ClickOnce ou MSIX continua fora desta sprint.

## Validacao Executada

- `dotnet restore ControleDeWebServices.sln`: executado com sucesso.
- `dotnet build ControleDeWebServices.sln --configuration Release --no-restore`: executado com sucesso, 0 erros e 0 avisos.
- `dotnet test ControleDeWebServices.sln --no-build`: executado com sucesso, 12 testes aprovados.
- `dotnet publish ControleDeWebServices/ControleDeWebServices.csproj --configuration Release --output artifacts/publish/ControleDeWebServices`: executado com sucesso.
- Startup pela pasta publicada: processo iniciou, permaneceu ativo por 5 segundos e foi encerrado manualmente.
- Log criado em `artifacts/publish/ControleDeWebServices/Logs/ControleDeWebServices-20260911.log`.
- `git check-ignore` confirmou que executavel publicado e log em `artifacts/` ficam ignorados.

## Artefatos Conferidos

- `ControleDeWebServices.exe`.
- `ControleDeWebServices.dll`.
- `ControleDeWebServices.runtimeconfig.json`.
- `ControleDeWebServices.deps.json`.
- `ControleDeWebServices.dll.config`.
- `Configuracoes.config`.
- `Configuracoes .config`.
- `SeloDigital.GO.Servico.dll.config`.
- `Comum.Utilitarios.dll`.
- Pasta `Imagens` com SVGs usados pelo shell.
- Dependencias NuGet copiadas para a pasta publicada.

## Observacoes

- `Config.ini` real nao foi incluido nem versionado, conforme decisao de seguranca.
- A publicacao atual e framework-dependent; a maquina alvo precisa de .NET Desktop Runtime compativel com `net8.0-windows`.
- A saida publicada inclui dependencias transientes como `Microsoft.Xaml.Behaviors.dll` e `System.Data.SqlClient.dll`; isso foi observado como resultado do grafo NuGet atual, nao como falha de publish.
- QA com banco real, executaveis reais e arquivos externos permanece dependente do ambiente operacional.

## Pendencias Para Publicacao Final

- Validar a pasta publicada em maquina limpa com runtime instalado.
- Fornecer `Config.ini` seguro fora do repositorio.
- Rodar checklist funcional com banco principal e bancos externos reais.
- Decidir formato final de distribuicao: pasta compactada, instalador interno, ClickOnce ou MSIX.
- Validar permissao de escrita em `Logs/` no diretorio final escolhido.
