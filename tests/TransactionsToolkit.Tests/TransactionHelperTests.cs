using Xunit;
using NBitcoin;
using TransactionsToolkit;
using TransactionsToolkit.Models;
using TransactionsToolkit.Shared.Enums;

namespace TransactionsToolkit.Tests
{
    public class TransactionHelperTests
    {
        [Fact]
        public void GetDustThreshold_ReturnsPositive()
        {
            var pk      = new Key().PubKey;
            var script  = pk.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ScriptPubKey;
            var dust    = TransactionHelper.GetDustThreshold(script, new FeeRate(Money.Satoshis(1)));
            Assert.True(dust > Money.Zero);
        }

        [Fact]
        public void CreateOpReturnTransaction_HasOpReturnOutput()
        {
            var key      = new Key();
            var pub      = key.PubKey;
            var addr     = pub.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet);
            var utxo     = new Utxo { Outpoint = new OutPoint(uint256.One,0), Amount=Money.Coins(0.001m), ScriptPubKey=addr.ScriptPubKey };
            var data     = System.Text.Encoding.ASCII.GetBytes("hi");
            var tx       = TransactionHelper.CreateOpReturnTransaction(new[]{utxo}, data, key, Money.Satoshis(1000), NetworkType.Testnet);

            // find an OP_RETURN output
            Assert.Contains(tx.Outputs, o => 
                o.ScriptPubKey.ToOps().First().Code == OpcodeType.OP_RETURN);
        }
    }
}
