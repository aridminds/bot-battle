# Bot-Battle Rust Agent with Extism Rust PDK

Source of documentation: https://github.com/extism/rust-pdk

This library can be used to write [Extism Plug-ins](https://extism.org/docs/concepts/plug-in) in Rust.

## Install

Generate a `lib` project with Cargo:

```bash
cargo new --lib my-agent
```

Add the library from [crates.io](https://crates.io/crates/extism-pdk).

```bash
cargo add extism-pdk
```

Change your `Cargo.toml` to set the crate-type to `cdylib` (this instructs the compiler to produce a dynamic library, which for our target will be a Wasm binary):

```toml
[lib]
crate-type = ["cdylib"]
```

### Rustup and wasm32-unknown-unknown installation

Our example below will use the `wasm32-unknown-unknown` target. If this is not installed you will need to do so before this example will build. The easiest way to do this is use [`rustup`](https://rustup.rs/).

```bash
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh
```

Once `rustup` is installed, add the `wasm32-unknown-unknown` target:

```bash
rustup target add wasm32-unknown-unknown
```


## Getting Started

The goal of writing an [Extism plug-in](https://extism.org/docs/concepts/plug-in) is to compile your Rust code to a Wasm module with exported functions that the host application can invoke. The first thing you should understand is creating an export. Let's write a simple program that calculate a tank action a `calculate_action` function which will take a AgentRequest as a struct and return a json-string. For this, we use the `#[plugin_fn]` macro on our exported function:


```rust
use extism_pdk::*;

use crate::{rotate_response::{AgentRotateResponse, Rotate}};
use agent_request::Direction;


#[plugin_fn]
pub fn calculate_action(arena: AgentRequest) -> FnResult<String> {
  //Rotate  
  let action = Rotate {
      direction: Direction::North
  };
  let response = AgentRotateResponse { action };
  Ok(serde_json::to_string(&response).unwrap())
}
```

Since we don't need any system access for this, we can compile this to the lightweight `wasm32-unknown-unknown` target instead of using the `wasm32-wasi` target:

```bash
cargo build --target wasm32-unknown-unknown
```

> **Note**: You can also put a default target in `.cargo/config.toml`:
```toml
[build]
target = "wasm32-unknown-unknown"
```
