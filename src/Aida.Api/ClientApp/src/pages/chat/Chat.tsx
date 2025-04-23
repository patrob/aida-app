import { useState, useRef } from "react";
import { Helmet } from "react-helmet";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Paperclip, Send, X, User, Bot } from "lucide-react";
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip";
import { cn } from "@/lib/utils";

interface Message {
  id: string;
  content: string;
  role: "user" | "assistant";
  files?: File[];
}

const Chat = () => {
  const [messages, setMessages] = useState<Message[]>([
    {
      id: "1",
      content: "Hello! I'm AIDA, your AI Developer Assistant. How can I help you today?",
      role: "assistant",
    },
  ]);
  const [inputValue, setInputValue] = useState("");
  const [selectedFiles, setSelectedFiles] = useState<File[]>([]);
  const [isDraggingOver, setIsDraggingOver] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const dropAreaRef = useRef<HTMLDivElement>(null);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInputValue(e.target.value);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      const newFiles = Array.from(e.target.files);
      addFiles(newFiles);
    }
  };

  const addFiles = (newFiles: File[]) => {
    if (selectedFiles.length + newFiles.length <= 5) {
      setSelectedFiles([...selectedFiles, ...newFiles]);
    } else {
      alert("You can only upload up to 5 files at a time.");
    }
  };

  const handleFileRemove = (index: number) => {
    setSelectedFiles(selectedFiles.filter((_, i) => i !== index));
  };

  const handleFileSelect = () => {
    fileInputRef.current?.click();
  };

  const handleDragEnter = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDraggingOver(true);
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (!isDraggingOver) {
      setIsDraggingOver(true);
    }
  };

  const handleDragLeave = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    
    // Only set dragging state to false if we're leaving the main drop area
    // and not just moving between child elements
    if (dropAreaRef.current && !dropAreaRef.current.contains(e.relatedTarget as Node)) {
      setIsDraggingOver(false);
    }
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDraggingOver(false);

    if (e.dataTransfer.files && e.dataTransfer.files.length > 0) {
      const droppedFiles = Array.from(e.dataTransfer.files);
      addFiles(droppedFiles);
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (inputValue.trim() === "" && selectedFiles.length === 0) return;

    const newUserMessage: Message = {
      id: Date.now().toString(),
      content: inputValue,
      role: "user",
      files: selectedFiles.length > 0 ? [...selectedFiles] : undefined,
    };

    setMessages([...messages, newUserMessage]);
    setInputValue("");
    setSelectedFiles([]);
    
    // Simulate AI response after a small delay
    setTimeout(() => {
      const aiResponse: Message = {
        id: (Date.now() + 1).toString(),
        content: "I've received your message. This is a stub response that would normally come from the backend LLM API.",
        role: "assistant",
      };
      setMessages(prev => [...prev, aiResponse]);
      
      // Scroll to bottom after new message
      messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, 1000);
  };

  return (
    <>
      <Helmet>
        <title>Chat - AIDA</title>
        <meta name="description" content="Chat with AIDA, your AI Developer Assistant" />
      </Helmet>
      <div 
        ref={dropAreaRef}
        className={cn(
          "flex h-screen flex-col bg-gray-50 dark:bg-gray-900 relative",
          isDraggingOver && "bg-blue-50 dark:bg-blue-950"
        )}
        onDragEnter={handleDragEnter}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
        onDrop={handleDrop}
      >
        {/* Drag overlay indicator */}
        {isDraggingOver && (
          <div className="absolute inset-0 bg-blue-500/10 pointer-events-none flex items-center justify-center border-2 border-dashed border-blue-500 z-10 rounded-lg m-4">
            <div className="bg-white dark:bg-gray-800 p-4 rounded-lg shadow-lg text-center">
              <Paperclip className="h-8 w-8 mx-auto mb-2 text-blue-500" />
              <p className="text-lg font-medium">Drop files to attach</p>
              <p className="text-sm text-gray-500 dark:text-gray-400">Maximum 5 files allowed</p>
            </div>
          </div>
        )}

        {/* Header */}
        <header className="border-b border-gray-200 dark:border-gray-800 p-4">
          <h1 className="text-xl font-bold text-center">AIDA Chat</h1>
        </header>

        {/* Chat Messages */}
        <ScrollArea className="flex-1 p-4">
          <div className="max-w-3xl mx-auto space-y-4">
            {messages.map((message) => (
              <div
                key={message.id}
                className={cn(
                  "flex gap-3 p-4 rounded-lg",
                  message.role === "user"
                    ? "bg-white dark:bg-gray-800 ml-10"
                    : "bg-gray-100 dark:bg-gray-700 mr-10"
                )}
              >
                <div className="flex-shrink-0">
                  {message.role === "user" ? (
                    <div className="w-8 h-8 rounded-full bg-primary flex items-center justify-center text-white">
                      <User className="h-4 w-4" />
                    </div>
                  ) : (
                    <div className="w-8 h-8 rounded-full bg-blue-500 flex items-center justify-center text-white">
                      <Bot className="h-4 w-4" />
                    </div>
                  )}
                </div>
                <div className="flex flex-col flex-1 overflow-hidden">
                  <div className="text-sm font-medium mb-1">
                    {message.role === "user" ? "You" : "AIDA"}
                  </div>
                  <div className="whitespace-pre-wrap break-words">{message.content}</div>
                  
                  {/* Display attached files if any */}
                  {message.files && message.files.length > 0 && (
                    <div className="mt-2">
                      <div className="text-sm text-gray-500 dark:text-gray-400 mb-1">Attached files:</div>
                      <div className="flex flex-wrap gap-2">
                        {message.files.map((file, index) => (
                          <div 
                            key={index}
                            className="flex items-center rounded bg-gray-100 dark:bg-gray-600 px-2 py-1 text-xs"
                          >
                            <Paperclip className="h-3 w-3 mr-1" />
                            {file.name}
                          </div>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
              </div>
            ))}
            <div ref={messagesEndRef} />
          </div>
        </ScrollArea>

        {/* Input Area */}
        <div className="border-t border-gray-200 dark:border-gray-800 p-4">
          <form onSubmit={handleSubmit} className="max-w-3xl mx-auto">
            {/* Display selected files */}
            {selectedFiles.length > 0 && (
              <div className="flex flex-wrap gap-2 mb-2">
                {selectedFiles.map((file, index) => (
                  <div
                    key={index}
                    className="flex items-center bg-gray-100 dark:bg-gray-700 rounded-full px-3 py-1 text-sm"
                  >
                    <Paperclip className="h-3 w-3 mr-1" />
                    <span className="truncate max-w-[100px]">{file.name}</span>
                    <button
                      type="button"
                      onClick={() => handleFileRemove(index)}
                      className="ml-1 text-gray-500 hover:text-gray-700"
                    >
                      <X className="h-3 w-3" />
                    </button>
                  </div>
                ))}
              </div>
            )}

            <div className="flex items-end gap-2">
              <input
                type="file"
                ref={fileInputRef}
                onChange={handleFileChange}
                className="hidden"
                multiple
              />
              <TooltipProvider>
                <Tooltip>
                  <TooltipTrigger asChild>
                    <Button
                      type="button"
                      size="icon"
                      variant="outline"
                      onClick={handleFileSelect}
                      disabled={selectedFiles.length >= 5}
                    >
                      <Paperclip className="h-5 w-5" />
                    </Button>
                  </TooltipTrigger>
                  <TooltipContent>
                    <p>Attach files (max 5)</p>
                  </TooltipContent>
                </Tooltip>
              </TooltipProvider>
              <div className="relative flex-1">
                <Input
                  value={inputValue}
                  onChange={handleInputChange}
                  placeholder="Type your message..."
                  className="pr-10 py-6 resize-none"
                />
                <Button
                  type="submit"
                  size="icon"
                  className="absolute right-1 bottom-1 top-1 h-auto"
                  disabled={inputValue.trim() === "" && selectedFiles.length === 0}
                >
                  <Send className="h-5 w-5" />
                </Button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </>
  );
};

export default Chat; 