
import { Code, GitPullRequest, Rocket, Terminal } from "lucide-react";

const features = [
  {
    icon: Code,
    title: "AI-Powered Code Generation",
    description: "Transform requirements into working code using advanced AI models.",
  },
  {
    icon: GitPullRequest,
    title: "Automated Pull Requests",
    description: "Generate PRs automatically from your requirement tickets.",
  },
  {
    icon: Terminal,
    title: "Smart Code Analysis",
    description: "Get intelligent code suggestions and improvements in real-time.",
  },
  {
    icon: Rocket,
    title: "Accelerated Development",
    description: "Speed up your development cycle with AI automation.",
  },
];

export const Features = () => {
  return (
    <section className="py-20 bg-slate-50">
      <div className="container px-4 md:px-6">
        <div className="text-center mb-12">
          <h2 className="text-3xl font-bold mb-4">Powerful Features</h2>
          <p className="text-muted-foreground text-lg max-w-[600px] mx-auto">
            Everything you need to streamline your development workflow with AI.
          </p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {features.map((feature, index) => (
            <div
              key={index}
              className="group relative bg-white p-6 rounded-2xl shadow-sm hover:shadow-md transition-shadow animate-fade-up opacity-0"
              style={{ animationDelay: `${index * 100 + 200}ms` }}
            >
              <div className="mb-4 inline-block p-3 rounded-xl bg-gradient-to-br from-aida-magenta to-aida-fuchsia text-white">
                <feature.icon className="w-6 h-6" />
              </div>
              <h3 className="text-xl font-semibold mb-2">{feature.title}</h3>
              <p className="text-muted-foreground">{feature.description}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};
