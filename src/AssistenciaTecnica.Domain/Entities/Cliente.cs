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

            ValidarTentantIdVazio(tenantId, nameof(tenantId));

            Id = Guid.NewGuid();
            TenantId = tenantId;
            RazaoSocial = razaoSocial;
            Cnpj = cnpj;
        }

        private static void ValidarCampoObrigatorio(string? valor, string campoObrigatorio)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException("O campo é obrigatório!", campoObrigatorio);
            }
        }

        private static void ValidarTentantIdVazio(Guid tenantId, string nomeCampo)
        {
            if(tenantId == Guid.Empty)
            {
                throw new ArgumentException("TentantId está vazio!", nomeCampo);
            }
        }
    }
}