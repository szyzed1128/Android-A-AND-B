const WebSocket = require('ws');
const ws = new WebSocket('ws://localhost:9222/devtools/page/BE44296848F683057F290DAF2D8A8FF1');

ws.on('open', () => {
  console.log('Connected to CDP');
  ws.send(JSON.stringify({id: 1, method: 'Runtime.enable'}));
});

ws.on('message', (data) => {
  const msg = JSON.parse(data);
  
  if (msg.id === 1) {
    console.log('Runtime enabled, calling startBTScan...');
    ws.send(JSON.stringify({
      id: 2,
      method: 'Runtime.evaluate',
      params: {
        expression: 'window.JSBridge.startBTScan()',
        returnByValue: true
      }
    }));
  }

  if (msg.id === 2) {
    console.log('startBTScan result:', JSON.stringify(msg.result, null, 2));
    setTimeout(() => {
      ws.close();
      process.exit(0);
    }, 2000);
  }
});

ws.on('error', (err) => {
  console.error('Error:', err);
  process.exit(1);
});

setTimeout(() => {
  console.log('Timeout');
  ws.close();
  process.exit(1);
}, 15000);
