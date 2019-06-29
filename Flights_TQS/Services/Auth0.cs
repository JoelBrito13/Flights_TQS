using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Auth0.Core.Collections;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Flights_TQS.Interfaces;
using Flights_TQS.Entities;

namespace Flights_TQS.Services
{
    public enum JwtHashAlgorithm
    {
        RS256,
        HS384,
        HS512
    }

    public static class JsonWebToken
    {
        private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> {
        { JwtHashAlgorithm.RS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
        { JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
        { JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
      };
        }

        public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        {
            return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
        }

        public static string Encode(object payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
        {
            var segments = new List<string>();
            var header = new { alg = algorithm.ToString(), typ = "JWT" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));
            //byte[] payloadBytes = Encoding.UTF8.GetBytes(@"{"iss":"761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com","scope":"https://www.googleapis.com/auth/prediction","aud":"https://accounts.google.com/o/oauth2/token","exp":1328554385,"iat":1328550785}");

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            var stringToSign = string.Join(".", segments.ToArray());

            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        public static string Decode(string token, string key)
        {
            return Decode(token, key, true);
        }

        public static string Decode(string token, string key, bool verify)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JObject.Parse(headerJson);
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JObject.Parse(payloadJson);

            if (verify)
            {
                var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var algorithm = (string)headerData["alg"];

                var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
                var decodedCrypto = Convert.ToBase64String(crypto);
                var decodedSignature = Convert.ToBase64String(signature);

                if (decodedCrypto != decodedSignature)
                {
                    ApplicationException applicationException = new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
                    throw applicationException;
                }
            }

            return payloadData.ToString();
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "RS256": return JwtHashAlgorithm.RS256;
                case "HS384": return JwtHashAlgorithm.HS384;
                case "HS512": return JwtHashAlgorithm.HS512;
                default: throw new InvalidOperationException("Algorithm not supported.");
            }
        }

        // from JWT spec
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }

    public class JsonAccessToken
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class Management
    {
        private readonly IAppServices AppServices;

        public Management(IAppServices appServices)
        {
            AppServices = appServices;
        }


        private bool HasAccessToken(out string token)
        {
            string url = $"{AppServices.Auth0Settings["ManagementApi"]}/oauth/token";

            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(Method.POST);

            var parameter = new
            {
                grant_type = "client_credentials",
                client_id = AppServices.Auth0Settings["ManagementClientId"],
                client_secret = AppServices.Auth0Settings["ManagementClientSecret"],
                audience = AppServices.Auth0Settings["ManagementAudience"]
            };

            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(parameter), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            token = (response.IsSuccessful ? response.Content : null);
            return response.IsSuccessful;
        }

        public async Task<IPagedList<User>> ListAsync()
        {
            if (HasAccessToken(out string token))
            {
                var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);

                var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                //await client.Users.GetAllAsync();
            }

            return null;
        }

        public async Task<User> ReadAsync(string id)
        {
            if (HasAccessToken(out string token))
            {
                var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);

                var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                var user = await client.Users.GetAsync(id);

                if (user != null)
                {
                    user.AppMetadata = user.AppMetadata ?? new Dictionary<string, object>();
                    user.UserMetadata = user.UserMetadata ?? new Dictionary<string, object>();
                }

                return user;
            }
            else
                return null;
        }

        public async Task<User> ReadByEmailAsync(string email)
        {
            if (HasAccessToken(out string token))
            {
                var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);

                var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                var users = await client.Users.GetUsersByEmailAsync(email);

                return ((users.Count == 0) ? null : users[0]);
            }

            return null;
        }

        public async Task<User> IncludeAsync(int id, string email, string password, string fName, string lName)
        {
            try
            {
                if (HasAccessToken(out string token))
                {
                    var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);

                    var userCreateRequest = new UserCreateRequest()
                    {
                        Connection = AppServices.Auth0Settings["Connection"],
                        VerifyEmail = true,
                        EmailVerified = false,

                        Email = email,
                        Password = password,
                        FirstName = fName,
                        LastName = lName,
                        FullName = String.Format("{0} {1}", fName, lName).Trim(),
                        AppMetadata = new
                        {
                            idUser = id
                        }
                    };

                    var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                    return await client.Users.CreateAsync(userCreateRequest);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> EditAsync(FlightUser flightUser)
        {
            if (HasAccessToken(out string token))
            {
                var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);

                var userUpdateRequest = new UserUpdateRequest()
                {
                    Connection = AppServices.Auth0Settings["Connection"],
                    ClientId = AppServices.Auth0Settings["ManagementClientId"],

                    Email = flightUser.Email
                };

                var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                var user = ReadByEmailAsync(flightUser.Email).Result;

                if (user == null)
                    return null;
                else
                    return await client.Users.UpdateAsync(user.UserId, userUpdateRequest);
            }

            return null;
        }

        public async Task<User> EditAppMetadataAsync(FlightUser flightUser)
        {
            if (HasAccessToken(out string token))
            {
                var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);

                var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                var user = ReadByEmailAsync(flightUser.Email).Result;

                if (user == null)
                    return null;
                else
                {
                    var userUpdateRequest = new UserUpdateRequest()
                    {
                        Connection = AppServices.Auth0Settings["Connection"],
                        ClientId = AppServices.Auth0Settings["ManagementClientId"],

                        AppMetadata = user.AppMetadata ?? new Dictionary<string, object>()
                    };

                    // AppMetadata.idxUsuario
                    if ((userUpdateRequest.AppMetadata as JObject).ContainsKey("idFlightUser"))
                        userUpdateRequest.AppMetadata["idFlightUser"] = flightUser.Id;
                    else
                        (userUpdateRequest.AppMetadata as JObject).Add("idFlightUser", flightUser.Id);

                    return await client.Users.UpdateAsync(user.UserId, userUpdateRequest);
                }
            }

            return null;
        }

        public async Task DeleteAsync(FlightUser flightUser)
        {
            if (HasAccessToken(out string token))
            {
                var accessToken = JsonConvert.DeserializeObject<JsonAccessToken>(token);
                var client = new ManagementApiClient(accessToken.access_token, AppServices.Auth0Settings["Domain"]);
                var user = ReadByEmailAsync(flightUser.Email).Result;

                if (user == null)
                    return;
                else
                {
                    var userUpdateRequest = new UserUpdateRequest()
                    {
                        Connection = AppServices.Auth0Settings["Connection"],
                        ClientId = AppServices.Auth0Settings["ManagementClientId"],

                        Blocked = true
                    };

                    await client.Users.UpdateAsync(user.UserId, userUpdateRequest);
                }

            }
            else
                return;
        }
    }
}
