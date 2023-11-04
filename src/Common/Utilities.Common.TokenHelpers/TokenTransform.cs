using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Utilities.Common.TokenHelpers
{
    /// <summary>
    /// Defines the <see cref="TokenTransform" />
    /// </summary>
    public static class TokenTransform
    {
        /// <summary>
        /// The TransformIfProtectedForwardedToken
        /// </summary>
        /// <param name="token">The token<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string TransformIfProtectedForwardedToken(string token)
        {
            if (token != null)
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                string transformedToken = TransformProtectedForwardedToken(jwtSecurityTokenHandler, token, jwtSecurityToken);

                return transformedToken ?? token;
            }

            return null;
        }

        /// <summary>
        /// The TransformProtectedForwardedToken
        /// </summary>
        /// <param name="jwtSecurityTokenHandler">The jwtSecurityTokenHandler<see cref="JwtSecurityTokenHandler"/></param>
        /// <param name="token">The token<see cref="string"/></param>
        /// <param name="jwtSecurityToken">The jwtSecurityToken<see cref="JwtSecurityToken"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string TransformProtectedForwardedToken(JwtSecurityTokenHandler jwtSecurityTokenHandler, string token, JwtSecurityToken jwtSecurityToken)
        {
            if (!jwtSecurityToken.Header.TryGetValue(JwtRegisteredClaimNames.Nonce, out object nonceHeader))
            {
                return null;
            }

            string nonce = nonceHeader as string;

            if (string.IsNullOrEmpty(nonce))
            {
                return null;
            }

            jwtSecurityToken.Header[JwtRegisteredClaimNames.Nonce] = GetHashedNonce(nonce);

            string newToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            string[] originalTokenParts = token.Split('.');
            string[] updatedTokenParts = newToken.Split('.');

            if (originalTokenParts.Length != 3 || updatedTokenParts.Length < 2)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder(token.Length);
            sb.Append(updatedTokenParts[0]);
            sb.Append('.');
            sb.Append(originalTokenParts[1]);
            sb.Append('.');
            sb.Append(originalTokenParts[2]);

            return sb.ToString();
        }

        /// <summary>
        /// The GetHashedNonce
        /// </summary>
        /// <param name="nonce">The nonce<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetHashedNonce(string nonce)
        {
            using (SHA256 hashAlgorithm = SHA256.Create())
            {
                return Base64UrlEncoder.Encode(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(nonce)));
            }
        }
    }
}
