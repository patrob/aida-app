import { Helmet } from "react-helmet";
import { Hero } from "@/components/Hero";
import { Features } from "@/components/Features";
import { HowItWorks } from "@/components/HowItWorks";

const Index = () => {
  return (
    <>
      <Helmet>
        <title>AIDA - AI Developer Assistant</title>
        <meta name="description" content="AIDA turns your requirements into pull requests, streamlining your development process with AI-powered automation." />
        <meta property="og:title" content="AIDA - AI Developer Assistant" />
        <meta property="og:description" content="Transform your development workflow with AI-powered code generation and automated pull requests." />
        <meta name="twitter:card" content="summary_large_image" />
      </Helmet>
      <main className="min-h-screen">
        <Hero />
        <Features />
        <HowItWorks />
      </main>
    </>
  );
};

export default Index; 