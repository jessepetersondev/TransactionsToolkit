using System.Collections.Generic;
using System.Linq;
using NBitcoin;
using TransactionsToolkit.Models;
using TransactionsToolkit.Shared.Enums;

namespace TransactionsToolkit
{
    public static class TransactionHelper
    {
        /// <summary> Build & sign a simple P2PKH spend </summary>
        public static Transaction BuildAndSign(
            IEnumerable<Utxo> utxos,
            BitcoinAddress   dest,
            Key              privateKey,
            Money            fee)
        {
            var network = dest.Network;
            var txb = network.CreateTransactionBuilder();
            foreach (var u in utxos)
                txb.AddCoins(new Coin(u.Outpoint, new TxOut(u.Amount, u.ScriptPubKey)));
            txb.AddKeys(privateKey);

            var total = utxos.Sum(u => u.Amount);
            txb.Send(dest, total - fee);
            txb.SendFees(fee);

            // send change back to P2PKH of same key
            var change = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);
            txb.SetChange(change);

            return txb.BuildTransaction(true);
        }

        public static Transaction CreateOpReturnTransaction(
            IEnumerable<Utxo> utxos,
            byte[]            data,
            Key               privateKey,
            Money             fee,
            NetworkType       net = NetworkType.Mainnet)
        {
            // Pick the network
            var network = net == NetworkType.Mainnet ? Network.Main : Network.TestNet;

            // Build builder and disable dust removal
            var txb = network.CreateTransactionBuilder();
            txb.DustPrevention = false;                 // ← Allow OP_RETURN zero‑sat outputs

            // Add UTXOs and keys
            foreach (var u in utxos)
                txb.AddCoins(new Coin(u.Outpoint, new TxOut(u.Amount, u.ScriptPubKey)));
            txb.AddKeys(privateKey);

            // Add the OP_RETURN output
            var script = TxNullDataTemplate.Instance.GenerateScriptPubKey(data);
            txb.Send(script, Money.Zero);

            // Specify fee and change
            txb.SendFees(fee);
            var change = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);
            txb.SetChange(change);

            // Build & sign
            return txb.BuildTransaction(true);
        }


        /// <summary> Calculate dust threshold for a scriptPubKey at a given fee rate </summary>
        public static Money GetDustThreshold(Script scriptPubKey, FeeRate feeRate = null)
        {
            feeRate ??= new FeeRate(Money.Satoshis(1));
            // Use a zero-value TxOut just to calculate dust
            return new TxOut(Money.Zero, scriptPubKey).GetDustThreshold(feeRate);
        }
    }
}
