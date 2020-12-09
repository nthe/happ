using System.Collections.ObjectModel;
using server.Models;

namespace server
{
    public class Router : Collection<IRecipient>
    {
        public void Route(Envelope envelope)
        {
            foreach (var recp in this.Items)
                if (recp.Resource.Equals(envelope.Data.Resource))
                    recp.HandleDelivery(envelope);
        }
    }
}