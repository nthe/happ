using System.ComponentModel;
using server.Models;

namespace server.Recipients
{
    public class ProcessStarter : IRecipient
    {
		public const string Tag = "run-process";

        public string Resource => Tag;

        public void HandleDelivery(Envelope message)
        {
            var argsLen = string.Join(string.Empty, message.Data?.Args).Length;
            if (argsLen == 0)
            {
                message.SendResponse("Nothing to start");
                return;
            }
			try
			{
				var p = System.Diagnostics.Process.Start(message.Data.Args[0]);
				var response = $"Started {p.ProcessName}!";
				message.SendResponse(response);
			}
			catch (Win32Exception)
			{
				message.SendResponse("No such process to start...");
			}
        }

    }
}