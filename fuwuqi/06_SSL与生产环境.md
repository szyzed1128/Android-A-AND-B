# 06 SSL 与生产环境配置

> 当前测试阶段无需 SSL，本文档在进入生产阶段时参考。

---

## 一、为什么生产环境必须用 wss://

iOS App Store 要求所有网络请求必须使用 ATS（App Transport Security）：
- 所有 HTTP 连接需升级为 HTTPS
- 所有 WebSocket（ws://）需升级为 WSS（wss://）
- 否则 App Store 审核时会被拒绝，或用户在 iOS 14+ 设备上连接失败

Android 没有强制要求，但为安全一致性，生产环境统一使用 wss://。

---

## 二、域名申请

**推荐**：在阿里云控制台申请域名（与服务器同平台，解析方便）。

域名示例：`obd-server.your-domain.com`

申请后添加 DNS A 记录：
```
类型: A
主机记录: obd-server（或 @）
记录值: 服务器公网 IP
TTL: 600
```

---

## 三、SSL 证书申请（Let's Encrypt 免费）

```bash
# 1. 安装 certbot
sudo apt install -y certbot python3-certbot-nginx

# 2. 申请证书（需要 80 端口可访问，且域名已解析到本服务器）
sudo certbot --nginx -d obd-server.your-domain.com

# 3. 测试自动续期
sudo certbot renew --dry-run

# 证书文件位置
# /etc/letsencrypt/live/obd-server.your-domain.com/fullchain.pem
# /etc/letsencrypt/live/obd-server.your-domain.com/privkey.pem
```

### 自动续期（certbot 默认已配置 cron 或 systemd timer）

```bash
# 验证自动续期任务
sudo systemctl status certbot.timer
# 或
sudo crontab -l | grep certbot
```

---

## 四、Nginx 生产配置

参考 `03_端口与代理配置.md` 中的完整 Nginx 配置。

**关键配置项**：
```nginx
location /ws {
    proxy_pass http://obd_websocket;    # 指向 127.0.0.1:8080
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection "upgrade";
    proxy_read_timeout 3600s;           # WebSocket 长连接
    proxy_buffering off;                # WebSocket 必须关闭缓冲
}
```

**启用配置**：
```bash
sudo ln -sf /etc/nginx/sites-available/obd-websocket /etc/nginx/sites-enabled/
sudo nginx -t       # 语法检查，看到 OK 再继续
sudo systemctl reload nginx
```

---

## 五、B 端 App 的 wss:// 适配

**CloudBridge.ts 已经支持 wss://**（connect 方法第 166 行）：
```typescript
const hasProtocol = /^wss?:\/\//i.test(host);
let base = hasProtocol ? host : `ws://${host}:${port}`;
```

所以生产环境时，用户在 App 设置界面输入：
```
wss://obd-server.your-domain.com
```
（包含协议前缀，端口由 URL 决定，使用默认 443）

---

## 六、iOS ATS 配置确认

iOS App 的 `Info.plist` 中需要确保没有 `NSAllowsArbitraryLoads = true`（这是开发期间的临时配置）。

生产环境使用 wss:// 后，ATS 要求自动满足，无需特殊配置。

如果测试阶段需要临时允许 ws://（非加密），需在 Info.plist 添加例外（仅测试，上架前需要移除）：
```xml
<key>NSAppTransportSecurity</key>
<dict>
    <key>NSExceptionDomains</key>
    <dict>
        <key>你的服务器IP</key>
        <dict>
            <key>NSExceptionAllowsInsecureHTTPLoads</key>
            <true/>
            <key>NSIncludesSubdomains</key>
            <true/>
        </dict>
    </dict>
</dict>
```

---

## 七、生产环境切换检查清单

- [ ] 域名已申请并 DNS 解析到服务器
- [ ] Let's Encrypt 证书已申请
- [ ] Nginx 配置已更新为 SSL 版本
- [ ] B端 App 设置界面输入 `wss://域名` 测试连通
- [ ] iOS Info.plist 已移除 `NSAllowsArbitraryLoads`（如果有的话）
- [ ] 阿里云安全组已开放 443 端口
- [ ] 证书自动续期已验证
- [ ] 关闭安全组中的 8080 端口（不再对外暴露）
