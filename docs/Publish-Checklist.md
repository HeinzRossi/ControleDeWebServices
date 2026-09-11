# Checklist De Publicacao: ControleDeWebServices

## Objetivo

Preparar uma validacao repetivel para publicar ou testar o `ControleDeWebServices` em maquina limpa apos a modernizacao para WPF em .NET 8.

## Pre-Build

- Confirmar branch aprovada para publicacao.
- Confirmar `git status --short` limpo.
- Rodar `dotnet restore ControleDeWebServices.sln`.
- Rodar `dotnet build ControleDeWebServices.sln --configuration Release`.
- Rodar `dotnet test ControleDeWebServices.sln --no-build`.
- Confirmar que `ControleDeWebServices/Modelo` nao possui alteracoes pendentes.
- Confirmar que `ControleDeWebServices/Execucao` nao possui arquivos rastreados.

## Arquivos Obrigatorios

- Executavel e DLLs gerados para `net8.0-windows`.
- `ControleDeWebServices.dll.config` ou configuracao equivalente gerada pelo build, quando aplicavel.
- `Config.ini` valido no diretorio esperado de execucao, entregue fora do repositorio e sem commit de credenciais.
- DLL auxiliar `Comum.Utilitarios.dll`, se ainda for dependencia no build publicado.
- Arquivos externos usados pelos vinculos de teste: executaveis, XML/INI e configuracoes de servicos.
- Pasta `Logs/` com permissao de escrita ou permissao para ser criada na primeira inicializacao.

## Ambiente Da Maquina Limpa

- Windows com suporte a aplicacoes desktop WPF.
- .NET Desktop Runtime compativel com o target publicado.
- Acesso ao banco principal do sistema.
- Acesso aos bancos externos usados pelos testes: SQL Server, Firebird e PostgreSQL, conforme configuracao real.
- Permissao para iniciar executaveis vinculados.
- Permissao para encerrar processos esperados pelo fluxo operacional.

## Validacao De Inicializacao

- Abrir a aplicacao.
- Confirmar que nenhum dialogo aparece no startup.
- Confirmar tela inicial em WebServices.
- Confirmar criacao de `Logs/ControleDeWebServices-YYYYMMDD.log`.
- Confirmar que o log possui entrada de startup.
- Confirmar que o log nao exibe senha, connection string completa ou dado sensivel.

## Validacao Funcional Minima

- Navegar por todas as telas do menu.
- Executar uma consulta de WebServices.
- Validar cadastros simples: listar, incluir teste, editar teste, cancelar e excluir teste.
- Validar vinculo Cliente/Sistema e Cliente/Servico com dados controlados.
- Abrir configuracao tecnica de um vinculo.
- Executar importacao de parametros em ambiente seguro.
- Atualizar URL de um item de teste.
- Executar WebService de teste por botao explicito.

## Feedback E UX

- Confirmar toasts de sucesso, aviso e erro.
- Confirmar dialogos customizados em exclusoes/remocoes.
- Confirmar ausencia de `MessageBox`.
- Confirmar foco por teclado e Enter avancando campos.
- Confirmar que telas continuam legiveis em resolucoes usuais.

## Aceite

- Build Release e testes automatizados passam.
- Aplicacao inicia em maquina limpa.
- Fluxos criticos passam com dados reais ou ficam registrados com falha reproduzivel.
- Logs existem, sao uteis e nao vazam dados sensiveis.
- Nenhum artefato local de execucao deve ser commitado.

## Validacao Sprint 13

- Publish local validado em `artifacts/publish/ControleDeWebServices`.
- App publicado iniciou por curto periodo e criou log local.
- Artefatos principais conferidos: executavel, DLLs, configs, `Comum.Utilitarios.dll` e SVGs em `Imagens`.
- `artifacts/` confirmado como ignorado pelo Git.
- Instalador final ainda nao foi escolhido.

## Fluxo Oficial De Pacote Portavel

- Gerar publish Release em `artifacts/publish/ControleDeWebServices`.
- Compactar o conteudo publicado em `artifacts/packages/ControleDeWebServices-net8-windows.zip`.
- Nao incluir `Config.ini` real, logs ou credenciais no ZIP.
- Registrar SHA256 do pacote antes da entrega.
- Validar extraindo o ZIP em `artifacts/verify/ControleDeWebServices` e iniciando o app por curto periodo.
- Usar `docs/Config.ini.template.md` como referencia segura para montar o `Config.ini` no ambiente alvo.

## Validacao Sprint 14

- ZIP portavel validado em `artifacts/packages/ControleDeWebServices-net8-windows.zip`.
- SHA256 registrado em `docs/Sprint-14-Pacote-Zip.md`.
- App extraido iniciou por curto periodo e criou log local.
- `Config.ini` e `Logs/` ficaram fora do ZIP original.
- Instalador final continua pendente de decisao futura.
