# Sprint 1: Migracao Para .NET 8 WPF

## Objetivo

Converter o projeto principal para formato SDK-style com WPF em .NET moderno, mantendo o menor conjunto de mudancas necessario para restaurar e compilar. Esta sprint nao altera modelos, telas, regras de negocio, fluxos de UX ou arquitetura interna.

## Estado

- Branch: `codex/sprint-1-dotnet-wpf`.
- Base: `codex/sprint-0-baseline`.
- Target inicial: `net8.0-windows`.
- Resultado do restore: concluido com aviso de compatibilidade em `MaterialDesignThemes 2.6.0`.
- Resultado do build inicial: concluido com 0 erros e avisos de compatibilidade/analisadores.
- Resultado do build final incremental: concluido com 0 erros e 1 aviso ativo.

## Alteracoes Realizadas

- `ControleDeWebServices.csproj` convertido para SDK-style com:
  - `Sdk="Microsoft.NET.Sdk"`;
  - `OutputType=WinExe`;
  - `TargetFramework=net8.0-windows`;
  - `UseWPF=true`;
  - `UseWindowsForms=true` temporario para preservar compilacao do codigo legado que ainda usa `System.Windows.Forms`;
  - `GenerateAssemblyInfo=false` para preservar `Properties/AssemblyInfo.cs`;
  - `Nullable=disable` e `ImplicitUsings=disable` para reduzir mudancas de comportamento.
- `packages.config` removido.
- Dependencias migradas para `PackageReference`.
- `Comum.Utilitarios.dll` mantida como referencia local temporaria.
- Configuracoes e assets auxiliares mantidos como conteudo para saida quando necessario.

## Decisoes Temporarias

- `UseWindowsForms=true` permanece apenas como ponte de compatibilidade. A remocao depende da Sprint 4, quando `MessageBox` for substituido por toast/dialog customizado.
- `MaterialDesignThemes 2.6.0` foi mantido para migração mínima, mas gera `NU1701` por ser restaurado via assets de .NET Framework. A atualização fica para Sprint 2.
- `App.config` foi preservado para nao misturar migracao de runtime com mudanca de configuracao.
- Nenhum modelo em `ControleDeWebServices/Modelo` foi alterado.

## Avisos Relevantes Do Build

- `NU1701`: `MaterialDesignThemes 2.6.0` foi restaurado usando frameworks antigos em vez de `net8.0-windows`. Este foi o aviso ativo no build final.
- `CA1416`: uso de APIs Windows Forms, incluindo `MessageBox` e `OpenFileDialog`, apareceu no build inicial e exige tratamento na modernizacao.
- `CS8981`: tipo `configuration` em lowercase apareceu no build inicial e pode conflitar com futuras palavras reservadas do C#.

## Validacao

- `dotnet restore ControleDeWebServices.sln`: executado com sucesso.
- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/` continua ignorada por `.gitignore`.

## Pendencias Para Sprint 2

- Atualizar `MaterialDesignThemes` e `MaterialDesignColors` para versoes compativeis com WPF moderno.
- Reavaliar `FluentWPF`; remover/substituir se continuar dependente de API antiga.
- Avaliar troca de `System.Data.SqlClient` para `Microsoft.Data.SqlClient`.
- Atualizar Firebird, Npgsql, PeanutButter.INI e demais pacotes.
- Remover Ninject se continuar sem uso real.
- Isolar ou substituir `Comum.Utilitarios.dll` se houver incompatibilidade em runtime.
- Planejar remocao de `UseWindowsForms=true` apos troca de `MessageBox`.

## Criterios De Aceite

- Projeto restaura com `PackageReference`.
- Projeto compila em `net8.0-windows`.
- `packages.config` nao e mais usado.
- Modelos permanecem intactos.
- Mudancas ficam restritas a migracao estrutural do projeto.
- Avisos reais ficam documentados para Sprint 2.
