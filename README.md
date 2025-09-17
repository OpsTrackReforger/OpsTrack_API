🚀 Publish Docker image to GitHub Container Registry (GHCR)
1. Log in to GHCR
Første gang (eller når dit token udløber), log ind i GHCR med dit GitHub‑brugernavn og dit Personal Access Token (PAT):
docker login ghcr.io -u <GITHUB_USERNAME>
👉 Brug dit classic PAT med scope write:packages som password.

2. Build the image
Fra roden af repoet, byg dit image ud fra Dockerfile:
docker build -t opstrackapi .

3. Tag image med korrekt namespace
Tag det image, så det matcher dit org/repo i GHCR:
docker tag opstrackapi ghcr.io/opstrackreforger/opstrack_api:latest

Tip: Brug versions‑tags i stedet for latest når du releaser, fx v1.0.0.

4. Push til GHCR
Skub billedet op til GHCR:
docker push ghcr.io/opstrackreforger/opstrack_api:latest

5. Verify
Efter push kan du se pakken under Packages i GitHub‑org’en:
https://github.com/orgs/opstrackreforger/packages

Eksempel med versionstag:
docker build -t opstrackapi .
docker tag opstrackapi ghcr.io/opstrackreforger/opstrack_api:v1.0.0
docker push ghcr.io/opstrackreforger/opstrack_api:v1.0.0

