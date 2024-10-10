namespace StudioTattooManagement.Utils.UtilsClasses
{
    public class Preco
    {
        // Propriedade somente leitura
        public decimal Valor { get; private set; }

        // Construtor que faz a validação
        public Preco(decimal valor)
        {
            // Fail-fast validation
            if (valor <= 0)
                throw new ArgumentException("O valor do preço deve ser maior que zero.", nameof(valor));

            Valor = valor;
        }

        // Método de fábrica estático para criar um Preco de forma mais semântica, se necessário
        public static Preco Criar(decimal valor)
        {
            return new Preco(valor);
        }

        // Sobrescrever ToString para facilitar a exibição do preço
        public override string ToString()
        {
            return Valor.ToString("C"); // Exibe o valor em formato de moeda, ex: R$100,00
        }

        // Sobrescrever Equals e GetHashCode para comparação de igualdade por valor
        public override bool Equals(object obj)
        {
            if (obj is Preco other)
                return Valor == other.Valor;

            return false;
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }

        // Operadores para comparação entre Precos
        public static bool operator >(Preco a, Preco b) => a.Valor > b.Valor;
        public static bool operator <(Preco a, Preco b) => a.Valor < b.Valor;
        public static bool operator >=(Preco a, Preco b) => a.Valor >= b.Valor;
        public static bool operator <=(Preco a, Preco b) => a.Valor <= b.Valor;
    }
}
