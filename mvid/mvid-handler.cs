using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Resources;
using System.Configuration;
using System.Net;
using System.Collections.Specialized;

public class mvid_handler
{
  public string mv_session_id;

  public mvid_handler()
  {
    mvid_challenge mvc = new mvid_challenge();
    mv_session_id = "";
    string error_message = "";
    string mvid_app_domain = mvidconfig.challenge["domain"].ToString();
    string mvid_shared_key = mvidconfig.challenge["shared_key"].ToString();
    string mvid_redirect_on_success = mvidconfig.challenge["redirect_on_success"].ToString();
    string mvid_redirect_on_failure = mvidconfig.challenge["redirect_on_failure"].ToString();
    string mvid_storage_type = mvidconfig.storage["type"].ToString();
    string mvid_storage_name = mvidconfig.storage["name"].ToString();

    // Not sure what storage_class is!
    MVID_SessionStorage storage_class = null;
    NameValueCollection nvc = HttpContext.Current.Request.Form;

    if(mvid_storage_type != null)
    {
      try
      {
        Type storage_class_type = Type.GetType(mvid_storage_type + "SessionStorage");
        storage_class = (MVID_SessionStorage) Activator.CreateInstance(storage_class_type);
      }
      catch
      {
        error_message = "The storage type: " + mvid_storage_type + " is not available.";
      }
    }

    if (!string.IsNullOrEmpty(nvc["mv_session_hash"]))
    {
      // A session hash was recieved from MV-ID either after a doLogin() or a doSSO()
      Boolean success = false;

      if (storage_class != null) 
      {
        // Register the session hash for application usage:
        success = mvc.registerSessionUsage(nvc["mv_session_hash"], mvid_app_domain, mvid_shared_key, ref mv_session_id, ref error_message);
      }

      if (success)
      {
        storage_class.save(mvid_storage_name, mv_session_id);

        if (mvid_redirect_on_success != null) 
        {
          HttpContext.Current.Response.Redirect(mvid_redirect_on_success);
        }
      }
      else
      {
        if (mvid_redirect_on_failure != null)
        {
          HttpContext.Current.Response.Redirect(mvid_redirect_on_failure + "?error_message=" + HttpUtility.UrlEncode(error_message));
        }
        else
        {
          HttpContext.Current.Response.Write("Error: " + error_message);
          Environment.Exit(-1);
        }
      }
    }
    else
    {
      if (storage_class == null)
      {
		  if (mvid_redirect_on_failure != null)
		  {
			  HttpContext.Current.Response.Redirect(mvid_redirect_on_failure + "?error_message=" + HttpUtility.UrlEncode(error_message));
		  }
		  else
		  {
			  HttpContext.Current.Response.Write("Error: " + error_message);
			  Environment.Exit(-1);
		  }
      }
      else
      {
        mv_session_id = storage_class.load(mvid_storage_name);
      }
    }
  }
}