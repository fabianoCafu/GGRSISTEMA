import React from "react";
import App from "../App";
import reportWebVitals from "../ReportWebVitals";
import "bootstrap/dist/css/bootstrap.css";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";


const baseUrl =
  document.getElementsByTagName("base")[0]?.getAttribute("href") ?? "/";

const rootElement = document.getElementById("root") as HTMLElement;

const root = createRoot(rootElement);

root.render(
  <React.StrictMode>
    <BrowserRouter basename={baseUrl}>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);

reportWebVitals();

