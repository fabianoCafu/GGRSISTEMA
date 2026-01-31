import type { Metric } from "web-vitals";
type ReportHandler = (metric: Metric) => void;

function reportWebVitals(onPerfEntry?: ReportHandler): void {
  if (onPerfEntry && typeof onPerfEntry === "function") {
    import("web-vitals").then(
      ({ onCLS, onFCP, onLCP, onTTFB }) => {
        onCLS(onPerfEntry);
        onFCP(onPerfEntry);
        onLCP(onPerfEntry);
        onTTFB(onPerfEntry);
      }
    );
  }
}

export default reportWebVitals;

