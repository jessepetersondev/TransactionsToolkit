using NBitcoin;

namespace TransactionsToolkit.Models
{
    public class Utxo
    {
        public OutPoint  Outpoint     { get; init; }
        public Money     Amount       { get; init; }
        public Script    ScriptPubKey { get; init; }
    }
}
