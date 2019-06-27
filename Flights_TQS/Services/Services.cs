using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Flights_TQS.Services
{
    public static class Extensions
    {
        public static bool In<T>(this T item, params T[] values) where T : struct
        {
            return values.Contains(item);
        }

        public static string Descricao<T>(this T item) where T : struct
        {
            var info = typeof(T).GetMember(item.ToString());
            var atributos = info[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (atributos.Length == 0)
                return item.ToString();
            else
                return ((System.ComponentModel.DescriptionAttribute)atributos[0]).Description;
        }

        public static StringBuilder Diferencas<T>(this T itemOld, T itemNew, List<string> exclusoes = null)
        {
            if (exclusoes == null) exclusoes = new List<string>();

            var resultado = new StringBuilder();
            var type = itemOld.GetType();

            foreach (var property in type.GetProperties())
            {
                var valueOld = property.GetValue(itemOld, null);
                var valueNew = (EqualityComparer<T>.Default.Equals(itemNew, default(T)) ? null : property.GetValue(itemNew, null));

                // this will handle the scenario where either value is null
                if (!(valueOld is System.Collections.IEnumerable) && !exclusoes.Any(p => p.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)) && !Equals(valueOld, valueNew))
                {
                    // Handle the display values when the underlying value is null
                    var valueOldStr = ((valueOld == null) ? "null" : valueOld.ToString());
                    var valueNewStr = ((valueNew == null) ? "null" : valueNew.ToString());

                    if (itemNew == null)
                    {
                        var valueTemp = valueOldStr;

                        valueOldStr = valueNewStr;
                        valueNewStr = valueTemp;
                    }

                    resultado.AppendLine($"• {property.Name}: {valueOldStr} >> {valueNewStr}");
                }
            }

            return resultado;
        }
    }

    public static class HttpRequestExtensions
    {
        public static bool IsLocal(this HttpRequest request)
        {
            ConnectionInfo connection = request.HttpContext.Connection;

            if (connection.RemoteIpAddress != null)
            {
                if (connection.LocalIpAddress != null)
                    return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
                else
                    return IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            // for in memory TestServer or when dealing with default connection info
            if ((connection.RemoteIpAddress == null) && (connection.LocalIpAddress == null))
                return true;

            return false;
        }
    }

    public static class Utils
    {
        #region // Atributos //
        public static bool RunningXP
        {
            get
            {
                int major = System.Environment.OSVersion.Version.Major;
                int minor = System.Environment.OSVersion.Version.Minor;

                return (major == 5 && minor == 1);
            }
        }

        public static string LocalIPAddress
        {
            get
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        return ip.ToString();

                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }
        #endregion

        public static T Converter<T>(object value) where T : IConvertible
        {
            if ((value == null) || Convert.IsDBNull(value))
                if (typeof(T) == typeof(String))
                    return (T)Convert.ChangeType(String.Empty, typeof(T));
                else
                    return default(T);
            else
              if (!typeof(T).IsEnum)
                if (value == null || Convert.IsDBNull(value))
                    return default(T);
                else
                    return (T)Convert.ChangeType(value, typeof(T));
            else
            {
                if (!Enum.IsDefined(typeof(T), value))
                    throw new ArgumentOutOfRangeException();

                return (T)Enum.ToObject(typeof(T), value);
            }
        }

        public static string RandomDarkColor(Random random = null)
        {
            if (random == null) random = new Random();
            return String.Format("#{0:X6}", random.Next(0x1000000) & 0x7F7F7F);
        }
    }

    public static class UtilsString
    {
        public static string RandomPassword(int length)
        {
            if (length <= 0)
                return String.Empty;

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%{}[]()/',;:.<>";
            StringBuilder res = new StringBuilder();

            using (RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rnd.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);

                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        public static bool IsNumeric(string s)
        {
            return Decimal.TryParse(s, out decimal n);
        }

        public static string NumericOnly(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            else
                return new String(s.Where(char.IsDigit).ToArray());
        }

        public static string Guid()
        {
            return System.Guid.NewGuid().ToString("D").ToUpper();
        }
    }

    public static class UtilsDateTime
    {
        public static DateTime DateTimeBrasil()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(@"E. South America Standard Time"));
        }
    }

    public static class UtilsFile
    {
        public static string NomeArquivoValido(string nomeArquivo)
        {
            return Path.GetInvalidFileNameChars().Aggregate(nomeArquivo, (current, c) => current.Replace(c.ToString(), String.Empty));
        }
    }

    public static class UtilsAspose
    {
        private static string AsposeLicense2019 = @"
      <License>
        <Data>
          <LicensedTo>Shanghai Hudun Information Technology Co., Ltd</LicensedTo>
          <EmailTo>317701809@qq.com</EmailTo>
          <LicenseType>Developer OEM</LicenseType>
          <LicenseNote>Limited to 1 developer, unlimited physical locations</LicenseNote>
          <OrderID>180514201116</OrderID>
          <UserID>266166</UserID>
          <OEM>This is a redistributable license</OEM>
          <Products>
            <Product>Aspose.Total for .NET</Product>
          </Products>
          <EditionType>Enterprise</EditionType>
          <SerialNumber>210ec8e7-81e1-4537-b446-692de4981217</SerialNumber>
          <SubscriptionExpiry>20190517</SubscriptionExpiry>
          <LicenseVersion>3.0</LicenseVersion>
          <LicenseInstructions>http://www.aspose.com/corporate/purchase/license-instructions.aspx</LicenseInstructions>
        </Data>
        <Signature>ctJ3yLxSAPsBQd0Jcqf7CA53FzN1YrvaA5dSrTpdFW/Afh0hyKKwry+C1tjWIOEFyzKYWH+Ngn/HeXUzMQJA0Roowcq112nV/QnrSSqDm6FJVNssH4p/YmXRjl7LBixwV8AbyWX8lhVoyok7lI5k5K8bbaK+T8Ur+jIwSZAcmVA=</Signature>
      </License>
    ";

        
    }


    public class NumeroPorExtenso
    {
        private readonly List<int> numeroLista;
        private int num;

        // array de 2 linhas e 14 colunas[2][14]
        private static readonly String[,] qualificadores = new String[,] {
      {"centavo", "centavos"}, //[1][0] e [1][1]
      {"", ""}, //[2][0] e [2][1]
      {"mil", "mil"},
      {"milhão", "milhões"},
      {"bilhão", "bilhões"},
      {"trilhão", "trilhões"},
      {"quatrilhão", "quatrilhões"},
      {"quintilhão", "quintilhões"},
      {"sextilhão", "sextilhões"},
      {"setilhão", "setilhões"},
      {"octilhão", "octilhões"},
      {"nonilhão", "nonilhões"},
      {"decilhão", "decilhões"}
    };

        private static readonly String[,] numeros = new String[,] {
      {"zero", "um", "dois", "três", "quatro",
        "cinco", "seis", "sete", "oito", "nove",
        "dez","onze", "doze", "treze", "quatorze",
        "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"},
      {"vinte", "trinta", "quarenta", "cinqüenta", "sessenta",
        "setenta", "oitenta", "noventa",null,null,null,null,null,null,null,null,null,null,null,null},
      {"cem", "cento",
        "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos",
        "setecentos", "oitocentos", "novecentos",null,null,null,null,null,null,null,null,null,null}
    };

        public NumeroPorExtenso()
        {
            numeroLista = new List<int>();
        }

        public NumeroPorExtenso(Decimal dec)
        {
            numeroLista = new List<int>();
            SetNumero(dec);
        }

        public void SetNumero(Decimal dec)
        {
            dec = Decimal.Round(dec, 2);
            dec = dec * 100;
            num = Convert.ToInt32(dec);
            numeroLista.Clear();

            if (num == 0)
            {
                numeroLista.Add(0);
                numeroLista.Add(0);
            }
            else
            {
                AddRemainder(100);
                while (num != 0)
                    AddRemainder(1000);
            }
        }

        private void AddRemainder(int divisor)
        {
            int div = num / divisor;
            int mod = num % divisor;

            numeroLista.Add(mod);
            num = div;
        }

        private bool TemMaisGrupos(int ps)
        {
            while (ps > 0)
            {
                if (numeroLista[ps] != 00 && !TemMaisGrupos(ps - 1))
                    return true;

                ps--;
            }

            return true;
        }

        private bool EhPrimeiroGrupoUm()
        {
            return (numeroLista[numeroLista.Count - 1] == 1);
        }

        private bool IsGrupoZero(Int32 ps)
        {
            if (ps <= 0 || ps >= numeroLista.Count)
                return true;

            return (numeroLista[ps] == 0);
        }

        private bool IsUnicoGrupo()
        {
            if (numeroLista.Count <= 3)
                return false;

            if (!IsGrupoZero(1) && !IsGrupoZero(2))
                return false;

            bool hasOne = false;

            for (Int32 i = 3; i < numeroLista.Count; i++)
            {
                if (numeroLista[i] != 0)
                {
                    if (hasOne)
                        return false;

                    hasOne = true;
                }
            }

            return true;
        }

        private String NumToString(int numero, int escala)
        {
            int unidade = (numero % 10);
            int dezena = (numero % 100);
            int centena = (numero / 100);
            var buf = new StringBuilder();

            if (numero != 0)
            {
                if (centena != 0)
                {
                    if (dezena == 0 && centena == 1)
                        buf.Append(numeros[2, 0]);
                    else
                        buf.Append(numeros[2, centena]);
                }

                if (buf.Length > 0 && dezena != 0)
                    buf.Append(" e ");

                if (dezena > 19)
                {
                    dezena = dezena / 10;
                    buf.Append(numeros[1, dezena - 2]);
                    if (unidade != 0)
                    {
                        buf.Append(" e ");
                        buf.Append(numeros[0, unidade]);
                    }
                }
                else if (centena == 0 || dezena != 0)
                    buf.Append(numeros[0, dezena]);

                buf.Append(" ");
                if (numero == 1)
                    buf.Append(qualificadores[escala, 0]);
                else
                    buf.Append(qualificadores[escala, 1]);
            }

            return buf.ToString();
        }

        public override String ToString()
        {
            var buf = new StringBuilder();

            for (var count = numeroLista.Count - 1; count > 0; count--)
            {
                if (buf.Length > 0 && !IsGrupoZero(count))
                    buf.Append(" e ");

                buf.Append(NumToString(numeroLista[count], count));
            }

            if (buf.Length > 0)
            {
                while (buf.ToString().EndsWith(" "))
                    buf.Length = buf.Length - 1;

                if (IsUnicoGrupo())
                    buf.Append(" de ");

                if (EhPrimeiroGrupoUm())
                    buf.Insert(0, "h");

                if (numeroLista.Count == 2 && (numeroLista[1] == 1))
                    buf.Append(" real");
                else
                    buf.Append(" reais");

                if (numeroLista[0] != 0)
                    buf.Append(" e ");
            }

            if (numeroLista[0] != 0)
                buf.Append(NumToString(numeroLista[0], 0));

            return buf.ToString();
        }
    }
}
