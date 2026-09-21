/*
 * Cliente
 *
 * Identidade
 * - Id: Guid
 *   - não deve poder ser alterado de fora
 *   - deve ser criado automaticamente
 *
 * Dados
 * - RazaoSocial: string
 *   - não pode ser alterada livremente de fora
 *   - null, "", e "   " são inválidos
 *
 * - Cnpj: string
 *   - não pode ser alterado livremente de fora
 *   - null, "", e "   " são inválidos
 */

namespace AssistenciaTecnica.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public string RazaoSocial { get; private set; }
        public string Cnpj { get; private set; }

        public Cliente(Guid tenantId, string razaoSocial, string cnpj)
        {
            ValidarCampoObrigatorio(razaoSocial, nameof(razaoSocial));
            ValidarCampoObrigatorio(cnpj, nameof(cnpj));

            ValidarTenantIdVazio(tenantId, nameof(tenantId));

            var cnpjNormalizado = NormalizarCnpj(cnpj);
            ValidarCampoObrigatorio(cnpjNormalizado, nameof(cnpj));

            VerificarQuantidadeCaracteresCnpj(cnpjNormalizado, nameof(cnpj));

            VerificarCaracteresCnpj(cnpjNormalizado, nameof(cnpj));

            VerificarUltimosDoisDigitos(cnpjNormalizado, nameof(cnpj));

            Id = Guid.NewGuid();
            TenantId = tenantId;
            RazaoSocial = razaoSocial;
            Cnpj = cnpjNormalizado;
        }

        private static void ValidarCampoObrigatorio(string? valor, string campoObrigatorio)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException("O campo é obrigatório!", campoObrigatorio);
            }
        }

        private static void ValidarTenantIdVazio(Guid tenantId, string nomeCampo)
        {
            if(tenantId == Guid.Empty)
            {
                throw new ArgumentException("TentantId está vazio!", nomeCampo);
            }
        }

        private static string NormalizarCnpj(string cnpj)
        {
            cnpj = cnpj.Replace(".", "");
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace("-", "");

            return cnpj;
        }

        private static void VerificarQuantidadeCaracteresCnpj(string cnpj, string nomeCampo)
        {
            if(cnpj.Length != 14)
            {
                throw new ArgumentException("Cnpj Inválido!", nomeCampo);
            }
        }

        private static void VerificarCaracteresCnpj(string cnpj, string nomeCampo)
        {
            

            foreach(var caractere in cnpj)
            {
                bool isDigito = caractere >= '0' && caractere <= '9';
                bool isLetraMaiuscula = caractere >= 'A' && caractere <= 'Z';
                bool isLetraMinuscula = caractere >= 'a' && caractere <= 'z';

                if (!isDigito && !isLetraMaiuscula && !isLetraMinuscula)
                {
                    throw new ArgumentException("O CNPJ contém caracteres não permitidos.", nomeCampo);
                }

            }
        }

        private static void VerificarUltimosDoisDigitos(string cnpj, string nomeCampo)
        {
            char penultimo = cnpj[cnpj.Length - 2];
            char ultimo = cnpj[cnpj.Length - 1];
            bool penultimoIsDigito = penultimo >= '0' && penultimo <= '9';
            bool ultimoIsDigito = ultimo >= '0' && ultimo <= '9';

            if(!ultimoIsDigito || !penultimoIsDigito)
            {
                throw new ArgumentException("O CNPJ contém caracteres não permitidos.", nomeCampo);
            }
        }

        private static int CalcularPrimeiroDigito(string cnpj)
        {
            int[] pesos = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for(int i = 0; i < pesos.Length; i++)
            {
                int valor = cnpj[i] - '0';
                soma += valor * pesos[i];
            }
            int resto = soma % 11;
            int digito = resto < 2 ? 0 : 11 - resto;
            return digito;
        }
    }
}