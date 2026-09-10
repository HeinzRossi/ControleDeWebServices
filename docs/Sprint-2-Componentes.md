# Sprint 2: Atualizacao De Componentes

## Objetivo

Atualizar, remover ou substituir componentes apos a migracao para WPF em .NET 8, mantendo o menor conjunto de mudancas necessario para build limpo. Esta sprint nao altera modelos, regras de negocio, arquitetura profunda ou desenho visual final.

## Estado

- Branch: `codex/sprint-2-componentes`.
- Base: `codex/sprint-1-dotnet-wpf`.
- Target mantido: `net8.0-windows`.
- Resultado final: `dotnet build ControleDeWebServices.sln --no-restore` com 0 erros e 25 avisos herdados do codigo atual.
- `NU1701` do MaterialDesignThemes removido.
- Avisos restantes: principalmente `CA1416` em `MessageBox`/dialogs WinForms e `CS8981` em tipo legado; ficam registrados para tratamento nas sprints seguintes, sem alterar comportamento nesta sprint.

## Matriz De Componentes

| Componente | Decisao | Resultado |
| --- | --- | --- |
| Entity Framework | Atualizado | `6.5.1` para `6.5.2` |
| FirebirdSql.Data.FirebirdClient | Atualizado | `10.3.3` para `10.3.4` |
| MaterialDesignThemes | Atualizado | `2.6.0` para `5.3.2` |
| MaterialDesignColors | Atualizado | `1.2.7` para `5.3.2` |
| Npgsql | Atualizado | `7.0.10` para `10.0.3` |
| SQL Server client | Substituido | `System.Data.SqlClient` direto removido; `Microsoft.Data.SqlClient` `7.0.2` usado no codigo externo |
| FluentWPF | Removido | PackageReference, resources e atributos `fw:*` removidos |
| Ninject | Removido | PackageReference e binding redirect removidos |
| Microsoft.Xaml.Behaviors.Wpf | Removido como dependencia direta | Permanece apenas se vier transitivo de outro pacote |
| Microsoft.Extensions.DependencyInjection.Abstractions | Removido como dependencia direta | DI real fica para Sprint 3 |
| Microsoft.Extensions.Logging.Abstractions | Removido como dependencia direta | Logging real fica para sprint futura |
| System.Reflection.Emit | Removido | Sem uso direto encontrado |
| PeanutButter.INI | Mantido temporariamente | Continua em `3.0.384`; substituicao fica para sprint futura se necessario |
| Comum.Utilitarios.dll | Mantido temporariamente | Continua como referencia local isolada |
| Windows Forms | Mantido temporariamente | `UseWindowsForms=true` continua por causa de `MessageBox` legado |

## Alteracoes Realizadas

- Projeto manteve SDK-style e `net8.0-windows`.
- Pacotes principais atualizados em `ControleDeWebServices.csproj`.
- `AtualizarPadroes` passou a usar `Microsoft.Data.SqlClient`.
- `FluentWPF` foi removido do `App.xaml`, `MainWindow.xaml` e `ImportarParametrosDe.xaml`.
- Colunas antigas `materialDesign:MaterialDataGridTextColumn`, incompatíveis com MaterialDesignThemes 5, foram trocadas por `DataGridTextColumn` nativo.
- Binding redirects mortos de `Ninject` e `Microsoft.Extensions.Logging.Abstractions` foram removidos do `App.config`.

## Decisoes Temporarias

- `UseWindowsForms=true` permanece apenas para compilar `MessageBox` e dialogs legados; remocao fica para Sprint 4.
- `PeanutButter.INI` permanece ate a infraestrutura de configuracao ser isolada.
- `Comum.Utilitarios.dll` permanece ate criptografia/auxiliares ficarem atras de interfaces.
- `System.Data.SqlClient` ainda pode aparecer como dependencia transitiva por Entity Framework ou Microsoft.Data.SqlClient, mas nao fica mais como dependencia direta nem namespace usado em `AtualizarPadroes`.

## Validacao

- `dotnet restore ControleDeWebServices.sln`: executado com sucesso.
- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros e 25 avisos herdados do codigo atual.
- `dotnet list ControleDeWebServices/ControleDeWebServices.csproj package --include-transitive`: executado para confirmar dependencias diretas e transitivas.
- `git diff -- ControleDeWebServices/Modelo`: sem alteracoes.
- `ControleDeWebServices/Execucao/` continua ignorada por `.gitignore`.

## Pendencias Para Sprint 3

- Criar estrutura de camadas: Presentation, ViewModels, Application, Domain e Infrastructure.
- Introduzir DI real com `Microsoft.Extensions.DependencyInjection`.
- Criar contratos base para navegacao, feedback e contexto.
- Manter modelos intactos e usar ViewModels/servicos ao redor deles.
