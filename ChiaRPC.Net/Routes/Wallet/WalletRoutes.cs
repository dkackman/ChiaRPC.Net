using System;

namespace ChiaRPC.Routes
{
    internal static class WalletRoutes
    {
        public static Uri GetWalletBalance()
            => new Uri("get_wallet_balance", UriKind.Relative);

        public static Uri GetWalletAddress()
            => new Uri("get_next_address", UriKind.Relative);

        public static Uri CreateBackupFile()
            => new Uri("create_backup", UriKind.Relative);

        public static Uri GetWallets()
            => new Uri("get_wallets", UriKind.Relative);

        public static Uri GetSyncStatus()
            => new Uri("get_sync_status", UriKind.Relative);

        public static Uri SendTransaction()
            => new Uri("send_transaction", UriKind.Relative);

        public static Uri SendTransactionMulti()
            => new Uri("send_transaction_multi", UriKind.Relative);

        public static Uri GetTransaction()
            => new Uri("get_transaction", UriKind.Relative);

        public static Uri GetTransactions()
            => new Uri("get_transactions", UriKind.Relative);
    }
}
