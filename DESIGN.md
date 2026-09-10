---
name: ControleDeWebServices
description: Sistema WPF administrativo para cadastro, importacao, vinculo e manutencao de web services.
colors:
  primary: "#2F6F8F"
  primary-strong: "#245875"
  surface: "#F7F9FA"
  surface-raised: "#FCFDFD"
  surface-muted: "#EEF3F5"
  text-primary: "#172126"
  text-secondary: "#516168"
  border-subtle: "#D7E0E4"
  success: "#4E9A57"
  danger: "#B94E45"
  warning: "#B9822B"
typography:
  title:
    fontFamily: "Segoe UI, system-ui, sans-serif"
    fontSize: "20px"
    fontWeight: 600
    lineHeight: 1.25
  body:
    fontFamily: "Segoe UI, system-ui, sans-serif"
    fontSize: "13px"
    fontWeight: 400
    lineHeight: 1.4
  label:
    fontFamily: "Segoe UI, system-ui, sans-serif"
    fontSize: "12px"
    fontWeight: 500
    lineHeight: 1.25
rounded:
  sm: "4px"
  md: "6px"
  lg: "8px"
spacing:
  xs: "4px"
  sm: "8px"
  md: "12px"
  lg: "16px"
  xl: "24px"
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.surface-raised}"
    rounded: "{rounded.md}"
    padding: "8px 14px"
  button-danger:
    backgroundColor: "{colors.danger}"
    textColor: "{colors.surface-raised}"
    rounded: "{rounded.md}"
    padding: "8px 14px"
  button-success:
    backgroundColor: "{colors.success}"
    textColor: "{colors.surface-raised}"
    rounded: "{rounded.md}"
    padding: "8px 14px"
---

# Design System: ControleDeWebServices

## 1. Overview

**Creative North Star: "Painel Operacional Limpo"**

ControleDeWebServices deve parecer uma ferramenta de trabalho confiavel, feita para usuarios que cadastram, importam, consultam e mantem configuracoes tecnicas com frequencia. O visual deve ser claro, denso e previsivel, com acabamento atual sem transformar a aplicacao em uma peca decorativa.

A experiencia deve favorecer leitura rapida de tabelas, reconhecimento imediato de acoes e baixa carga cognitiva. O usuario precisa entender onde esta, qual registro esta editando e quais acoes sao seguras, destrutivas ou primarias.

**Key Characteristics:**
- Interface clara, profissional e operacional.
- Densidade adequada para cadastros, consultas, importacoes e vinculos.
- Componentes consistentes antes de efeitos visuais.
- Estados de interacao visiveis para mouse e teclado.
- Linguagem objetiva, com termos iguais em telas semelhantes.

## 2. Colors

A paleta proposta e restrita: neutros frios para estrutura, azul operacional para acao primaria, verde para criacao/sucesso e vermelho para exclusao/erro.

### Primary
- **Azul Operacional:** cor de acoes primarias, cabecalhos de tabela, selecao ativa e foco de navegacao. Deve aparecer com moderacao para guiar a tarefa.
- **Azul Operacional Forte:** variacao para hover, pressed states e areas que precisam de contraste adicional.

### Secondary
- **Verde de Confirmacao:** usado para incluir, confirmar sucesso e indicar estados positivos.
- **Vermelho de Risco:** usado apenas em exclusao, erro e acoes irreversiveis.
- **Ambar de Atencao:** usado para alertas nao bloqueantes, pendencias e avisos.

### Neutral
- **Superficie Base:** fundo principal da aplicacao.
- **Superficie Elevada:** paineis, formularios e areas de conteudo.
- **Superficie Suave:** filtros, toolbars, linhas alternadas e regioes de apoio.
- **Texto Principal:** titulos, dados e conteudo de alta prioridade.
- **Texto Secundario:** labels, metadados, hints e informacoes auxiliares.
- **Borda Sutil:** divisores, contornos de campos e separadores de tabela.

### Named Rules

**The One Accent Rule.** Azul e para orientar acao e estado, nao para decorar a tela inteira.

**The Semantic Color Rule.** Verde, vermelho e ambar sempre carregam significado. Nao usar essas cores para diferenciar secoes sem motivo funcional.

## 3. Typography

