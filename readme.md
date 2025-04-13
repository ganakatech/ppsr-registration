# PPSR Vehicle Batch Registration

## Prerequisites
- Docker
- Node.js + npm (for local dev)
- .NET 8 SDK (for local dev)

## Running the App
```bash
docker-compose up --build
```

- Backend: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- Frontend: [http://localhost:3000](http://localhost:3000)

## Local Development
### Backend
```bash
cd backend
 dotnet run
```
### Frontend
```bash
cd frontend
npm install
npm run dev
```