import { defineStore } from 'pinia'

export const useUrlStore = defineStore('url', {
  state: () => ({ original: '', new: '', error: ''  }),
  actions: {
    set (newUrl: string) {
      this.original = newUrl;
    },
    shorten (shortUrl: string) {
      this.new = shortUrl;
      this.error = '';
    },
    setError (errorText: string) {
      this.error = errorText;
      this.new = '';
    },
    validate () {
      try {
        new URL(this.original);
        return true;
      } catch {
        this.error = 'Invalid URL format!';
        this.new = '';
        return false;
      }
    }
  }
});
