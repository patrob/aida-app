import {render, screen} from "@testing-library/react";
import Index from "./Index";

describe('Index', () => {
  it('should render', () => {
    render(<Index/>);
    const title = screen.getByText(/Introducing AIDA/);
    expect(title).toBeInTheDocument();
  })
});
