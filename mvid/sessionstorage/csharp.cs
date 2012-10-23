using System;
using System.Web;

public class csharpSessionStorage : MVID_SessionStorage
{
    public override Boolean save(string storage_name, string mv_session_id)
    {
        HttpContext.Current.Session[storage_name] = mv_session_id;
        return true;
    }

    public override string load(string storage_name) 
    {
		try
		{
			if (HttpContext.Current.Session[storage_name].ToString() != "")
			{
				return HttpContext.Current.Session[storage_name].ToString();
			}
			return null;
		}
		catch
		{
			return null;
		}
    }
}