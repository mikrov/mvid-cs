using System;
using System.Web;

public class cookieSessionStorage : MVID_SessionStorage
{
	public override Boolean save(string storage_name, string mv_session_id)
	{
		HttpCookie cookie = new HttpCookie(storage_name, mv_session_id);
		HttpContext.Current.Response.Cookies.Add(cookie);
		return true;
	}

	public override string load(string storage_name)
	{
		try
		{
			if (!string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[storage_name].Value.ToString()))
			{
				return HttpContext.Current.Request.Cookies[storage_name].Value.ToString();
			}
			return null;
		}
		catch
		{
			return null;
		}
	}
}