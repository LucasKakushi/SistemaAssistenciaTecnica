namespace AssistenciaTecnica.Domain.Entities
{
    public class Tenant
    {
        public Guid Id { get; private set; }
        public string Codigo { get; private set; }
        public string Nome { get; private set; }

        public Tenant(string codigo, string nome)
        {
            ValidarCampoObrigatorio(codigo, nameof(codigo));
            ValidarCampoObrigatorio(nome, nameof(nome));

            Id = Guid.NewGuid();
            Codigo = codigo;
            Nome = nome;
        }

        private static void ValidarCampoObrigatorio(string ?valor, string nomeCampo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException("O campo é obrigatório!", nomeCampo);
            }
        }
    }
}