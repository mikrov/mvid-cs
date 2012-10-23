<%@ Page Language="C#"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>

  <head>
    <title>MV-ID Unauthenticated</title>
    <meta charset="utf-8" />
  </head>

  <body>
    <p>Error: <%= Request.QueryString["error_message"]%></p>
</body>

</html>