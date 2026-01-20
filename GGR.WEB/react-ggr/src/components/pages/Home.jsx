
const Home = () => {
  return (
      <div>
          <h1>Hello, world!</h1>
          <p>Welcome to your new single-page application, built with:</p>
          <ul>
              <li>
                  <a href="https://get.asp.net/" target="_blank" rel="noreferrer">
                      ASP.NET Core
                  </a>{' '}
                  and{' '}
                  <a href="https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx" target="_blank" rel="noreferrer">
                      C#
                  </a>{' '}
                  for cross-platform server-side code
              </li>
              <li>
                  <a href="https://react.dev/" target="_blank" rel="noreferrer">
                      React
                  </a>{' '}
                  for client-side code
              </li>
              <li>
                  <a href="https://getbootstrap.com/" target="_blank" rel="noreferrer">
                      Bootstrap
                  </a>{' '}
                  for layout and styling
              </li>
          </ul>
          <p>To help you get started, we have also set up:</p>
          <ul>
              <li>
                  <strong>Client-side navigation</strong>. For example, click{' '}
                  <em>Counter</em> then <em>Back</em> to return here.
              </li>
              <li>
                  <strong>Development server integration</strong>. In development mode,
                  the development server runs in the background automatically.
              </li>
              <li>
                  <strong>Efficient production builds</strong>. Production builds are
                  minified and optimized.
              </li>
          </ul>
          <p>
            The <code>ClientApp</code> subdirectory is a standard React application.
            You can run commands like <code>npm install</code> or{' '}
            <code>npm test</code>.
          </p>
      </div>
  );
};

export default Home;
