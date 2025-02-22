
import { Check } from "lucide-react";

const steps = [
  {
    title: "Connect Your Repository",
    description: "Link AIDA to your GitHub repository to get started.",
  },
  {
    title: "Add Requirements",
    description: "Create or import your requirements and user stories.",
  },
  {
    title: "Generate Code",
    description: "Let AI analyze requirements and generate initial code.",
  },
  {
    title: "Review & Merge",
    description: "Review the generated PR and merge when ready.",
  },
];

export const HowItWorks = () => {
  return (
    <section className="py-20">
      <div className="container px-4 md:px-6">
        <div className="text-center mb-12">
          <h2 className="text-3xl font-bold mb-4">How It Works</h2>
          <p className="text-muted-foreground text-lg max-w-[600px] mx-auto">
            Get from requirements to code in four simple steps.
          </p>
        </div>
        <div className="max-w-4xl mx-auto">
          {steps.map((step, index) => (
            <div
              key={index}
              className="flex items-start gap-4 mb-8 animate-fade-up opacity-0"
              style={{ animationDelay: `${index * 100 + 200}ms` }}
            >
              <div className="flex-shrink-0 w-10 h-10 rounded-full bg-gradient-to-r from-aida-magenta to-aida-fuchsia flex items-center justify-center text-white">
                <Check className="w-5 h-5" />
              </div>
              <div>
                <h3 className="text-xl font-semibold mb-2">{step.title}</h3>
                <p className="text-muted-foreground">{step.description}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};
