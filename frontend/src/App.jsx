import React from 'react';
import FileUploader from './components/FileUploader';
import './App.css';

function App()
{
  return (
    <div className="container">
      <header className="app-header">
        <h1>PPSR Registration</h1>
        <p>Upload and process your vehicle registration data efficiently.</p>
      </header>
      <main>
        <FileUploader />
      </main>
      <footer className="app-footer">
        <p>&copy; 2025 PPSR Registration. All rights reserved.</p>
      </footer>
    </div>
  );
}

export default App;