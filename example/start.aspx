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
      $(function () {
        doKeepAlive({
          mv_session_id: "<%=mv_session_id%>",
          on_session_lost: "index.aspx"
        },
        function (is_session_alive) {
          // Manually handle lost session by testing on is_session_alive
          // on_session_lost argument used above should be all yuo need
          // in most cases.
        });
      });
    </script>
  </head>

  <body>
    <p>Logged in with mv_session_id : <%=mv_session_id%></p>
  </body>

</html>