using ChiaRPC.Models;
using ChiaRPC.Results;
using ChiaRPC.Routes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChiaRPC.Clients
{
    public sealed class NodeRPCClient : ChiaRPCClient, INodeRPCClient
    {
        public NodeRPCClient(ChiaRPCOptions options, string apiUrl)
            : base(options, "full_node", apiUrl)
        {
        }

        public async Task<BlockchainState> GetBlockchainStateAsync()
        {
            var result = await PostAsync<GetBlockchainStateResult>(FullNodeRoutes.GetBlockchainState());
            return result.BlockchainState;
        }

        public async Task<FullBlock> GetBlockAsync(HexBytes headerHash)
        {
            var result = await PostAsync<GetBlockResult>(FullNodeRoutes.GetBlock(), new Dictionary<string, string>()
            {
                ["header_hash"] = headerHash.Hex,
            });
            return result.Block;
        }

        public async Task<FullBlock[]> GetBlocksAsync(uint startHeight, uint endHeight)
        {
            var result = await PostAsync<GetBlocksResult>(FullNodeRoutes.GetBlocks(), new Dictionary<string, string>()
            {
                ["start"] = $"{startHeight}",
                ["end"] = $"{endHeight}",
            });
            return result.Blocks;
        }

        public async Task<RecentEndOfSubSlotBundle> GetRecentEndOfSubSlotBundleAsync(HexBytes challengeHash)
        {
            var result = await PostAsync<GetRecentEosResult>(FullNodeRoutes.GetRecentSignagePointOrEos(), new Dictionary<string, string>()
            {
                ["challenge_hash"] = challengeHash.Hex
            });
            return new RecentEndOfSubSlotBundle(result.EndOfSubSlotBundle, result.ReceivedAt, result.Reverted);
        }

        public async Task<RecentSignagePoint> GetRecentSignagePoint(HexBytes signagePointHash)
        {
            var result = await PostAsync<GetRecentSignagePointResult>(FullNodeRoutes.GetRecentSignagePointOrEos(), new Dictionary<string, string>()
            {
                ["sp_hash"] = signagePointHash.Hex
            });
            return new RecentSignagePoint(result.SignagePoint, result.ReceivedAt, result.Reverted);
        }

        public async Task<CoinRecord> GetCoinRecordByNameAsync(HexBytes name)
        {
            var result = await PostAsync<GetCoinRecordByNameResult>(FullNodeRoutes.GetCoinRecordByName(), new Dictionary<string, string>()
            {
                ["name"] = $"{name}"
            }, false);

            return result.CoinRecord;
        }
        public async Task<CoinSolution> GetPuzzleAndSolution(HexBytes coinId, ulong height)
        {
            var result = await PostAsync<GetPuzzleAndSolutionResult>(FullNodeRoutes.GetPuzzleAndSolution(), new Dictionary<string, string>()
            {
                ["coin_id"] = $"{coinId}",
                ["height"] = $"{height}",
            });
            return result.CoinSolution;
        }

        public Task<CoinSolution> GetPuzzleAndSolution(CoinRecord coinRecord)
            => GetPuzzleAndSolution(coinRecord.Name(), coinRecord.SpentBlockIndex);


    }
}
