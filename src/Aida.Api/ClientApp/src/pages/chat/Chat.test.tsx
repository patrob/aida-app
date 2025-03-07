import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { vi, describe, test, expect, beforeEach, afterEach } from 'vitest';
import Chat from './Chat';

// Mock the Helmet component
vi.mock('react-helmet', () => ({
  Helmet: ({ children }: { children: React.ReactNode }) => <div data-testid="helmet">{children}</div>,
}));

// Mock the ScrollArea component
vi.mock('@/components/ui/scroll-area', () => ({
  ScrollArea: ({ children }: { children: React.ReactNode }) => <div data-testid="scroll-area">{children}</div>,
}));

// Mock required UI components
vi.mock('@/components/ui/tooltip', () => ({
  Tooltip: ({ children }: { children: React.ReactNode }) => <div>{children}</div>,
  TooltipContent: ({ children }: { children: React.ReactNode }) => <div>{children}</div>,
  TooltipProvider: ({ children }: { children: React.ReactNode }) => <div>{children}</div>,
  TooltipTrigger: ({ children }: { children: React.ReactNode }) => <div>{children}</div>,
}));

// Mock the Button component
vi.mock('@/components/ui/button', () => ({
  Button: ({ children, onClick, type }: { children: React.ReactNode, onClick?: () => void, type?: "submit" | "reset" | "button" }) => (
    <button onClick={onClick} type={type || 'button'} data-testid={type === 'submit' ? 'submit-button' : 'button'}>
      {children}
    </button>
  ),
}));

// Mock cn utility to pass through classNames
vi.mock('@/lib/utils', () => ({
  cn: (...args: any[]) => args.filter(Boolean).join(' '),
}));

// Mock icons
vi.mock('lucide-react', () => ({
  Paperclip: () => <span data-testid="paperclip-icon">Paperclip</span>,
  Send: () => <span data-testid="send-icon">Send</span>,
  X: () => <span data-testid="x-icon">X</span>,
  User: () => <span data-testid="user-icon">User</span>,
  Bot: () => <span data-testid="bot-icon">Bot</span>,
}));

