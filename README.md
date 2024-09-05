
# Bot Battle

**Bot Battle** is an exciting multiplayer project where you can program your own tank and compete against other tanks. The core of this project is that everyone can define their tank's logic themselves – using WebAssembly. The tanks can move around the map, shoot, avoid obstacles, and collect items.

The backend is built using **C# .NET 8** and utilizes **Garnet** for the server infrastructure. The frontend is implemented with **Svelte**, a modern web framework for fast and performant web applications.

In the **Samples** folder, you'll also find example agents implemented in **Rust** and **C#**. These agents demonstrate how to develop your tank's logic with WebAssembly.

## 🚀 Features

- **Customizable Tank Control**: Program the logic of your tank! Decide when it turns, moves, shoots, or how it reacts to its environment.
- **Environment Interaction**: Your tank can respond to various things on the map, such as obstacles, items, or enemy tanks.
- **WebAssembly Extism Integration**: Use WebAssembly to run your tank's logic in a performant environment. Each tank is controlled by its own WebAssembly instance. https://extism.org/
- **Backend with C# and Garnet**: The backend is built with C# .NET 8 and uses Garnet to provide smooth gameplay and fast response times.
- **Frontend with Svelte**: The game's user interface is built using Svelte, allowing for fast and efficient interactions.
- **Sample Agents**: In the `samples` folder, you'll find example implementations of a Rust and C# agent to help you get started with developing WebAssembly-based tank control.
- **Multiplayer Support**: Compete against other players to see whose tank logic works best.

## 🛠 Installation

### Frontend Setup

The frontend is built with **Svelte**. To run it locally, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/bot-battle.git
    cd bot-battle/frontend
    ```

2. Install the necessary dependencies:
    ```bash
    npm install
    ```

3. Start the local server:
    ```bash
    npm run dev
    ```

4. Open your browser and go to `http://localhost:3000` to start the game.

### Backend Setup

The backend uses **C# .NET 8** and **Garnet**. To run it locally, follow these steps:

1. Navigate to the backend folder:
    ```bash
    cd ../backend
    ```

2. Ensure you have .NET 8 installed, then run the project:
    ```bash
    dotnet run
    ```

3. The backend server will now run on `http://localhost:5000`.

## 🔧 Requirements

- **Node.js** (version 14 or higher)
- **npm** (or yarn)
- **.NET 8 SDK**
- A browser that supports WebAssembly (e.g., Chrome, Firefox)

## 🕹 How It Works

1. **Create Your Tank**: Each player can program their own tank by providing a WebAssembly module that controls the tank's behavior.
   
2. **React to the Environment**: Your tank can respond to events like incoming bullets, obstacles (such as trees or walls), and other tanks. Use this information to adapt your tank's logic.

3. **Strategy and Combat**: Test your logic on the map! Use items, avoid obstacles, and eliminate other tanks to win.

## 💻 WebAssembly Integration

The core logic of your tank is controlled by WebAssembly. Here are the basic steps to program your own tank:

1. **Create a WebAssembly Module**: Write your code in a language like Rust, C, C++, or AssemblyScript and compile it to WebAssembly.

2. **Interact with the Map**: Your tank has access to information about its environment, such as the position of obstacles or the presence of items.

3. **Control Functions**: Your WebAssembly module should include functions that define how the tank moves, turns, and shoots.

In the **Samples** folder, you'll find example agents in **Rust** and **C#**, which can serve as a starting point for developing your tank's control logic.

Example (Rust):
```rust
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

## 🧑‍💻 Contributing

We welcome contributions from other developers! Whether you want to add new features, fix bugs, or improve the project, you're welcome to participate.

### Steps to Contribute:

1. Fork this repository
2. Create a new branch (`git checkout -b feature/new-feature`)
3. Implement your changes
4. Submit a pull request

## ⚙️ To-Do

- [ ] Add more maps
- [ ] Mobile optimization
- [ ] Player leaderboards
- [ ] More items and obstacles

## 📄 License

This project is licensed under the [MIT License](LICENSE).
