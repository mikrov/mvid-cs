mvid-cs
========

This is the official MV-ID Integration library.

MVID Library
------------
The actual integration library is contained in the "mvid" folder. The methode you need to include in your ASPX project is
"mvid_handler()":

  <%
    mvid_handler mvh = new mvid_handler();
    string mv_session_id = mvh.mv_session_id;
  %>

Installation
------------
1. Copy the "mvid" folder into the "App_code" folder of your project.
2. Edit the "mvid/config/mvid-config.cs" file, in particular you need to edit the "shared_key" and "domain" values
3. Include references "Jayrock.dll, Jayrock.Json.dll and JsonWsp.dll"

Example
-------
This is a fully working example. It requires a registered application domain and a shared-key. To try out this example:

1. Copy the index.aspx, start.aspx and error.aspx files to your webserver.
2. Make the site accessable via the application domain name you have registered with MV-ID.
3. Edit the "mvid-config.cs" file to reflect the domain you are using.