describe('Chat Component', () => {
  // Setup and teardown
  beforeEach(() => {
    // Reset mocks before each test
    vi.clearAllMocks();
    
    // Mock scrollIntoView - not implemented in JSDOM
    Element.prototype.scrollIntoView = vi.fn();
  });

  afterEach(() => {
    // Reset window.alert mock after each test
    vi.restoreAllMocks();
  });

  // Basic rendering test
  test('renders chat interface correctly', () => {
    render(<Chat />);
    
    // Check if the header exists
    expect(screen.getByText('AIDA Chat')).toBeInTheDocument();
    
    // Check if the initial welcome message exists
    expect(screen.getByText(/Hello! I'm AIDA/)).toBeInTheDocument();
    
    // Check if the input field exists
    expect(screen.getByPlaceholderText('Type your message...')).toBeInTheDocument();
    
    // Check for send icon
    expect(screen.getByTestId('send-icon')).toBeInTheDocument();
    
    // Check for paperclip icon (attachment)
    expect(screen.getByTestId('paperclip-icon')).toBeInTheDocument();
  });
  
  // Input handling test
  test('handles user input correctly', () => {
    render(<Chat />);
    
    const inputField = screen.getByPlaceholderText('Type your message...');
    const testMessage = 'Hello AIDA, how are you?';
    
    // Type message into input
    fireEvent.change(inputField, { target: { value: testMessage } });
    
    // Check if input value was updated
    expect(inputField).toHaveValue(testMessage);
  });
  
  // Message submission test
  test('submits user message and displays it', async () => {
    render(<Chat />);
    
    const inputField = screen.getByPlaceholderText('Type your message...');
    const testMessage = 'Hello AIDA, how are you?';
    
    // Type message into input
    fireEvent.change(inputField, { target: { value: testMessage } });
    
    // Find and submit the form
    const form = screen.getByPlaceholderText('Type your message...').closest('form');
    expect(form).not.toBeNull();
    fireEvent.submit(form!);
    
    // Check if user message appears in the chat
    expect(screen.getByText(testMessage)).toBeInTheDocument();
    
    // Check if input is cleared after submission
    expect(inputField).toHaveValue('');
    
    // Wait for AI response
    await waitFor(() => {
      expect(screen.getByText(/I've received your message/)).toBeInTheDocument();
    }, { timeout: 1500 });
  });
  
  // File selection test
  test('handles file selection via input change', () => {
    render(<Chat />);
    
    // Find the file input
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    expect(fileInput).not.toBeNull();
    
    // Create a mock file
    const file = new File(['file content'], 'test.txt', { type: 'text/plain' });
    
    // Simulate file selection directly
    fireEvent.change(fileInput, { target: { files: [file] } });
    
    // Check if file name appears in the UI
    expect(screen.getByText('test.txt')).toBeInTheDocument();
  });
  
  // File removal test
  test('allows removal of selected files', () => {
    render(<Chat />);
    
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    
    // Create a mock file
    const file = new File(['file content'], 'test.txt', { type: 'text/plain' });
    
    // Simulate file selection
    fireEvent.change(fileInput, { target: { files: [file] } });
    
    // Check if file name appears in the UI
    expect(screen.getByText('test.txt')).toBeInTheDocument();
    
    // Find and click the remove button with X icon
    const removeButton = screen.getByTestId('x-icon').closest('button');
    expect(removeButton).not.toBeNull();
    fireEvent.click(removeButton!);
    
    // Check if file name was removed
    expect(screen.queryByText('test.txt')).not.toBeInTheDocument();
  });
  
  // File attachment limit test
  test('enforces file limit of 5 attachments', () => {
    // Mock alert before rendering
    const alertMock = vi.spyOn(window, 'alert').mockImplementation(() => {});
    
    render(<Chat />);
    
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    
    // Create 6 mock files
    const files = Array.from({ length: 6 }, (_, i) => 
      new File(['content'], `test${i}.txt`, { type: 'text/plain' })
    );
    
    // Try to add 6 files
    fireEvent.change(fileInput, { target: { files } });
    
    // Check if alert was triggered
    expect(alertMock).toHaveBeenCalledWith('You can only upload up to 5 files at a time.');
  });
  
  // Drag and drop tests
  test('handles drag events and file drops', () => {
    render(<Chat />);
    
    // Get the main container which should have the drop handlers
    const container = screen.getByTestId('scroll-area').parentElement as HTMLElement;
    
    // Simulate drag enter
    fireEvent.dragEnter(container, {
      dataTransfer: {
        types: ['Files'],
        files: [],
      },
    });
    
    // Create mock files
    const files = [
      new File(['content1'], 'dropped1.txt', { type: 'text/plain' }),
    ];
    
    // Simulate file drop
    fireEvent.drop(container, {
      dataTransfer: {
        types: ['Files'],
        files,
      },
    });
    
    // Check if file was added
    expect(screen.getByText('dropped1.txt')).toBeInTheDocument();
  });
  
  test('sends message with attached files', () => {
    render(<Chat />);
    
    const inputField = screen.getByPlaceholderText('Type your message...');
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    
    // Create a mock file
    const file = new File(['file content'], 'attachment.txt', { type: 'text/plain' });
    
    // Add a message
    fireEvent.change(inputField, { target: { value: 'Here is an attached file' } });
    
    // Add a file
    fireEvent.change(fileInput, { target: { files: [file] } });
    
    // Find and submit the form
    const form = screen.getByPlaceholderText('Type your message...').closest('form');
    expect(form).not.toBeNull();
    fireEvent.submit(form!);
    
    // Check if the message appears
    expect(screen.getByText('Here is an attached file')).toBeInTheDocument();
    
    // Check for the filename in the message
    const messageElements = screen.getAllByText('attachment.txt');
    expect(messageElements.length).toBeGreaterThan(0);
  });
}); 