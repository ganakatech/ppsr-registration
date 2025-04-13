import React, { useState } from 'react';
import axios from 'axios';

export default function FileUploader()
{
  const [summary, setSummary] = useState(null);
  const [error, setError] = useState(null);

  const handleUpload = async (e) =>
  {
    const file = e.target.files[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    try
    {
      const res = await axios.post('http://localhost:5143/upload', formData);
      setSummary(res.data);
      setError(null);
    } catch (err)
    {
      console.error('Upload error:', err);
      setSummary(null);
      setError('Upload failed.');
    }
  };

  return (
    <div>
      <h2>Upload CSV</h2>
      <input type="file" onChange={handleUpload} />
      {summary && (
        <div>
          <h3>Summary</h3>
          <ul>
            <li>Submitted: {summary.submitted}</li>
            <li>Invalid: {summary.invalid}</li>
            <li>Processed: {summary.processed}</li>
            <li>Updated: {summary.updated}</li>
            <li>Added: {summary.added}</li>
          </ul>
        </div>
      )}
      {error && <p style={{ color: 'red' }}>{error}</p>}
    </div>
  );
}