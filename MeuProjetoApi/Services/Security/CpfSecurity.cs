using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace MeuProjetoApi.Services.Security
{
    public class CpfSecurity
    {
        private readonly byte[] EncryptionKey;

        private const int KeySize = 32; // 256 bits
        private const int IvSize = 12;  // 96 bits (padrão GCM)
        private const int TagSize = 16; // 128 bits (padrão GCM)

        public CpfSecurity(string encryptionKey)
        {
            this.EncryptionKey = Encoding.UTF8.GetBytes(encryptionKey);

            if (this.EncryptionKey.Length != KeySize)
            {
                throw new InvalidOperationException($"A chave de criptografia deve ter exatamente {KeySize} bytes (256 bits). Atual: {this.EncryptionKey.Length}");
            }
        }

        /// <summary>
        /// Gera um hash criptográfico irreversível (SHA256) para o CPF, usado para unicidade.
        /// </summary>
        public string GerarHash(string cpfLimpo)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cpfLimpo));
                return Convert.ToBase64String(hashBytes);
            }
        }


        /// <summary>
        /// Criptografa o CPF usando AES-256 GCM e armazena o IV (vetor de inicialização) junto.
        /// </summary>
        public string Criptografar(string cpfLimpo)
        {
            if (this.EncryptionKey.Length != KeySize)
            {
                throw new CryptographicException($"A chave deve ter exatamente {KeySize} bytes.");
            }

            using (var aesGcm = new AesGcm(this.EncryptionKey)) // Usa 'this.EncryptionKey'
            {
                // ... (lógica de criptografia, tag e combinação de bytes é a mesma) ...

                // 1. Geração do IV (Vector de Inicialização) único
                var nonce = new byte[IvSize];
                RandomNumberGenerator.Fill(nonce);

                // 2. Buffers de saída
                var plaintextBytes = Encoding.UTF8.GetBytes(cpfLimpo);
                var ciphertext = new byte[plaintextBytes.Length];
                var tag = new byte[TagSize];

                // 3. Criptografar
                aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag);

                // 4. Combina IV, Tag e Ciphertext em um array para armazenamento
                var resultBytes = new byte[nonce.Length + tag.Length + ciphertext.Length];

                Buffer.BlockCopy(nonce, 0, resultBytes, 0, nonce.Length);
                Buffer.BlockCopy(tag, 0, resultBytes, nonce.Length, tag.Length);
                Buffer.BlockCopy(ciphertext, 0, resultBytes, nonce.Length + tag.Length, ciphertext.Length);

                // 5. Retorna a combinação em Base64
                return Convert.ToBase64String(resultBytes);
            }
        }

        // --- Método Descriptografar (DEVE ser de INSTÂNCIA) ---

        /// <summary>
        /// Descriptografa o CPF, separando o IV, a Tag e os Dados Criptografados.
        /// </summary>
        // MUDANÇA: O método Descriptografar NÃO É MAIS 'STATIC'
        public string Descriptografar(string cpfCriptografado)
        {
            if (this.EncryptionKey.Length != KeySize)
            {
                throw new CryptographicException($"A chave deve ter exatamente {KeySize} bytes.");
            }

            // ... (lógica de descriptografia e separação de bytes é a mesma) ...

            // 1. Converte a string Base64 de volta para bytes
            var fullCipher = Convert.FromBase64String(cpfCriptografado);

            // Verifica o tamanho mínimo esperado (IV + Tag)
            if (fullCipher.Length < IvSize + TagSize)
            {
                throw new FormatException("O texto criptografado é inválido ou incompleto.");
            }

            // 2. Separa os componentes (IV, Tag e Ciphertext)
            var nonce = new byte[IvSize];
            var tag = new byte[TagSize];
            var ciphertext = new byte[fullCipher.Length - IvSize - TagSize];

            Buffer.BlockCopy(fullCipher, 0, nonce, 0, nonce.Length);
            Buffer.BlockCopy(fullCipher, nonce.Length, tag, 0, tag.Length);
            Buffer.BlockCopy(fullCipher, nonce.Length + tag.Length, ciphertext, 0, ciphertext.Length);

            // 3. Descriptografar e Autenticar
            using (var aesGcm = new AesGcm(this.EncryptionKey)) // Usa 'this.EncryptionKey'
            {
                var decryptedBytes = new byte[ciphertext.Length];

                try
                {
                    aesGcm.Decrypt(nonce, ciphertext, tag, decryptedBytes);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
                catch (CryptographicException ex)
                {
                    throw new CryptographicException("Falha na autenticação do CPF. Os dados podem ter sido adulterados.", ex);
                }
            }
        }
    }
}