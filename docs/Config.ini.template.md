# Template Seguro: Config.ini

## Uso

Crie um arquivo chamado `Config.ini` no mesmo diretorio do `ControleDeWebServices.exe` publicado. Este arquivo contem dados de ambiente e credenciais, portanto nao deve ser commitado no repositorio.

## Autenticacao SQL Com Usuario E Senha

```ini
[CONEXAO]
Servidor=SERVIDOR_SQL
DataBase=NOME_DO_BANCO
OSAuthent=FALSE
Usuario=USUARIO_DO_BANCO
Senha=SENHA_DO_BANCO
```

## Autenticacao Integrada Do Windows

```ini
[CONEXAO]
Servidor=SERVIDOR_SQL
DataBase=NOME_DO_BANCO
OSAuthent=TRUE
Usuario=
Senha=
```

## Observacoes

- `OSAuthent=TRUE` usa `Integrated Security=True`.
- `OSAuthent=FALSE` usa `User ID` e `Password`.
- Nao usar senhas reais em documentos, prints, commits ou chamados sem mascaramento.
- O arquivo deve ficar fora do Git e ser entregue por canal seguro ao ambiente alvo.
