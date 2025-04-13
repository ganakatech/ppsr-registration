import React, { useState } from 'react';
import axios from 'axios';
import { Box, Button, Typography, Paper, Input, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';

export default function FileUploader()
{
  const [summary, setSummary] = useState(null);
  const [error, setError] = useState(null);
  const [lastUploadedFile, setLastUploadedFile] = useState(null);

  const handleUpload = async (e) =>
  {
    const file = e.target.files[0];
    if (!file) return;

    if (lastUploadedFile && lastUploadedFile.name === file.name)
    {
      setError('This file has already been uploaded. Please wait for the operation to complete or check the summary.');
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try
    {
      setError(null);
      setLastUploadedFile(file);
      const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5143';
      const res = await axios.post(`${apiUrl}/upload`, formData);
      setSummary(res.data);
    } catch (err)
    {
      console.error('Upload error:', err);
      setSummary(null);
      setError('Upload failed. Please try again.');
    }
  };

  return (
    <Box sx={{ maxWidth: 600, margin: '0 auto', padding: 2 }}>
      <Paper elevation={3} sx={{ padding: 3 }}>
        <Typography variant="h5" gutterBottom>
          Upload a .csv File
        </Typography>
        <Typography variant="body1" color="textSecondary" gutterBottom>
          Select a .csv file to upload and process vehicle registration data.
        </Typography>
        <Box sx={{ marginBottom: 2 }}>
          <label htmlFor="file-upload">
            <Input
              id="file-upload"
              type="file"
              onChange={handleUpload}
              inputProps={{ accept: '.csv' }}
              sx={{ display: 'none' }}
            />
            <Button variant="contained" component="span">
              Choose File
            </Button>
          </label>
        </Box>

        {lastUploadedFile && (
          <Typography variant="body2" color="textSecondary" sx={{ marginTop: 1 }}>
            Selected File: {lastUploadedFile.name}
          </Typography>
        )}

        {summary && (
          <Box sx={{ marginTop: 3 }}>
            <Typography variant="h6" align="center" gutterBottom>Processing Summary</Typography>
            <TableContainer component={Paper} sx={{ maxWidth: 400, margin: '0 auto' }}>
              <Table size="small" aria-label="processing summary table">
                <TableHead>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Metric</TableCell>
                    <TableCell align="center" sx={{ fontWeight: 'bold' }}>Count</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  <TableRow>
                    <TableCell>Submitted</TableCell>
                    <TableCell align="center">{summary.submitted}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Invalid</TableCell>
                    <TableCell align="center">{summary.invalid}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Processed</TableCell>
                    <TableCell align="center">{summary.processed}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Updated</TableCell>
                    <TableCell align="center">{summary.updated}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Added</TableCell>
                    <TableCell align="center">{summary.added}</TableCell>
                  </TableRow>
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
        )}

        {error && (
          <Typography variant="body2" color="error" sx={{ marginTop: 2 }}>
            {error}
          </Typography>
        )}
      </Paper>
    </Box>
  );
}