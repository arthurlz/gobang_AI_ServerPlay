Gobang game with Ai and support multiplayers to compete on server.
=============
---

### Prerequisite
   .Net Framework >= 4.5
   and Visual studio 2015 after

### What I implement this project.
* A gobang Ai who you can compete with.
* Multiplayers can play gobang by running the server.

### Structures
```gobang
    ├── gobang
    │   ├── App.config
    │   ├── ChessBoard.cs
    │   ├── Game.cs
    │   ├── GameSetting.cs
    │   ├── Globe.cs
    │   ├── Program.cs
    │   ├── Properties
    │   │   ├── AssemblyInfo.cs
    │   │   ├── Resources.Designer.cs
    │   │   ├── Resources.resx
    │   │   ├── Settings.Designer.cs
    │   │   └── Settings.settings
    │   ├── Resources
    │   │   ├── bg.png
    │   │   ├── black.png
    │   │   ├── favicon.ico
    │   │   ├── selected.png
    │   │   ├── set.png
    │   │   └── white.png
    │   ├── gameUI.Designer.cs
    │   ├── gameUI.cs
    │   ├── gameUI.resx
    │   └── gobang.csproj
    ├── gobang.sln
    └── gobang.v12.suo
```
1. The main Ai search algorithm (Minimax with alpha-beta prunning) is implemented in the ```Game.cs```, AI searchs the several next depths of the search tree for the relatively best position next to go,evaluating the current state is set to be equal to calculate a point of current chess state.I only search for more 3 depth, in that the performance deteriorate suddenly when it goes to 4 and bigger,but a 3 depth's AI is strong enough.


```├── GobangServer
│   ├── GobangServer
│   │   ├── App.config
│   │   ├── Form1.Designer.cs
│   │   ├── Form1.cs
│   │   ├── Form1.resx
│   │   ├── GobangServer.csproj
│   │   ├── Player.cs
│   │   ├── Program.cs
│   │   ├── Properties
│   │   │   ├── AssemblyInfo.cs
│   │   │   ├── Resources.Designer.cs
│   │   │   ├── Resources.resx
│   │   │   ├── Settings.Designer.cs
│   │   │   └── Settings.settings
│   │   └── Resources
│   │       ├── dc4d0ef41bd5ad6e2a7c9e0882cb39dbb7fd3c25.jpg
│   │       └── www.ico.la_3e01a126008ffb225c7173b721bf1eeb_48X48.ico
│   ├── GobangServer.sln
│   └── GobangServer.v12.suo
```

2. Multiplayers can paly against with running ```GobangServer``` project, and clicking someone on the online users list on the main page, you can send a invitation to he to paly with.The server is connected by TCP protocol with client and when one player click the specific position, he sends message to server,then server transfer what it gets to the one on the other peer.


OK.Build and go.




