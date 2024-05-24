export default class ShortyApi {
  baseUrl = process.env['services__shorty-api__http__0'];
  create  = (url, onload, onerror) => {
    const xhr = new XMLHttpRequest();
    xhr.open('POST', this.baseUrl);
    xhr.setRequestHeader("Content-Type", "application/json;");
    xhr.onerror = () => onerror?.();
    xhr.onload = () => {
      if (xhr.status === 200) {
        onload?.(xhr.responseText);
      }
    };
    xhr.send(JSON.stringify({ url }));
  };
}