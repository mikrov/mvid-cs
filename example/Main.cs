using System;
using Mvid;
	
namespace MvidExample {
	class MainClass {
		public static void Main (string[] args) {
			// Create 2 strings for refernce variables
			string mv_session_id = "";
			string error_message = "";
			// Register the session to your applicaiton:
			bool success = Mvid.Challenge.registerSessionUsage(
				"<mv-session-hash>",                // (in)  mv_session_hash which is posted back to the application from MV-ID
				"<application-domain>",             // (in)  The domain registered with MV-ID
				"<shared-key>",                     // (in)  The trusted key shared between the application server(s) and MV-ID
				ref mv_session_id,                  // (out) mv_session_id
				ref error_message);                 // (out) Error message

			if (success) {
				// Successful session registration
				Console.WriteLine(mv_session_id);
			}
			else {
				// Session registration failure
				Console.WriteLine(error_message);
			}

			System.ConsoleKeyInfo KInfo = Console.ReadKey(true);
		}
	}
}
