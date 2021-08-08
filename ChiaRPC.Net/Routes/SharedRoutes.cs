using System;

namespace ChiaRPC.Routes
{
    internal static class SharedRoutes
    {
        public static Uri GetConnections()
            => new Uri("get_connections", UriKind.Relative);

        public static Uri OpenConnection()
            => new Uri("open_connection", UriKind.Relative);

        public static Uri CloseConnection()
            => new Uri("close_connection", UriKind.Relative);
    }
}
