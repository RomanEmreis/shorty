export default class ShortyApi {
  create = async (originalUrl: string) : Promise<string | null> => {
    const response = await fetch('/create', {
      method: 'POST',
      body: JSON.stringify({ url: originalUrl }),
      headers: {
        "Content-Type": "application/json",
      }
    });

    if (response.ok) {
      const responseText = await response.text();
      return `${response.url.replace('/create', '')}/${responseText.replace(/['"]+/g, '')}`;
    } else {
      return null;
    }
  }
}