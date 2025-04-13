import { render, screen } from '@testing-library/react';
import FileUploader from '../src/components/FileUploader';

test('renders upload component', () =>
{
  render(<FileUploader />);
  expect(screen.getByText(/upload csv/i)).toBeInTheDocument();
});