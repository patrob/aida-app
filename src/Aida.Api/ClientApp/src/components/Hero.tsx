import { ArrowRight } from "lucide-react";
import { Button } from "./ui/button";
import { Link } from "react-router-dom";

export const Hero = () => {
  return (
    <section className="relative min-h-[80vh] flex items-center justify-center overflow-hidden py-20">
      <div className="absolute inset-0 pointer-events-none">
        <div className="absolute inset-0 bg-gradient-to-br from-aida-magenta/5 to-aida-fuchsia/5" />
      </div>
      <div className="container px-4 md:px-6 relative z-10">
        <div className="flex flex-col items-center gap-4 text-center">
          <span className="inline-block animate-fade-in opacity-0 [--animation-delay:200ms] bg-gradient-to-r from-aida-magenta to-aida-fuchsia text-white px-4 py-1.5 text-sm font-medium rounded-full">
            Introducing AIDA
          </span>
          <h1 className="animate-fade-up opacity-0 text-4xl md:text-6xl font-bold tracking-tighter mx-auto max-w-3xl [text-wrap:balance]">
            Transform Your Development Workflow with AI
          </h1>
          <p className="animate-fade-up opacity-0 [--animation-delay:300ms] text-muted-foreground text-xl md:text-2xl mx-auto max-w-[600px] [text-wrap:balance]">
            AIDA turns your requirements into pull requests, streamlining your development process with AI-powered automation.
          </p>
          <div className="animate-fade-up opacity-0 [--animation-delay:400ms] flex flex-col sm:flex-row gap-4 min-[400px]:gap-6 mt-6">
            <Link to="/auth">
              <Button
                size="lg"
                className="bg-gradient-to-r from-aida-magenta to-aida-fuchsia hover:opacity-90 transition-opacity text-white font-semibold rounded-full"
              >
                Get Started
                <ArrowRight className="ml-2 h-4 w-4" />
              </Button>
            </Link>
            <Button
              variant="outline"
              size="lg"
              className="rounded-full border-2"
            >
              Learn More
            </Button>
          </div>
        </div>
      </div>
    </section>
  );
};
