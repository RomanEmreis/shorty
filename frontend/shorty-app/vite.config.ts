import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

const apiUrl                   = process.env.services__ingress__https__0;
const isProduction             = process.env.NODE_ENV !== 'development';
const urlResolvePatter: RegExp = /\/[a-zA-Z0-9]{7}$/;

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    host: true,
    port: parseInt(process.env.PORT ?? "5173"),
    proxy: {
      '/create': {
        target: apiUrl,
        secure: isProduction,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/create/, '/create'),
        bypass: ({ method, url }) => {
          if (method !== 'POST') {
            return url;
          }
        }
      },
      '/': {
        target: apiUrl,
        secure: isProduction,
        changeOrigin: true,
        bypass: ({ method, url }) => {
          if (method !== 'GET' || !url?.match(urlResolvePatter)) {
            return url;
          }
        }
      }
    }
  }
})
