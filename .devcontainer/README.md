# Dev Container

This folder contains configuration for running the project inside a **Dev Container** (VS Code Remote Containers or GitHub Codespaces).

## What this folder does

- Uses a .NET 10 base image to match `global.json`
- Installs Node.js 22 for the Vue/Vite frontend in `src/Web/ClientApp`
- Runs dependency restore on container creation:
  - `dotnet restore`
  - `npm install` in `src/Web/ClientApp`
- Runs a startup health summary each time the container starts via `.devcontainer/post-start-check.sh`
- Forwards common local development ports:
  - `5000` / `5001` (ASP.NET)
  - `5173` (Vite)
  - `18888` (Aspire dashboard)

## When to use it

If using VS Code:

1. Install the “Dev Containers” extension.
2. Open the repository.
3. Select **“Reopen in Container”**.

After the container is ready, run the app from the repository root:

```bash
dotnet run --project ./src/AppHost
```

To manually rerun the startup summary:

```bash
bash .devcontainer/post-start-check.sh
```

If using GitHub Codespaces:

- Codespaces will automatically use this configuration when the workspace starts.

## Notes

This folder has no effect on the runtime application. It is only used to configure the developer environment.
