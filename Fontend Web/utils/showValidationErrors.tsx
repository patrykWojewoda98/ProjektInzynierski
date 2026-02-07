export const showValidationErrors = (error: any) => {
  const errors = error?.response?.data?.errors ?? error?.response?.data;

  if (!errors) {
    return "Validation failed.";
  }

  if (typeof errors === "string") {
    return errors;
  }

  if (Array.isArray(errors)) {
    return errors.join("\n");
  }

  return Object.values(errors).flat().join("\n");
};
