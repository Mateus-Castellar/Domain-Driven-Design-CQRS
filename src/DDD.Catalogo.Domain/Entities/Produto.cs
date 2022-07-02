using DDD.Core.DomainObjects;

namespace DDD.Catalogo.Domain.Entities
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public Dimensoes Dimensoes { get; private set; }
        public int QuantidadeEstoque { get; private set; }

        //EfCore Relation
        public Categoria Categoria { get; private set; }
        public Guid CategoriaId { get; private set; }

        public Produto(string nome,
                       string descricao,
                       bool ativo,
                       decimal valor,
                       Guid categoriaId,
                       DateTime dataCadastro,
                       string imagem,
                       Dimensoes dimensoes)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            CategoriaId = categoriaId;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricaoProduto(string descicao)
        {
            Validacoes.ValidarSeVazio(descicao, "A descrição não pode ser vazia");
            Descricao = descicao;
        }

        public void DebitarEstoque(int quantidade)
        {
            //caso o numero a ser debitado venha negativo (ex: debite -10 peças)
            if (quantidade < 0)
                quantidade *= -1;

            if (VerficarEstoque(quantidade) is false)
                throw new DomainException("Estoque insuficiente");

            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoques(int quantidade) => QuantidadeEstoque += quantidade;

        public bool VerficarEstoque(int quantidadeVerficar) => QuantidadeEstoque >= quantidadeVerficar;

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do produto não pode estar vazio");
            Validacoes.ValidarSeVazio(Descricao, "O campo Descricao do produto não pode estar vazio");
            Validacoes.ValidarSeIgual(CategoriaId, Guid.Empty, "O campo CategoriaId do produto não pode estar vazio");
            Validacoes.ValidarSeMenorQue(Valor, 1, "O campo Valor do produto não pode se menor igual a 0");
            Validacoes.ValidarSeVazio(Imagem, "O campo Imagem do produto não pode estar vazio");
        }
    }
}
