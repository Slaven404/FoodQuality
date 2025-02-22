using AnalysisEngine.DTOs;
using Shared.Enums;
using System.Security.Cryptography;

namespace AnalysisEngine.Services
{
    public class AnalysisService
    {
        public async Task<string> PerformAnalysisAsync(AnalysisRequestDto request)
        {
            await Task.Delay(2000);

            return GenerateRandomDigitString((AnalysisTypeEnum)request.AnalysisTypeId);
        }

        private string GenerateRandomDigitString(AnalysisTypeEnum analysisType)
        {
            int numberOfDigits = analysisType switch
            {
                AnalysisTypeEnum.Microbiological => 8,
                AnalysisTypeEnum.Chemical => 7,
                AnalysisTypeEnum.Sensory => 6,
                _ => 6
            };

            Span<byte> buffer = stackalloc byte[8];
            RandomNumberGenerator.Fill(buffer);
            ulong number = BitConverter.ToUInt64(buffer) % (ulong)Math.Pow(10, numberOfDigits); ;
            return number.ToString($"D{numberOfDigits}");
        }
    }
}