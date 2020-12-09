using System.Threading;

namespace server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
			var token = new ManualResetEventSlim(false);
			var router = new Router {
				new Recipients.ProcessStarter(),
				new Recipients.ScreenMirror()
			};

			using var agent = new Agent(port: 11000);
			agent.Use(router);

			agent.ClientDisconnected += delegate {
				token.Set();
			};

			agent.Start();
			token.Wait();
			agent.Stop();
        }
    }
}