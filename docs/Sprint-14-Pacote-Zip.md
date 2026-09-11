# Sprint 14: Pacote ZIP Portavel Para Validacao

## Objetivo

Gerar um pacote `.zip` portavel a partir da publicacao Release framework-dependent do `ControleDeWebServices`, sem instalador e sem incluir `Config.ini` real, logs ou credenciais.

## Resultado

- Branch de execucao: `codex/sprint-14-pacote-zip`.
- Publish Release gerado em `artifacts/publish/ControleDeWebServices`.
- ZIP gerado em `artifacts/packages/ControleDeWebServices-net8-windows.zip`.
- ZIP extraido e validado em `artifacts/verify/ControleDeWebServices`.
- App extraido iniciou por curto periodo e criou log local.
- Template seguro criado em `docs/Config.ini.template.md`.
- `artifacts/` permanece ignorado pelo Git.

## Pacote

- Arquivo: `artifacts/packages/ControleDeWebServices-net8-windows.zip`.
- Tamanho: `11866308` bytes.
- SHA256: `8D7DBA62E4472591ADD8034DDE8431615DDA9760701309DB81AFFB3BC4771D1F`.
- Tipo: framework-dependent.
- Runtime esperado: .NET Desktop Runtime compativel com `net8.0-windows`.

## Validacao Executada

- `dotnet restore ControleDeWebServices.sln`: executado com sucesso.
- `dotnet build ControleDeWebServices.sln --configuration Release --no-restore`: executado com sucesso, 0 erros e 0 avisos.
- `dotnet test ControleDeWebServices.sln --no-build`: executado com sucesso, 12 testes aprovados.
- `dotnet publish ControleDeWebServices/ControleDeWebServices.csproj --configuration Release --output artifacts/publish/ControleDeWebServices`: executado com sucesso.
- `Compress-Archive`: executado com sucesso.
- `Expand-Archive`: executado com sucesso.
- Startup pela pasta extraida: processo iniciou, permaneceu ativo por 5 segundos e foi encerrado manualmente.
- Log criado em `artifacts/verify/ControleDeWebServices/Logs/ControleDeWebServices-20260911.log`.

## Artefatos Conferidos No ZIP Extraido

- `ControleDeWebServices.exe`.
- `ControleDeWebServices.dll`.
- `ControleDeWebServices.runtimeconfig.json`.
- `ControleDeWebServices.deps.json`.
- `ControleDeWebServices.dll.config`.
- `Configuracoes.config`.
- `Configuracoes .config`.
- `SeloDigital.GO.Servico.dll.config`.
- `Comum.Utilitarios.dll`.
- Pasta `Imagens` com SVGs.
- Dependencias NuGet copiadas para a pasta publicada.

## Controles De Seguranca

- `Config.ini` nao foi incluido no ZIP.
- `Logs/` nao foi incluido no ZIP original; a pasta nasceu apenas apos iniciar a copia extraida.
- `artifacts/` foi confirmado como ignorado pelo Git.
- Nenhuma alteracao foi feita em `ControleDeWebServices/Modelo`.

## Uso Do Pacote

1. Extrair `ControleDeWebServices-net8-windows.zip` em uma pasta local da maquina alvo.
2. Instalar .NET Desktop Runtime compativel com `net8.0-windows`, se ainda nao existir.
3. Criar `Config.ini` ao lado de `ControleDeWebServices.exe`, usando `docs/Config.ini.template.md` como referencia.
4. Executar `ControleDeWebServices.exe`.
5. Confirmar criacao de `Logs/ControleDeWebServices-YYYYMMDD.log`.

## Pendencias

- Validar o ZIP em maquina limpa real.
- Executar checklist funcional com banco principal e bancos externos reais.
- Decidir se o ZIP sera suficiente para operacao interna ou se uma sprint futura deve criar instalador.