**Display Font:** Segoe UI com fallback system-ui.
**Body Font:** Segoe UI com fallback system-ui.
**Label/Mono Font:** Nao definido nesta fase.

**Character:** A tipografia deve seguir o padrao nativo do Windows, com hierarquia curta e objetiva. O produto nao precisa de fonte expressiva; precisa de legibilidade e consistencia.

### Hierarchy
- **Title** (600, 20px, 1.25): titulo da janela, titulo de pagina e principais secoes.
- **Section** (600, 16px, 1.3): grupos de formulario, listas relacionadas e paines de vinculo.
- **Body** (400, 13px, 1.4): tabelas, campos, mensagens e conteudo padrao.
- **Label** (500, 12px, 1.25): rotulos compactos, filtros, cabecalhos auxiliares e tooltips.
- **Data** (400 ou 500, 13px, 1.35): conteudo de DataGrid, com peso maior apenas para identificadores importantes.

### Named Rules

**The Native Clarity Rule.** Usar uma unica familia tipografica para toda a UI. Variacao vem de peso, tamanho e espacamento, nao de muitas fontes.

## 4. Elevation

O sistema deve usar camadas tonais e bordas sutis como principal forma de profundidade. Sombras podem aparecer em menus, popups e elementos temporarios, mas nao devem transformar cada regiao em um card elevado.

### Shadow Vocabulary
- **Popup:** sombra leve para menus, combos e overlays temporarios.
- **Hover Subtle:** elevacao quase imperceptivel para botao ou linha interativa quando necessario.

### Named Rules

**The Flat By Default Rule.** Superficies ficam planas em repouso. Elevacao aparece para estado, foco ou conteudo temporario.

## 5. Components

### Buttons
- **Shape:** cantos discretos e consistentes (6px).
- **Primary:** azul operacional, texto claro, usado para salvar, atualizar e confirmar.
- **Danger:** vermelho de risco, usado apenas para excluir, remover ou descartar.
- **Success:** verde de confirmacao, usado para incluir ou concluir criacao.
- **Icon-only:** deve ter tooltip claro, alvo confortavel e iconografia consistente.
- **Hover / Focus:** hover com leve escurecimento; foco por teclado sempre visivel.

### Cards / Containers
- **Corner Style:** cantos discretos (6px ou 8px).
- **Background:** superficie elevada sobre fundo base.
- **Shadow Strategy:** plano por padrao, com borda sutil.
- **Internal Padding:** 12px a 16px para blocos densos; 24px para areas de maior leitura.

### Inputs / Fields
- **Style:** campos alinhados em grid, com label ou hint consistente.
- **Focus:** borda azul operacional ou indicador equivalente.
- **Error / Disabled:** erro em vermelho com mensagem objetiva; disabled com texto secundario e fundo suave.
- **Density:** altura minima confortavel, evitando campos de 26px quando houver risco de toque ou baixa legibilidade.

### Navigation
- **Style:** menu lateral com grupos claros, item ativo evidente e icones alinhados.
- **Typography:** texto curto, capitalizacao consistente e acentos corretos.
- **States:** default, hover, selected, focus e disabled quando aplicavel.
- **Behavior:** manter navegacao previsivel; expansao e recolhimento devem ser rapidos e funcionais.

### DataGrid
- **Header:** fundo primario ou superficie suave com texto de alto contraste, aplicado por estilo compartilhado.
- **Rows:** leitura limpa, padding consistente e selecao visivel.
- **Actions:** colunas de acao com botoes padronizados; evitar cada tela definir seus proprios estilos.
- **Empty State:** quando sem dados, mostrar mensagem util e acao possivel, nao uma area vazia.

### Forms
- **Layout:** grid responsivo por grupos, com alinhamento consistente.
- **Grouping:** dados basicos, conexao, arquivos, parametros e origem de importacao devem ter separacao visual clara.
- **Validation:** mensagens curtas, proximas do campo ou em area de feedback consistente.
- **Embedded Flow:** listas que abrem formulario dentro de um Frame devem preservar o contexto da consulta e oferecer retorno claro para a lista.

