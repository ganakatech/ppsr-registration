import React, { useState } from 'react';
import axios from 'axios';
import { Box, Button, Typography, Grid, Paper, Input } from '@mui/material';

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
      setError('Upload failed. Please try again.');
    }
  };

  return (
    <Box sx={{ maxWidth: 600, margin: '0 auto', padding: 2 }}>
      <Paper elevation={3} sx={{ padding: 3 }}>
        <Typography variant="h5" gutterBottom>
          Upload CSV File
        </Typography>
        <Typography variant="body1" color="textSecondary" gutterBottom>
          Select a CSV file to upload and process vehicle registration data.
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

        {summary && (
          <Box sx={{ marginTop: 3 }}>
            <Typography variant="h6">Processing Summary</Typography>
            <Grid container spacing={2} sx={{ marginTop: 1 }}>
              <Grid item xs={6}>
                <Typography variant="body2">Submitted:</Typography>
                <Typography variant="body1">{summary.submitted}</Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography variant="body2">Invalid:</Typography>
                <Typography variant="body1">{summary.invalid}</Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography variant="body2">Processed:</Typography>
                <Typography variant="body1">{summary.processed}</Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography variant="body2">Updated:</Typography>
                <Typography variant="body1">{summary.updated}</Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography variant="body2">Added:</Typography>
                <Typography variant="body1">{summary.added}</Typography>
              </Grid>
            </Grid>
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