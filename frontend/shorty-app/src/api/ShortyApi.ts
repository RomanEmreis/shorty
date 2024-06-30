export default class ShortyApi {
  create = (
    originalUrl: string,
    onSuccess: (newUrl: string) => void,
    onError: (errortext: string) => void
  ) => {
    const xhr = new XMLHttpRequest();

    xhr.open('POST', '/create');
    xhr.setRequestHeader("Content-Type", "application/json;");

    xhr.onerror = () => onError('An error occurred while creating a short URL');
    xhr.onload = () => {
      if (xhr.status === 200) {
        const newUrl = `${xhr.responseURL.replace('/create', '')}/${xhr.responseText.replace(/['"]+/g, '')}`;
        onSuccess(newUrl);
      }
    };
    xhr.send(JSON.stringify({ url: originalUrl }));
  };
}