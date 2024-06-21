export default class ShortyApi {
  create  = (url, onload, onerror) => {
    const xhr   = new XMLHttpRequest();
    xhr.open('POST', '/create');
    xhr.setRequestHeader("Content-Type", "application/json;");
    xhr.onerror = () => onerror?.();
    xhr.onload  = ({ target: request }) => {
      if (request.status === 200) {
        onload?.(`${request.responseURL.replace('/create', '')}/${request.responseText}`);
      }
    };
    xhr.send(JSON.stringify({ url }));
  };
}
