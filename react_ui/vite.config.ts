import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import legacy from '@vitejs/plugin-legacy'

export default defineConfig({
  plugins: [
    react(),
    legacy({
      targets: ['defaults', 'android >= 5'],
    }),
  ],
  base: './', 
  build: {
    outDir: '../my_source_code', 
    emptyOutDir: true,
    chunkSizeWarningLimit: 1000,
    rollupOptions: {
      output: {
        manualChunks: {
          vant: ['react-vant', '@react-vant/icons'],
          react: ['react', 'react-dom', 'react-router-dom']
        }
      }
    }
  }
})