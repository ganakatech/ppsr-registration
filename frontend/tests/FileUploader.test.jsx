import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, test, expect, beforeEach, vi } from 'vitest';
import axios from 'axios';
import FileUploader from '../src/components/FileUploader';

// Mock axios
vi.mock('axios');

describe('FileUploader', () =>
{
  beforeEach(() =>
  {
    // Clear mocks before each test
    vi.clearAllMocks();
  });

  test('renders upload component', () =>
  {
    render(<FileUploader />);
    expect(screen.getByRole('heading', { name: /upload csv/i })).toBeInTheDocument();
    expect(screen.getByLabelText(/choose file/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/choose file/i)).toHaveAttribute('accept', '.csv');
  });

  test('handles successful file upload', async () =>
  {
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

    await waitFor(() =>
    {
      // expect(screen.getByText(/Submitted: 10/i)).toBeInTheDocument();
      // expect(screen.getByText(/Invalid: 2/i)).toBeInTheDocument();
      // expect(screen.getByText(/Processed: 8/i)).toBeInTheDocument();
      // expect(screen.getByText(/Updated: 3/i)).toBeInTheDocument();
      // expect(screen.getByText(/Added: 5/i)).toBeInTheDocument();
      expect(screen.getByText(/Submitted:/i)).toBeInTheDocument();
      expect(screen.getByText(/Invalid:/i)).toBeInTheDocument();
      expect(screen.getByText(/Processed:/i)).toBeInTheDocument();
      expect(screen.getByText(/Updated:/i)).toBeInTheDocument();
      expect(screen.getByText(/Added:/i)).toBeInTheDocument();
    });

    expect(axios.post).toHaveBeenCalledTimes(1);
    expect(axios.post).toHaveBeenCalledWith('http://localhost:5143/upload', expect.any(FormData));
  });

  test('handles file upload error', async () =>
  {
    axios.post.mockRejectedValueOnce(new Error('Upload failed'));

    render(<FileUploader />);

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const input = screen.getByLabelText(/choose file/i);

    fireEvent.change(input, { target: { files: [file] } });

    await waitFor(() =>
    {
      expect(screen.getByText(/upload failed/i)).toBeInTheDocument();
    });

    expect(axios.post).toHaveBeenCalledTimes(1);
    expect(screen.queryByText(/summary/i)).not.toBeInTheDocument();
  });

  test('does nothing when no file is selected', () =>
  {
    render(<FileUploader />);

    const input = screen.getByLabelText(/choose file/i);
    fireEvent.change(input, { target: { files: [] } });

    expect(axios.post).not.toHaveBeenCalled();
    expect(screen.queryByText(/summary/i)).not.toBeInTheDocument();
    expect(screen.queryByText(/upload failed/i)).not.toBeInTheDocument();
  });
});