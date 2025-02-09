```markdown
# PlanDeck

PlanDeck is a collaborative Planning Poker application built with **.NET 9 Blazor**, **FluentUI Blazor**, **gRPC**, and **MSSQL**. It enables teams to estimate tasks or user stories in a Scrum environment quickly and effectively. With real-time updates, configurable voting systems, and intuitive UI, PlanDeck streamlines sprint planning and fosters transparent collaboration.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Technologies](#technologies)
- [Contributing](#contributing)
- [License](#license)

---

## Features

1. **Room Creation & Configuration**  
   - Create a new planning room with a custom name and voting system (Fibonacci, Modified Fibonacci, T-Shirt, Powers of 2).  
   - Configure who can reveal cards, manage issues, auto-reveal, and display average scores.

2. **User Invitation**  
   - Share a unique room URL or send invites directly from PlanDeck.  
   - Participants can join with a chosen username or as observers.

3. **Issue Management**  
   - Add, edit, and remove issues or user stories to be estimated.  
   - Import issues from CSV or directly from Azure DevOps sprint backlogs.

4. **Real-Time Voting**  
   - Select a card from the chosen deck (Fibonacci, T-Shirt, etc.) to vote on the active issue.  
   - Automatic or manual card reveal based on room settings.  
   - After revealing, participants can still edit their votes if permitted.

5. **Ongoing Visibility**  
   - See who has voted in real-time (cards remain hidden until revealed).  
   - Track the active issue and overall progress at a glance.

6. **New Voting Round**  
   - Start a new voting round quickly.  
   - Optionally move on to the next issue in the list automatically.

7. **Responsive Interface**  
   - Built with FluentUI Blazor for a modern, user-friendly, and consistent experience on various devices.

---

## Getting Started

### Prerequisites
- **.NET 9 SDK** (or higher)
- **MSSQL** database instance
- A compatible IDE or text editor (e.g., Visual Studio, Visual Studio Code, Rider)

### Installation & Setup
1. **Clone the repository**:
   ```bash
   git clone https://github.com/YourUsername/PlanDeck.git
   ```
2. **Navigate to the project folder**:
   ```bash
   cd PlanDeck
   ```
3. **Set up the database**:
   - Update the connection string in the appsettings (e.g., `appsettings.json`) to point to your MSSQL instance.
   - Run any EF Core migrations if included (e.g., `dotnet ef database update`).

4. **Restore dependencies & build**:
   ```bash
   dotnet restore
   dotnet build
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```
6. **Access the app**:  
   Open your browser at `https://localhost:5001` (or the URL/port displayed in your console).

---

## Usage

1. **Create a new planning room**  
   - Provide a room name and select a voting system.  
   - Configure additional options (card reveal permissions, auto-reveal, etc.).

2. **Invite participants**  
   - Share the room URL or use the “Invite Users” feature.  
   - Participants join with a name or as observers.

3. **Manage issues**  
   - Add issues manually or import them.  
   - Activate an issue to start the voting process.

4. **Vote**  
   - Choose a card from the deck to vote on the active issue.  
   - Your card remains hidden until all participants have voted or the reveal is triggered.

5. **Review and Discuss**  
   - Once cards are revealed, see the distribution and optionally the average value if enabled.  
   - Participants can re-vote if necessary (based on permissions).

6. **Move to Next Issue**  
   - Start a new round and continue until all issues are estimated.

---

## Technologies

- **.NET 9 Blazor** – for building the client-side web application in C#.
- **FluentUI Blazor** – for a modern and consistent UI.  
- **gRPC** – for efficient client-server communication.
- **MSSQL** – for data persistence and storage.

---

## Contributing

Contributions are welcome! If you would like to contribute:

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/new-awesome-feature`.
3. Commit your changes: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature/new-awesome-feature`.
5. Create a new Pull Request.

---

## License

This project is licensed under the [MIT License](LICENSE). You are free to use, modify, and distribute this software in accordance with the license terms.

---

Thank you for checking out **PlanDeck**! If you have any questions or suggestions, feel free to open an issue or submit a pull request.
```
