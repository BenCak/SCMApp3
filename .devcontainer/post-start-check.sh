#!/usr/bin/env bash

set -euo pipefail

echo ""
echo "SCMApp3 devcontainer health summary"
echo "================================="
echo "Workspace: $(pwd)"
echo "Date: $(date -Iseconds)"
echo ""

echo "Tool versions"
dotnet --version
node --version
npm --version

echo ""
echo "Project quick checks"
if [[ -f "global.json" ]]; then
  echo "- global.json found"
else
  echo "- global.json missing"
fi

if [[ -f "src/Web/ClientApp/package.json" ]]; then
  echo "- frontend package.json found"
else
  echo "- frontend package.json missing"
fi

echo ""
echo "Devcontainer startup check complete"
