import { createContext, useContext, useEffect, useState, ReactNode } from "react";

interface AuthContextType {
  isAuthenticated: boolean | null;
  login: (provider: "google" | "github") => Promise<void>;
  logout: () => void;
  user: User | null;
}

interface User {
  id: string;
  name: string;
  email: string;
  avatar?: string;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    // Check if the user is already logged in on initial load
    const checkAuth = () => {
      const token = localStorage.getItem("aida-auth");
      const userData = localStorage.getItem("aida-user");
      
      if (token) {
        setIsAuthenticated(true);
        if (userData) {
          setUser(JSON.parse(userData));
        }
      } else {
        setIsAuthenticated(false);
      }
    };

    checkAuth();
  }, []);

  const login = async (provider: "google" | "github") => {
    // This is a stub implementation - in a real app, this would make API calls to your backend
    // which would handle the OAuth flow with Google or GitHub
    
    console.log(`Logging in with ${provider}`);
    
    // Simulate successful login after a short delay
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    // Generate a mock user based on the provider
    const mockUser: User = {
      id: `user-${Date.now()}`,
      name: provider === "google" ? "Google User" : "GitHub User",
      email: provider === "google" ? "user@gmail.com" : "user@github.com",
      avatar: provider === "google" 
        ? "https://lh3.googleusercontent.com/a/default-user=s64-c"
        : "https://avatars.githubusercontent.com/u/default?v=4",
    };
    
    // Store auth info in localStorage (for demo purposes - in production use secure HTTP-only cookies)
    localStorage.setItem("aida-auth", "dummy-token");
    localStorage.setItem("aida-user", JSON.stringify(mockUser));
    
    setIsAuthenticated(true);
    setUser(mockUser);
  };

  const logout = () => {
    localStorage.removeItem("aida-auth");
    localStorage.removeItem("aida-user");
    setIsAuthenticated(false);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout, user }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

export default useAuth; 