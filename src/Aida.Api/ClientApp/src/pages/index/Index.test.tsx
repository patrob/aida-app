import {render, screen} from "@testing-library/react";
import { vi, describe, it, expect } from "vitest";
import Index from "./Index";
import { MemoryRouter } from "react-router-dom";

// Mock the Hero component to avoid router issues
vi.mock("@/components/Hero", () => ({
  Hero: () => <div data-testid="hero">Hero Component</div>
}));

// Mock the Features component
vi.mock("@/components/Features", () => ({
  Features: () => <div data-testid="features">Features Component</div>
}));

// Mock the HowItWorks component
vi.mock("@/components/HowItWorks", () => ({
  HowItWorks: () => <div data-testid="how-it-works">How It Works Component</div>
}));

describe('Index', () => {
  it('should render', () => {
    render(
      <MemoryRouter>
        <Index/>
      </MemoryRouter>
    );
    
    // Verify the main components are rendered
    expect(screen.getByTestId("hero")).toBeInTheDocument();
    expect(screen.getByTestId("features")).toBeInTheDocument();
    expect(screen.getByTestId("how-it-works")).toBeInTheDocument();
  });
}); 