### Import / Auxiliary Windows
- **Purpose:** janelas auxiliares, como Importar Parametros De, devem ser compactas e orientadas a uma decisao especifica.
- **Layout:** usar cabecalho curto, campos em sequencia logica e botoes no rodape.
- **Flow:** origem, selecao, pre-visualizacao quando aplicavel, confirmacao e resultado.
- **Actions:** Importar deve ser acao primaria; Fechar ou Cancelar deve ser secundaria. Vermelho nao deve ser usado para importar.

### Connection Configuration
- **Supported Context:** telas de informacoes e configuracao devem acomodar diferentes tipos de conexao, incluindo Firebird e PostgreSQL/Npgsql.
- **Grouping:** separar servidor, porta, banco, tipo de conexao, usuario e senha em um grupo claro.
- **Feedback:** erros de conexao devem informar o problema e sugerir recuperacao, sem expor dados sensiveis.

### Toasts / Feedback
- **Purpose:** toasts sao o padrao visual para mensagens de sucesso, erro, aviso e informacao.
- **Position:** canto superior direito da area de conteudo, acima da tela atual e sem bloquear navegacao quando a mensagem nao exigir decisao.
- **Duration:** sucesso e informacao somem automaticamente; erro e aviso permanecem tempo maior e sempre podem ser fechados.
- **Stacking:** limitar a pilha para evitar excesso visual. Mensagens repetidas devem atualizar o toast existente.
- **Success:** verde de confirmacao, por exemplo "Parametros importados com sucesso".
- **Error:** vermelho de risco, por exemplo "Nao foi possivel atualizar a URL".
- **Warning:** ambar de atencao, por exemplo "Selecione um cliente antes de continuar".
- **Info:** azul operacional ou neutro, por exemplo "Configuracoes carregadas".

### Custom Confirmations
- **Purpose:** confirmacoes bloqueantes devem usar dialogo proprio do app, nunca MessageBox.
- **Layout:** overlay discreto, painel central compacto, titulo direto, texto curto e botoes alinhados a direita.
- **Focus:** foco preso dentro do dialogo, Escape aciona Cancelar e Enter aciona a opcao segura quando houver risco.
- **Destructive Action:** botao destrutivo em vermelho com texto especifico, como "Excluir"; botao secundario "Cancelar".
- **Copy:** explicar o impacto real: excluir registro, remover vinculos do cliente ou remover vinculos do sistema.

### Operations / Execution
- **Purpose:** telas operacionais, como WebServices, devem deixar claro quando uma acao executa servicos, atualiza arquivos ou incrementa acessos.
- **Interaction:** duplo clique pode ser preservado como atalho, mas a tela deve oferecer indicacao visual ou acao explicita equivalente.
- **Progress:** operacoes longas devem exibir toast/status de inicio, sucesso e falha.

## 6. Do's and Don'ts

### Do:
- **Do** centralizar cores, tipografia, espacamentos e estilos em ResourceDictionaries.
- **Do** usar MaterialDesignThemes e FluentWPF de forma consistente, sem misturar padroes de tela a tela.
- **Do** manter densidade operacional, especialmente em tabelas e formularios.
- **Do** padronizar os nomes das acoes: Incluir, Editar, Excluir, Salvar, Cancelar, Atualizar.
- **Do** tratar importacao como fluxo auxiliar guiado, com origem, destino e resultado claros.
- **Do** representar tipos de conexao como informacao operacional, sem acoplar estilo visual a um banco especifico.
- **Do** usar toast para sucesso, erro, aviso e informacao.
- **Do** usar dialogo customizado do app para confirmacoes destrutivas.
- **Do** preservar comportamentos reais do produto, como Enter avancando foco e retorno para a lista apos salvar/cancelar.
- **Do** garantir foco visivel, contraste suficiente e navegacao por teclado.

### Don't:
- **Don't** usar cores hard-coded em cada XAML para botoes, headers e fundos.
- **Don't** repetir estilos de DataGrid em cada tela.
- **Don't** depender de larguras fixas para toda a tela quando o layout puder se adaptar.
- **Don't** usar MessageBox como experiencia visual padrao.
- **Don't** usar vermelho para salvar ou confirmar; vermelho deve indicar risco.
- **Don't** usar vermelho para Importar. Importar e uma acao primaria ou de continuidade, nao uma acao destrutiva.
- **Don't** esconder acoes importantes em botoes sem texto ou tooltip.
- **Don't** adicionar efeitos visuais decorativos que nao ajudem a operacao.
