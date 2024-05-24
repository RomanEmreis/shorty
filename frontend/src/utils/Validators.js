export const validateUrl = (url) => {
  try {
    const _ = new URL(url);
    return true;
  } catch {
    return false;
  }
};