import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Helmet } from "react-helmet";
import { useNavigate, useLocation } from "react-router-dom";
import { Github } from "lucide-react";
import { useAuth } from "@/lib/auth";
import { useState } from "react";

const Auth = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useAuth();
  const [isLoading, setIsLoading] = useState<"google" | "github" | null>(null);

  // Get the path to redirect to after login
  const from = (location.state as { from?: { pathname: string } })?.from?.pathname || "/chat";

  const handleGoogleSignIn = async () => {
    try {
      setIsLoading("google");
      await login("google");
      navigate(from, { replace: true });
    } catch (error) {
      console.error("Login failed:", error);
    } finally {
      setIsLoading(null);
    }
  };

  const handleGithubSignIn = async () => {
    try {
      setIsLoading("github");
      await login("github");
      navigate(from, { replace: true });
    } catch (error) {
      console.error("Login failed:", error);
    } finally {
      setIsLoading(null);
    }
  };

  return (
    <>
      <Helmet>
        <title>Sign In - AIDA</title>
        <meta name="description" content="Sign in to AIDA and get started with AI-powered code generation." />
      </Helmet>
      <div className="flex min-h-screen items-center justify-center bg-gray-50 dark:bg-gray-900 p-4">
        <Card className="w-full max-w-md">
          <CardHeader className="space-y-1 text-center">
            <CardTitle className="text-2xl font-bold">Welcome to AIDA</CardTitle>
            <CardDescription>
              Sign in using your preferred authentication method
            </CardDescription>
          </CardHeader>
          <CardContent className="grid gap-4">
            <Button 
              variant="outline" 
              className="bg-white hover:bg-gray-100 dark:bg-gray-800 dark:hover:bg-gray-700 flex items-center justify-center gap-2"
              onClick={handleGoogleSignIn}
              disabled={isLoading !== null}
            >
              {isLoading === "google" ? (
                <span className="animate-spin mr-2">⚪</span>
              ) : (
                <svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24">
                  <path d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" fill="#4285F4"/>
                  <path d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" fill="#34A853"/>
                  <path d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" fill="#FBBC05"/>
                  <path d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" fill="#EA4335"/>
                  <path d="M1 1h22v22H1z" fill="none"/>
                </svg>
              )}
              Continue with Google
            </Button>
            <Button 
              variant="outline" 
              className="bg-white hover:bg-gray-100 dark:bg-gray-800 dark:hover:bg-gray-700 flex items-center justify-center gap-2"
              onClick={handleGithubSignIn}
              disabled={isLoading !== null}
            >
              {isLoading === "github" ? (
                <span className="animate-spin mr-2">⚪</span>
              ) : (
                <Github className="h-5 w-5" />
              )}
              Continue with GitHub
            </Button>
          </CardContent>
          <CardFooter className="flex justify-center">
            <p className="text-sm text-gray-500 dark:text-gray-400">
              By continuing, you agree to our Terms of Service and Privacy Policy.
            </p>
          </CardFooter>
        </Card>
      </div>
    </>
  );
};

export default Auth; 