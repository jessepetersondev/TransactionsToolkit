# TransactionsToolkit

A .NET 8 library for building/signing Bitcoin transactions, including OP_RETURN outputs and dust calculation.

## Features

- **BuildAndSign**: simple P2PKH spending
- **CreateOpReturnTransaction**: embed data via OP_RETURN
- **GetDustThreshold**: compute dust limits per scriptPubKey

## Install

```bash
dotnet add package TransactionsToolkit --version 1.0.0
