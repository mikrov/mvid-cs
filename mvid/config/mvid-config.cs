using System;
using System.Collections;

class mvidconfig
{
	static private Hashtable mvid_config = new Hashtable();
	static private Hashtable mvid = new Hashtable();
	static private Hashtable challenge = new Hashtable();
	static private Hashtable storage = new Hashtable();
	static private Hashtable memcache = new Hashtable();

	static mvidconfig(){
		mvid_config.Add("mvid", mvid);
		mvid.Add("challenge", challenge);
		mvid.Add("storage", storage);

		/* Type in the shared key you recieved from MV-ID after you registered your application domain. If you havn't
		registered your application domain yet there is a form you can fill in at:
		https://signon.mv-nordic.com/wiki/app-domain-reg */
		challenge.Add("shared_key" , "<shared_key>");

		/* The domain you wish to register the user sessions with. The base domain of this value must match the
		application domain you have registered with MV-ID */
		challenge.Add("domain", "<application-domain>");

		challenge.Add("redirect_on_success" , "start.aspx");
		challenge.Add("redirect_on_failure" , "error.aspx");

		/* The identifier name to store the mv_session_id with.

		Default: "mv_session_id" */
		storage.Add("name" , "mv_session_id");

		/* Choose how to store the mv_session_id:
		1. "cookie"   - If you plan to develope an application that communicates with MV-ID's webservices via javascript
		you must store the mv_session_id as a session cookie on the client's browser.

		2. "csharp"   - On the other hand, if you are going to do all communications to MV-ID's webservices from the
		serverside, you might as well store the mv_session_ids as a C# session variable.

		3.- If you want to store your mv_session_id a completely different way you can extend the with a new
		session storage class by adding it in the directory sessionstorage.

		Default: "cookie" */
		storage.Add("type" , "cookie");
	}

	static public string read_config(Array path, string default_val)
	{
		Hashtable current_crumb = mvid_config;
		foreach (string v in path)
		{
			if (current_crumb[v] != null)
			{
				if (current_crumb[v].GetType() == typeof(Hashtable))
				{
					current_crumb = (Hashtable)current_crumb[v];
				}
				else
				{
					return current_crumb[v].ToString();
				}
			}
			else
			{
				if (default_val != null)
				{
					return default_val;
				}
				return null;
			}
		}
		return current_crumb.Values.ToString();
	}
}