import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, test, expect, beforeEach, vi } from 'vitest';
import axios from 'axios';
import FileUploader from '../src/components/FileUploader';

// Mock axios
vi.mock('axios');

describe('FileUploader', () => {
  beforeEach(() => {
    // Clear mocks before each test
    vi.clearAllMocks();
  });

  test('renders upload component', () => {
    render(<FileUploader />);
    expect(screen.getByRole('heading', { name: /upload a .csv File/i })).toBeInTheDocument();
    expect(screen.getByLabelText(/choose file/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/choose file/i)).toHaveAttribute('accept', '.csv');
  });

  test('handles successful file upload', async () => {
    const mockResponse = {
      submitted: 10,
      invalid: 2,
      processed: 8,
      updated: 3,
      added: 5
    };

    axios.post.mockResolvedValueOnce({ data: mockResponse });

    render(<FileUploader />);

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const input = screen.getByLabelText(/choose file/i);

    fireEvent.change(input, { target: { files: [file] } });

    await waitFor(() => {
      // Check for table headers
      expect(screen.getByText('Metric')).toBeInTheDocument();
      expect(screen.getByText('Count')).toBeInTheDocument();

      // Check for row labels
      expect(screen.getByText('Submitted')).toBeInTheDocument();
      expect(screen.getByText('Invalid')).toBeInTheDocument();
      expect(screen.getByText('Processed')).toBeInTheDocument();
      expect(screen.getByText('Updated')).toBeInTheDocument();
      expect(screen.getByText('Added')).toBeInTheDocument();

      // Check for values in table cells
      const cells = screen.getAllByRole('cell');
      expect(cells.find(cell => cell.textContent === '10')).toBeInTheDocument();
      expect(cells.find(cell => cell.textContent === '2')).toBeInTheDocument();
      expect(cells.find(cell => cell.textContent === '8')).toBeInTheDocument();
      expect(cells.find(cell => cell.textContent === '3')).toBeInTheDocument();
      expect(cells.find(cell => cell.textContent === '5')).toBeInTheDocument();
    });

    expect(axios.post).toHaveBeenCalledTimes(1);
    expect(axios.post).toHaveBeenCalledWith('http://localhost:5143/upload', expect.any(FormData));
  });

  test('prevents duplicate file upload', async () => {
    render(<FileUploader />);

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const input = screen.getByLabelText(/choose file/i);

    // First upload
    fireEvent.change(input, { target: { files: [file] } });

    // Try to upload the same file again
    fireEvent.change(input, { target: { files: [file] } });

    await waitFor(() => {
      expect(screen.getByText(/this file has already been uploaded/i)).toBeInTheDocument();
    });

    // Should only call API once
    expect(axios.post).toHaveBeenCalledTimes(1);
  });

  test('handles file upload error', async () => {
    axios.post.mockRejectedValueOnce(new Error('Upload failed'));

    render(<FileUploader />);

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const input = screen.getByLabelText(/choose file/i);

    fireEvent.change(input, { target: { files: [file] } });

    await waitFor(() => {
      expect(screen.getByText(/upload failed/i)).toBeInTheDocument();
    });

    expect(axios.post).toHaveBeenCalledTimes(1);
    expect(screen.queryByText(/summary/i)).not.toBeInTheDocument();
  });

  test('does nothing when no file is selected', () => {
    render(<FileUploader />);

    const input = screen.getByLabelText(/choose file/i);
    fireEvent.change(input, { target: { files: [] } });

    expect(axios.post).not.toHaveBeenCalled();
    expect(screen.queryByText(/summary/i)).not.toBeInTheDocument();
    expect(screen.queryByText(/upload failed/i)).not.toBeInTheDocument();
  });
});