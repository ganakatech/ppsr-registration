# PPSR Vehicle Batch Registration

## Prerequisites
- Docker
- Node.js + npm (for local dev)
- .NET 8 SDK (for local dev)

## Running the App
```bash
docker compose up --build
```

- Backend: [http://localhost:5000/swagger](http://localhost:5000/swagger/index.html)
- Frontend: [http://localhost:3000](http://localhost:3000)

## Local Development
### Backend
```bash
cd backend && cd ppsr-registration
 dotnet run
```
### Backend unit tests
```bash
cd backend && cd ppsr-registration.Tests
 dotnet test
```
### Frontend
```bash
cd frontend
npm install
npm run dev
```
### Frontend
```bash
cd frontend
npm test
```