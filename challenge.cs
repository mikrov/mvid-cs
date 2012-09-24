using System;
using System.Collections;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using JsonWsp;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Text;

// MV-ID integration
// -----------------
// In order to use MV-ID's web services you must register user sessions using the SessionSecurity
// web service method registerSessionUsage():
//
//   https://signon.mv-nordic.com/session-security/SessionSecurity
//
// This method is a challenge-response mechanism, that ensures that only a registered application
// server can create a valid mv_session_id. It takes two arguments:
// 1. The mv_session_hash recieved after successful login.
// 2. The domain name registered for the application.
//
// The result of this call is a generated nonce value to salt the challenge response. Now there
// are 3 values known by both MV-ID and the application server: mv_session_hash, nonse and the
// key shared registed with the domain. The mv_session_id is calculated as follows:
//
//   $mv_session_id = md5($mv_session_hash.$nonce.$shared_key);

namespace Mvid {
	public class Challenge {
		public static bool CommonCall(JsonWsp.Client cli, string method_name, JsonObject args_dict, ref JsonObject result, ref string error_message) {
			// Call method
			JsonWsp.Response response = cli.CallMethod(method_name,args_dict);
			// Print
			JsonWsp.Response.CallResult cr = response.GetCallResult();
			if (cr==JsonWsp.Response.CallResult.Success) {
				result = (Jayrock.Json.JsonObject) response.GetJsonResponse()["result"];
				return true;
			}
			if (cr==JsonWsp.Response.CallResult.ServiceFault) {
				error_message = response.GetServiceFault().GetString();
				return false;
			}
			return false;
		}
		
		public static string GetMD5Hash(string input) {
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs) {
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();
	    }

		public static bool registerSessionUsage (string mv_session_hash,string domain, string shared_key,ref string mv_session_id,ref string error_message)	{
			JsonWsp.Client cli = new JsonWsp.Client("https://signon.mv-nordic.com/session-security/SessionSecurity/jsonwsp/description");
			cli.SetViaProxy(true);
			
			// Build arguments
			JsonObject args_dict = new JsonObject();
			args_dict.Add("mv_session_hash",mv_session_hash);
			args_dict.Add("domain","mv-id-test.valhalla.local");
			// Call method
			JsonObject result = new JsonObject();
			bool success = CommonCall(cli,"registerSessionUsage",args_dict,ref result,ref error_message);
			if (success) {
				JsonObject method_result = (JsonObject) result["method_result"];
				JsonNumber test = (JsonNumber) method_result["res_code"];
				if (((JsonNumber)method_result["res_code"]).ToInt32() != 0) {
					error_message = (string) method_result["res_msg"];
					return false;
				}
				string nonce = (string) result["nonce"];
				mv_session_id = GetMD5Hash(mv_session_hash + nonce + shared_key);
				return true;
			}
			return false;
		}
	}
}
