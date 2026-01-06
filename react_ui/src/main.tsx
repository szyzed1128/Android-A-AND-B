import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
// Assuming global styles or vant styles might need handling, but basic import works.
// import 'react-vant/lib/index.css'; // Vant 3+ usually handles this via plugin or direct import if needed.

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
