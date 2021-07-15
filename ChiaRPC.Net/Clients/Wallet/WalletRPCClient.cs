using ChiaRPC.Models;
using ChiaRPC.Results;
using ChiaRPC.Routes;
using ChiaRPC.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChiaRPC.Clients
{
    public sealed class WalletRPCClient : ChiaRPCClient
    {
        public WalletRPCClient(ChiaRPCOptions options, string apiUrl)
            : base(options, "wallet", apiUrl)
        {
        }

        /// <summary>
        /// Retrieves balances for a wallet.
        /// </summary>
        /// <returns></returns>
        public async Task<Wallet> GetWalletBalance(uint walletId)
        {
            var result = await PostAsync<GetWalletBalanceResult>(WalletRoutes.GetWalletBalance(), new Dictionary<string, string>()
            {
                ["wallet_id"] = $"{walletId}"
            });
            return result.Wallet;
        }

        /// <summary>
        /// Retrieves the address of the wallet at the given id for the current key.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWalletAddressAsync(uint walletId, bool generateAddress)
        {
            var result = await PostAsync<GetWalletAddressResult>(WalletRoutes.GetWalletAddress(), new Dictionary<string, string>()
            {
                ["wallet_id"] = $"{walletId}",
                ["new_address"] = $"{generateAddress}"
            });
            return result.Address;
        }

        /// <summary>
        /// Creates a wallet backup file at the given file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task CreateBackupFileAsync(string filePath)
        {
            await PostAsync<ChiaResult>(WalletRoutes.CreateBackupFile(), new Dictionary<string, string>()
            {
                ["file_path"] = filePath,
            });
        }

        /// <summary>
        /// Gets a list of WalletInfos for the currently logged in key.
        /// </summary>
        /// <returns></returns>
        public async Task<WalletInfo[]> GetWalletsAsync()
        {
            var result = await PostAsync<GetWalletsResult>(WalletRoutes.GetWallets());

            return result.Wallets;
        }

        /// <summary>
        /// Gets the sync status of the wallet.
        /// </summary>
        /// <returns></returns>
        public async Task<SyncStatus> GetSyncStatusAsync()
        {
            var result = await PostAsync<GetSyncStatusResult>(WalletRoutes.GetSyncStatus());

            return new SyncStatus(result.Synced, result.Syncing, result.GenesisInitialized);
        }

        /// <summary>
        /// Sends a standard transaction to a target puzzle_hash.
        /// </summary>
        /// <param name="walletId">The id of the wallet to take funds from</param>
        /// <param name="amount">The amount of mojo to send</param>
        /// <param name="fee">The amount of mojo used as fee</param>
        /// <param name="targetAddress">The address to send the funds to</param>
        /// <returns>A record representing the transaction. The transaction id is stored in the name property.</returns>
        public async Task<TransactionRecord> SendTransactionAsync(uint walletId, ulong amount, ulong fee, string targetAddress)
        {
            var result = await PostAsyncRaw<TransactionResult>(WalletRoutes.SendTransaction(), new Dictionary<string, object>()
            {
                ["wallet_id"] = $"{walletId}",
                ["amount"] = amount,
                ["fee"] = fee,
                ["address"] = targetAddress,
            });

            return result.Transaction;
        }

        /// <summary>
        /// Sends a standard transaction to a target puzzle_hash.
        /// </summary>
        /// <param name="walletId">The id of the wallet to take funds from</param>
        /// <param name="amount">The amount of mojo to send</param>
        /// <param name="fee">The amount of mojo used as fee</param>
        /// <param name="targetPuzzleHash">The puzzle hash to send the funds to</param>
        /// <returns></returns>
        public Task<TransactionRecord> SendTransactionAsync(uint walletId, ulong amount, ulong fee, HexBytes targetPuzzleHash)
        {
            var address = Bech32M.PuzzleHashToAddress(targetPuzzleHash);
            return SendTransactionAsync(walletId, amount, fee, address);
        }

        /// <summary>
        /// Gets a transaction record by transaction id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<TransactionRecord> GetTransactionAsync(HexBytes transactionId)
        {
            var result = await PostAsync<TransactionResult>(WalletRoutes.GetTransaction(), new Dictionary<string, string>()
            {
                ["transaction_id"] = transactionId.Hex,
            });

            return result.Transaction;
        }

        /// <summary>
        /// Gets transaction records for a walletId.
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<TransactionRecord[]> GetTransactionsAsync(uint walletId, uint start = 0, uint end = 50)
        {
            var result = await PostAsync<GetTransactionsResult>(WalletRoutes.GetTransactions(), new Dictionary<string, string>()
            {
                ["wallet_id"] = $"{walletId}",
                ["start"] = $"{start}",
                ["end"] = $"{end}",
            });

            return result.Transactions;
        }
    }
}
