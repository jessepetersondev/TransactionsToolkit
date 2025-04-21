using System;
using NBitcoin;
using TransactionsToolkit;
using TransactionsToolkit.Models;
using TransactionsToolkit.Shared.Enums;

Console.WriteLine("=== TransactionsToolkit Demo ===");

var key  = new Key();
var pub  = key.PubKey;
var addr = pub.GetAddress(ScriptPubKeyType.Legacy, Network.TestNet);
var utxo = new Utxo {
    Outpoint     = new OutPoint(uint256.One,0),
    Amount       = Money.Coins(0.001m),
    ScriptPubKey = addr.ScriptPubKey
};

// OP_RETURN demo
var tx = TransactionHelper.CreateOpReturnTransaction(
    new[]{utxo},
    System.Text.Encoding.UTF8.GetBytes("👋"),
    key,
    Money.Satoshis(1500),
    NetworkType.Testnet
);

Console.WriteLine("Raw TX:");
Console.WriteLine(tx.ToHex());
