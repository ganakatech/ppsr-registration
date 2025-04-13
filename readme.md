# PPSR Vehicle Batch Registration

## Prerequisites
- Docker
- Node.js + npm (for local dev)
- .NET 9 SDK (for local dev)

## Running the App
```bash
docker compose up --build
```

- Backend: [http://localhost:5000/swagger](http://localhost:5000/swagger/index.html)
- Frontend: [http://localhost:3000](http://localhost:3000)

## Input files used for testing
```bash
cd Inputfiles
Sample-data.csv - File with 3 valid records and 1 invalid record
Sample data.csv - File with 4 invalid records
```

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
### Frontend unit tests
```bash
cd frontend
npm test
```