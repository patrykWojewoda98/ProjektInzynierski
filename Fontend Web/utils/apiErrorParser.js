export function parseApiError(error) {
  if (error && error.response) {
    const data = error.response.data;

    // 1️⃣ ModelState / FluentValidation (standard)
    if (data && data.errors && typeof data.errors === "object") {
      return Object.values(data.errors).flat();
    }

    // 2️⃣ FluentValidation jako string (stack trace)
    if (typeof data === "string") {
      // szukamy linii z "-- Field: message"
      const lines = data.split("\n");

      const validationLines = lines.filter((l) => l.trim().startsWith("--"));

      if (validationLines.length > 0) {
        return validationLines.map((l) =>
          l.replace(/^--\s*/, "").replace(/^[^:]+:\s*/, "")
        );
      }

      // fallback: pierwsza sensowna linia
      const firstLine = lines.find((l) => l && !l.startsWith("at "));

      if (firstLine) {
        return [firstLine.trim()];
      }
    }

    // 3️⃣ message z backendu
    if (data && typeof data.message === "string") {
      return [data.message];
    }
  }

  // 4️⃣ fallback
  return ["Unexpected error occurred."];
}
