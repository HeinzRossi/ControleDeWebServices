# Sprint 9: Design System WPF E Polimento Visual

## Objetivo

Aplicar o design system visual do ControleDeWebServices de forma centralizada, reduzindo estilos duplicados em XAML e consolidando o visual operacional limpo aprovado para Shell, telas migradas, tabelas, formulários, toasts e diálogos.

## Resultado

- Branch de execução: `codex/sprint-9-design-system-visual`.
- Design system WPF criado em ResourceDictionaries próprios.
- `App.xaml` passou a carregar tokens e componentes globais junto do MaterialDesign.
- Telas migradas passaram a consumir chaves `App.*` para botões, tabelas, superfícies, painéis, títulos, toasts e diálogo customizado.
- `MessageBox` remanescente em `DadosDBContext` foi removido da experiência visual.
- `UseWindowsForms=true` foi removido do projeto após validação de ausência de uso visual WinForms.
- Modelos em `ControleDeWebServices/Modelo` não foram alterados.

## Decisões De Design

- Manter WPF + MaterialDesignThemes como base de controles, mas com identidade visual própria por cima.
- Usar paleta operacional limpa: azul para ação primária, verde para sucesso/criação, vermelho para risco, âmbar para atenção e neutros frios para estrutura.
- Concentrar chaves públicas em `App.*`, como `App.PrimaryButton`, `App.SuccessButton`, `App.DangerButton`, `App.DataGrid`, `App.GridHeader`, `App.PageSurface`, `App.FilterPanel`, `App.Toast` e `App.ConfirmDialog`.
- Usar `App.DataGrid` como padrão de densidade, linhas alternadas, bordas sutis e cabeçalho forte.
- Manter toasts e diálogo customizado como componentes visuais oficiais, sem `MessageBox`.
- Corrigir copy visual em pontos de navegação e telas migradas, incluindo “Serviços”, “Seções”, “Ações”, “Atenção”, “manutenção” e “Vínculo”.

## Limites Da Sprint

- Não houve alteração em modelos, entidades, migrations ou estrutura de banco.
- Não houve mudança de regra de negócio.
- Não houve troca de biblioteca visual além do uso centralizado de estilos WPF próprios.
- Validação manual completa de UI deve ser feita no ambiente do usuário com banco/configuração real.

## Validação

- `dotnet build ControleDeWebServices.sln --no-restore`: executado com sucesso, 0 erros e 0 avisos.
- `rg -n "MessageBox|System.Windows.Forms|UseWindowsForms" ControleDeWebServices -g "*.cs" -g "*.xaml" -g "*.csproj"`: sem ocorrências esperadas.
- `rg -n "PrimaryActionButton|SuccessActionButton|DangerActionButton|GridHeaderStyle" ControleDeWebServices/View -g "*.xaml"`: sem ocorrências.
- `git diff -- ControleDeWebServices/Modelo`: sem alterações.
- `git ls-files ControleDeWebServices/Execucao`: sem retorno.

## Próximos Passos

- Fazer QA visual manual com dados reais e janelas redimensionadas.
- Avaliar ajustes finos de foco por teclado, contraste e estado disabled.
- Considerar Sprint 10 para testes automatizados, logs operacionais e hardening de execução em ambiente real.
