# Weather App

A full-stack weather application built with:

- **Backend:** ASP.NET Core Web API (C#)
- **Frontend:** React + Vite
- **Data Source:** National Weather Service API (api.weather.gov)

---

# 📁 Single command to run the webapp

⚠️ Always run commands from:

    weather-webapp-main (home-directory)

# 👨‍💻 Quick Start

    cd weather-webapp-main (or cd <home-directory>)
    npm run setup
    npm run dev

    Open:

    http://localhost:5173

---

# 📁 Project Structure

All commands must be executed from the project root:

    weather-webapp-main/
    │
    ├── backend/
    │   └── NwsWeather.Api/
    │       ├── Clients/
    │       ├── Controllers/
    │       ├── Dtos/
    │       ├── Parsers/
    │       ├── Services/
    │       ├── Properties/
    │       │   └── launchSettings.json
    │       ├── Program.cs
    │       └── NwsWeather.Api.csproj
    │
    ├── frontend/
    │   ├── public/
    │   ├── src/
    │   ├── index.html
    │   ├── vite.config.js
    │   └── package.json
    │
    ├── package.json   ← Root runner (runs both backend + frontend)
    └── README.md

---

# 🧰 System Requirements

## 1) Node.js (LTS recommended)

Verify:

    node -v
    npm -v

Download from: https://nodejs.org

## 2) .NET SDK

Verify:

    dotnet --version

Download from: https://dotnet.microsoft.com/download

---

# 📦 One-Time Setup (Install Dependencies)

From the project root:

    cd weather-webapp-main (or cd <home-directory>) 

Run:

    npm install
    npm install --prefix frontend
    dotnet restore backend/NwsWeather.Api

---

# 🚀 Running the Application (Single Command)

From the project root:

    npm run dev

This starts both backend and frontend.

Stop with:

    Ctrl + C

---

# 🌐 Local URLs

Frontend:

    http://localhost:5173

Backend:

    http://localhost:5266

Swagger:

    http://localhost:5266/swagger

---

# 🧪 Troubleshooting

## npm: command not found

Install Node.js.

## dotnet: command not found

Install .NET SDK.

## Proxy errors / ECONNREFUSED

Ensure backend is running and ports match.

## macOS Rollup Security Block

Run:

    rm -rf frontend/node_modules frontend/package-lock.json
    npm install --prefix frontend

Then allow blocked item in: System Settings → Privacy & Security

---

Runs entirely on localhost. No deployment required.
