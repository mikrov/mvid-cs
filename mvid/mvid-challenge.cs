using System;
using System.Web;
using System.Net;
using System.Text;
using JsonWsp;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System.Security.Cryptography;

public class mvid_challenge
{
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
  //   mv_session_id = md5(mv_session_hash + nonce + shared_key);

  public Boolean registerSessionUsage(string mv_session_hash,string domain,string shared_key,ref string mv_session_id,ref string error_message)
  {
    // Connect to MV-ID's session security service to register a valid application session
    Client client = new Client("https://signon.mv-nordic.com/session-security/SessionSecurity/jsonwsp/description");
    client.SetViaProxy(true);
    // Build arguments
    JsonObject args_dict = new JsonObject();
    args_dict.Add("mv_session_hash", mv_session_hash);
    args_dict.Add("domain", domain);
    // Send registration request
    Response response = client.CallMethod("registerSessionUsage",args_dict);
    if(response.GetJsonWspType() == Response.JsonWspType.Response && response.GetCallResult() == Response.CallResult.Success) {
      // Get recieve a server-to-server nonse in order to respond to MV-ID's challenge
      JsonObject responseJson = response.GetJsonResponse();
      JsonObject result = (JsonObject) responseJson["result"];
      JsonObject method_result = (JsonObject) result["method_result"];
      if (((JsonNumber) method_result["res_code"]).ToInt32() != 0) {
        error_message = method_result["res_msg"].ToString();
        return false;
      }
      string nonce = result["nonce"].ToString();
      // Build mv_session_id and store it as a cookie
      mv_session_id = GetMD5Hash(mv_session_hash + nonce + shared_key);
      // Check the generated mv_session_id
      WebClient web_reader = new WebClient();
      Boolean valid_session_id = Boolean.Parse(web_reader.DownloadString("https://signon.mv-nordic.com/sp-tools/is_authenticated?mv_session_id=" + mv_session_id));
      if (!valid_session_id) {
        error_message = "The generated mv_session_id is invalid - check that the application domain and shared key are correct entered.";
        return false;
      }
      return true;
    }
    else if (response.GetJsonWspType() == JsonWsp.Response.JsonWspType.Fault)
    {
      // Handle service fault here
      error_message = "Service fault: " + response.GetServiceFault().GetString();
      return false;
    }
    else
    {
      error_message = "Service fault";
      return false;
    }
  }

  public string GetMD5Hash(string input)
  {
    System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
    byte[] bs = Encoding.UTF8.GetBytes(input);
    bs = x.ComputeHash(bs);
    StringBuilder s = new StringBuilder();
    foreach (byte b in bs)
    {
      s.Append(b.ToString("x2").ToLower());
    }
    return s.ToString();
  }

}