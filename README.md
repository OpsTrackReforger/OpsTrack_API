**Support**
Join the discord: https://discord.gg/w9Gf3twcUn
Or create an issue


**Description**
OpsTrack_API is a WebAPI including SqLite database that can receive, store and deliver game data from the OpsTrack_Reforger mod.
This is created to be the "middleman" between ArmA: Reforger and other applications that may need information from a game server.

**Features**
API for server events, Player join/leave etc.
Documented through Swagger
Simple APIKEY set through enviroment variable on startup.
SQLite support
Docker ready with persistent volume for database.

**Get Started**
Requirements: .NET 8 SKD, Docker, Git

**Run locally:**
`git clone https://github.com/OpsTrackReforger/OpsTrack_API`
`cd Opstrack_API\OpsTrack_API`
`dotnet run`

**Run through Docker (Recommended):**

`ExecStartPre=-/usr/bin/docker pull ghcr.io/opstrackreforger/opstrack_api:latest`

`ExecStartPre=-/usr/bin/docker rm -f opstrackapi`

`ExecStart=/usr/bin/docker run --name opstrackapi -p 8080:8080 -e ApiKey="Your_Secret_Key" -v /opt/opstrackapi/data:/app/D>ExecStop=/usr/bin/docker stop -t 10 opstrackapi`

**Configuration**
ApiKey - Environment variable - Must be set (-e ApiKey="....")


**CI/CD**
Github actions builds docker image and pushes to registry when pushes are made to master.
