import { useCallback, useState } from 'react';
import ShortyApi from '../api/ShortyApi';
import { validateUrl } from '../utils/Validators';
import logo from '../assets/logo.png'

import './App.css';

const api = new ShortyApi();

export default () => {
  const [error , setError]  = useState(null);
  const [newUrl, setNewUrl] = useState(null);
  const [url   , setUrl]    = useState('');

  const handleFocus         = useCallback((e) => e.target.select(), []);
  const handleChange        = useCallback((e) => setUrl(e.target.value), [])
  const handleCopy          = useCallback(() => navigator.clipboard.writeText(newUrl), [newUrl]);
  const handleClick         = useCallback(() => {
    setError(null);
    setNewUrl(null);
    if (validateUrl(url)) {
      api.create(
        url, 
        (shortUrl) => setNewUrl(shortUrl.replace(/['"]+/g, '')),
        () => setError('An Internal Server Error occurred')
      );
    } else {
      setError("Please provide the correct URL to shortify!");
    }
  }, [url]);

  return (
    <div className='center-screen'>
      <img className='logo-img' src={logo} alt='logo' />
      <div className='create-field'>
        <input className='input-field' type='text' value={url} onChange={handleChange} onFocus={handleFocus} />
        <button className='create-btn' onClick={handleClick}>Generate</button>
      </div>
      {newUrl && (
        <div className='result-field'>
          <a className='result-text' href={newUrl} target='_blank'>{newUrl}</a>
          <button className='copy-btn' onClick={handleCopy}>Copy</button>
        </div>
      )}
      {error && (
        <div className='error-field'>
          <span className='error-text'>{error}</span>
        </div>
      )}
      <div className='copyright'>Created by Roman Emreis & AI</div>
    </div>
  );
};