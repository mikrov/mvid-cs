<%@ Page Language="C#"%>
<%
  mvid_handler mvh = new mvid_handler();
  string mv_session_id = mvh.mv_session_id;
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>

  <head>
    <title>MV-ID Unauthenticated</title>
    <meta charset="utf-8" />
    <script src="https://signon.mv-nordic.com/sp-js/mvlogin.js" type="text/javascript"></script>
	<script type="text/javascript">
      $(function() {
        var sso_args = {
        mv_session_id: "<%=mv_session_id%>"
        }
        doSSO(sso_args,function(res) {
          if (res.app_auth_ok) {
          /* SSO Success:
             User has been authenticated and the application has been registered for session usage
             In this example the user is redirected to the location entered in mvid-config.cs.
             If you are already in the "redirect_on_success" page 
          */
            document.location = 'start.aspx';
          }
          else if (!res.mvid_auth_ok) {
            /* SSO Failed:
               User has not been authenticated or the application did not register the session
               correctly. In this example we will place a login button for the user to press.
               Alternatively you can just want to call doLogin() right away as shown in the code comment
            */
               $("#mvid").html("<button onclick='doLogin()'>Login</button>");
          }
        });
      });
    </script>
  </head>
  <body>
    <div id="mvid"></div>
  </body>
</html>