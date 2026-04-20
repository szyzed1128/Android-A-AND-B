const AUTO_REFRESH_MS = 3000;

const elements = {
  overviewCards: document.getElementById('overview-cards'),
  serversBody: document.getElementById('servers-body'),
  instancesBody: document.getElementById('instances-body'),
  sessionsBody: document.getElementById('sessions-body'),
  anomaliesBody: document.getElementById('anomalies-body'),
  actionFeedback: document.getElementById('action-feedback'),
  lastUpdated: document.getElementById('last-updated'),
  autoRefresh: document.getElementById('auto-refresh'),
  onlyAnomalies: document.getElementById('only-anomalies'),
  refreshBtn: document.getElementById('refresh-btn'),
  repairBadBtn: document.getElementById('repair-bad-btn'),
  closeLogsBtn: document.getElementById('close-logs-btn'),
  sessionSearch: document.getElementById('session-search'),
  serversCount: document.getElementById('servers-count'),
  instancesCount: document.getElementById('instances-count'),
  sessionsCount: document.getElementById('sessions-count'),
  anomaliesCount: document.getElementById('anomalies-count'),
  logsPanel: document.getElementById('logs-panel'),
  logsOutput: document.getElementById('logs-output'),
};

let autoRefreshTimer = null;
let latestState = {
  overview: null,
  servers: [],
  instances: [],
  sessions: [],
  anomalies: [],
};

function formatDuration(ms) {
  if (typeof ms !== 'number') return '-';
  if (ms < 0) return '已过期';
  const totalSec = Math.floor(ms / 1000);
  const hours = Math.floor(totalSec / 3600);
  const minutes = Math.floor((totalSec % 3600) / 60);
  const seconds = totalSec % 60;
  if (hours > 0) return `${hours}h ${minutes}m ${seconds}s`;
  if (minutes > 0) return `${minutes}m ${seconds}s`;
  return `${seconds}s`;
}

function formatTimestamp(ts) {
  if (typeof ts !== 'number') return '-';
  return new Date(ts).toLocaleString('zh-CN', { hour12: false });
}

function formatPercent(value) {
  if (typeof value !== 'number' || Number.isNaN(value)) return '-';
  return `${value.toFixed(1)}%`;
}

function escapeHtml(text) {
  return String(text ?? '')
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#39;');
}

function renderStatus(status) {
  const text = escapeHtml(status ?? '-');
  return `<span class="status status-${text}">${text}</span>`;
}

function setActionFeedback(message, tone = 'info') {
  if (!elements.actionFeedback) return;
  elements.actionFeedback.textContent = message;
  elements.actionFeedback.className = `action-feedback ${tone}`;
  elements.actionFeedback.hidden = false;
}

function summarizeBatchRepairResult(payload) {
  const total = payload?.data?.total ?? 0;
  const results = Array.isArray(payload?.data?.results) ? payload.data.results : [];
  const successCount = results.filter(item => item?.success).length;
  const failed = results.filter(item => !item?.success);

  if (total === 0) {
    return '当前没有 bad 实例需要批量修复';
  }

  const lines = [`批量修复完成：总数 ${total}，成功 ${successCount}，失败 ${failed.length}`];
  failed.slice(0, 5).forEach(item => {
    lines.push(`${item.id}: ${item.error || '未知错误'}`);
  });
  if (failed.length > 5) {
    lines.push(`其余 ${failed.length - 5} 个失败项请查看日志`);
  }
  return lines.join('\n');
}

function buildActionSuccessMessage(action, response, context = {}) {
  const { instanceId, sessionId, serverId } = context;
  const message = response?.message;

  switch (action) {
    case 'reset-instance':
      return message || `实例 ${instanceId} 已触发重置`;
    case 'restart-runtime':
      return message || `实例 ${instanceId} 运行时重启完成`;
    case 'disable-instance':
      return message || `实例 ${instanceId} 已摘除`;
    case 'enable-instance':
      return message || `实例 ${instanceId} 已恢复调度`;
    case 'remove-instance':
      return message || `实例 ${instanceId} 已从调度层移除`;
    case 'cancel-reserve':
      return `会话 ${sessionId} 已取消预留`;
    case 'release-session':
      return `会话 ${sessionId} 已释放实例`;
    case 'disable-server':
      return message || `服务器 ${serverId} 已摘除`;
    case 'enable-server':
      return message || `服务器 ${serverId} 已恢复调度`;
    default:
      return message || '操作完成';
  }
}

function getRowLevel(id, anomaliesByType) {
  const keys = anomaliesByType.get(id) || [];
  return keys.some(item => item.severity === 'critical') ? 'row-critical'
    : keys.some(item => item.severity === 'warning') ? 'row-warning'
    : '';
}

function renderOverview(overview) {
  const cards = [
    ['实例总数', overview.totalInstances],
    ['空闲实例', overview.idleInstances],
    ['预留实例', overview.reservedInstances],
    ['忙碌实例', overview.busyInstances],
    ['用户保留', overview.reservedForUserInstances],
    ['异常实例', overview.badInstances],
    ['探针中实例', overview.probingInstances],
    ['活跃会话', overview.activeSessions],
    ['在线设备', overview.onlineDevices],
    ['异常数', overview.anomalyCount],
  ];

  elements.overviewCards.innerHTML = cards.map(([label, value]) => `
    <div class="card">
      <div class="card-label">${escapeHtml(label)}</div>
      <div class="card-value">${escapeHtml(value)}</div>
    </div>
  `).join('');
}

function renderServers(servers) {
  elements.serversCount.textContent = `共 ${servers.length} 条`;

  if (servers.length === 0) {
    elements.serversBody.innerHTML = '<tr><td class="empty" colspan="7">暂无服务器数据</td></tr>';
    return;
  }

  elements.serversBody.innerHTML = servers.map(item => `
    <tr class="${item.disabled ? 'row-warning' : ''}">
      <td>${escapeHtml(item.serverId)}<div class="subtle">${escapeHtml(item.serverIp || '-')} / ${escapeHtml(item.publicWsHost || '-')}</div></td>
      <td>${renderStatus(item.disabled ? 'disabled' : 'enabled')}</td>
      <td>
        total=${escapeHtml(item.totalInstances)}
        <div class="subtle">idle=${escapeHtml(item.idleInstances)} bad=${escapeHtml(item.badInstances)} reserved=${escapeHtml(item.reservedInstances)}</div>
      </td>
      <td>${formatPercent(item.cpu)}</td>
      <td>${formatPercent(item.memory)}</td>
      <td>${formatDuration(item.healthAgeMs)}<div class="subtle">${formatTimestamp(item.lastHealthAt)}</div></td>
      <td>
        <button class="table-action" data-action="${item.disabled ? 'enable-server' : 'disable-server'}" data-server-id="${escapeHtml(item.serverId)}">
          ${item.disabled ? '恢复' : '摘除'}
        </button>
      </td>
    </tr>
  `).join('');
}

function renderInstances(instances, anomalies) {
  const anomaliesByInstanceId = new Map();
  anomalies.forEach(item => {
    if (!item.instanceId) return;
    const list = anomaliesByInstanceId.get(item.instanceId) || [];
    list.push(item);
    anomaliesByInstanceId.set(item.instanceId, list);
  });

  const filtered = elements.onlyAnomalies.checked
    ? instances.filter(item => anomaliesByInstanceId.has(item.id))
    : instances;

  elements.instancesCount.textContent = `共 ${filtered.length} 条`;

  if (filtered.length === 0) {
    elements.instancesBody.innerHTML = '<tr><td class="empty" colspan="11">暂无实例数据</td></tr>';
    return;
  }

  elements.instancesBody.innerHTML = filtered.map(item => `
    <tr class="${getRowLevel(item.id, anomaliesByInstanceId)}">
      <td>${escapeHtml(item.id)}<div class="subtle">${escapeHtml(item.serverId)} / ${escapeHtml(item.wsPort)}</div></td>
      <td>${renderStatus(item.status)}${item.serverDisabled ? '<div class="subtle">server disabled</div>' : ''}${item.disabled ? '<div class="subtle">instance disabled</div>' : ''}</td>
      <td>${renderStatus(item.agentStatus || '-')}</td>
      <td>${escapeHtml(item.sessionId || '-')}</td>
      <td>${renderStatus(item.health || '-')}</td>
      <td>${formatPercent(item.cpu)}</td>
      <td>${formatPercent(item.memory)}</td>
      <td>${formatDuration(item.healthAgeMs)}<div class="subtle">${formatTimestamp(item.lastHealthAt)}</div></td>
      <td>${item.reservedForDeviceId ? `${escapeHtml(item.reservedForDeviceId)}<div class="subtle">${formatDuration(item.reservedRemainingMs)}</div>` : '-'}</td>
      <td>
        <button class="table-action" data-action="reset-instance" data-instance-id="${escapeHtml(item.id)}">重置</button>
        <button class="table-action" data-action="restart-runtime" data-instance-id="${escapeHtml(item.id)}">重启运行时</button>
        <button class="table-action" data-action="${item.disabled ? 'enable-instance' : 'disable-instance'}" data-instance-id="${escapeHtml(item.id)}">${item.disabled ? '恢复' : '摘除'}</button>
        <button class="table-action" data-action="show-logs" data-instance-id="${escapeHtml(item.id)}">日志</button>
        <button class="table-action danger" data-action="remove-instance" data-instance-id="${escapeHtml(item.id)}">移除</button>
      </td>
      <td title="${escapeHtml(item.lastError || '')}">${escapeHtml(item.lastError || '-')}</td>
    </tr>
  `).join('');
}

function renderSessions(sessions, anomalies) {
  const anomaliesBySessionId = new Map();
  anomalies.forEach(item => {
    if (!item.sessionId) return;
    const list = anomaliesBySessionId.get(item.sessionId) || [];
    list.push(item);
    anomaliesBySessionId.set(item.sessionId, list);
  });

  const search = elements.sessionSearch.value.trim().toLowerCase();
  const filtered = sessions.filter(item => {
    const matchSearch = !search || [
      item.sessionId,
      item.deviceId,
      item.carBrand,
      item.carModel,
      item.instanceId,
    ].some(value => String(value || '').toLowerCase().includes(search));

    const matchAnomaly = !elements.onlyAnomalies.checked || anomaliesBySessionId.has(item.sessionId);
    return matchSearch && matchAnomaly;
  });

  elements.sessionsCount.textContent = `共 ${filtered.length} 条`;

  if (filtered.length === 0) {
    elements.sessionsBody.innerHTML = '<tr><td class="empty" colspan="9">暂无会话数据</td></tr>';
    return;
  }

  elements.sessionsBody.innerHTML = filtered.map(item => `
    <tr class="${getRowLevel(item.sessionId, anomaliesBySessionId)}">
      <td>${escapeHtml(item.sessionId)}</td>
      <td>${escapeHtml(item.deviceId)}</td>
      <td>${renderStatus(item.status)}</td>
      <td>${escapeHtml(item.instanceId || '-')}</td>
      <td>${escapeHtml(item.carBrand || '-')} ${escapeHtml(item.carModel || '')}</td>
      <td>${formatDuration(item.durationMs)}</td>
      <td>${formatDuration(item.heartbeatAgeMs)}<div class="subtle">${formatTimestamp(item.lastHeartbeatAt || item.createdAt)}</div></td>
      <td>${item.instanceStatus ? renderStatus(item.instanceStatus) : '-'}</td>
      <td>
        ${item.status === 'reserved'
          ? `<button class="table-action" data-action="cancel-reserve" data-session-id="${escapeHtml(item.sessionId)}">取消预留</button>`
          : ''}
        ${item.instanceId
          ? `<button class="table-action danger" data-action="release-session" data-session-id="${escapeHtml(item.sessionId)}">释放实例</button>`
          : ''}
      </td>
    </tr>
  `).join('');
}

function renderAnomalies(anomalies) {
  elements.anomaliesCount.textContent = `共 ${anomalies.length} 条`;

  if (anomalies.length === 0) {
    elements.anomaliesBody.innerHTML = '<tr><td class="empty" colspan="7">暂无异常</td></tr>';
    return;
  }

  elements.anomaliesBody.innerHTML = anomalies.map(item => `
    <tr class="${item.severity === 'critical' ? 'row-critical' : 'row-warning'}">
      <td class="severity-${escapeHtml(item.severity)}">${escapeHtml(item.severity)}</td>
      <td>${escapeHtml(item.type)}</td>
      <td>${escapeHtml(item.instanceId || '-')}</td>
      <td>${escapeHtml(item.sessionId || '-')}</td>
      <td>${escapeHtml(item.deviceId || '-')}</td>
      <td>${escapeHtml(item.message)}</td>
      <td>${formatDuration(item.ageMs)}</td>
    </tr>
  `).join('');
}

async function loadAll() {
  try {
    const [overviewRes, serversRes, instancesRes, sessionsRes, anomaliesRes] = await Promise.all([
      fetch('/dashboard/api/overview'),
      fetch('/dashboard/api/servers'),
      fetch('/dashboard/api/instances'),
      fetch('/dashboard/api/sessions'),
      fetch('/dashboard/api/anomalies'),
    ]);

    const [overviewJson, serversJson, instancesJson, sessionsJson, anomaliesJson] = await Promise.all([
      overviewRes.json(),
      serversRes.json(),
      instancesRes.json(),
      sessionsRes.json(),
      anomaliesRes.json(),
    ]);

    if (!overviewJson.success) throw new Error(overviewJson.error || 'overview 加载失败');
    if (!serversJson.success) throw new Error(serversJson.error || 'servers 加载失败');
    if (!instancesJson.success) throw new Error(instancesJson.error || 'instances 加载失败');
    if (!sessionsJson.success) throw new Error(sessionsJson.error || 'sessions 加载失败');
    if (!anomaliesJson.success) throw new Error(anomaliesJson.error || 'anomalies 加载失败');

    latestState = {
      overview: overviewJson.data,
      servers: serversJson.data,
      instances: instancesJson.data,
      sessions: sessionsJson.data,
      anomalies: anomaliesJson.data,
    };

    renderOverview(latestState.overview);
    renderServers(latestState.servers);
    renderInstances(latestState.instances, latestState.anomalies);
    renderSessions(latestState.sessions, latestState.anomalies);
    renderAnomalies(latestState.anomalies);
    elements.lastUpdated.textContent = `最近刷新：${new Date().toLocaleString('zh-CN', { hour12: false })}`;
  } catch (error) {
    elements.lastUpdated.textContent = `加载失败：${error instanceof Error ? error.message : String(error)}`;
  }
}

async function postAction(url, method = 'POST') {
  const res = await fetch(url, { method });
  const text = await res.text();

  let json = null;
  try {
    json = text ? JSON.parse(text) : {};
  } catch {
    throw new Error(`HTTP ${res.status}: ${text || '响应不是合法 JSON'}`);
  }

  if (!res.ok || !json.success) {
    throw new Error(json?.error || `HTTP ${res.status}`);
  }
  return json;
}

async function showInstanceLogs(instanceId) {
  const res = await fetch(`/dashboard/api/admin/instances/${encodeURIComponent(instanceId)}/logs`);
  const text = await res.text();
  let json = null;
  try {
    json = text ? JSON.parse(text) : {};
  } catch {
    throw new Error(`HTTP ${res.status}: ${text || '日志响应不是合法 JSON'}`);
  }
  if (!res.ok || !json.success) {
    throw new Error(json.error || `HTTP ${res.status}`);
  }

  const payload = json.data;
  const sections = [
    `=== 实例 ===\n${JSON.stringify(payload.instance, null, 2)}`,
    `=== Scheduler ===\n${payload.scheduler?.content || '-'}`,
    ...((payload.runtime?.sections || []).map(item => `=== ${item.title} ===\n${item.content || '-'}`)),
  ];

  elements.logsOutput.textContent = sections.join('\n\n');
  elements.logsPanel.hidden = false;
}

async function handleTableAction(event) {
  const button = event.target.closest('button[data-action]');
  if (!button) return;

  const { action, instanceId, sessionId, serverId } = button.dataset;
  const originalText = button.textContent;
  button.disabled = true;

  try {
    setActionFeedback(`正在执行：${originalText || action}`, 'info');
    let response = null;

    if (action === 'reset-instance') {
      response = await postAction(`/dashboard/api/admin/instances/${encodeURIComponent(instanceId)}/reset`);
    } else if (action === 'restart-runtime') {
      response = await postAction(`/dashboard/api/admin/instances/${encodeURIComponent(instanceId)}/restart-runtime`);
    } else if (action === 'disable-instance') {
      if (!window.confirm(`确认摘除实例 ${instanceId} 吗？`)) return;
      response = await postAction(`/dashboard/api/admin/instances/${encodeURIComponent(instanceId)}/disable`);
    } else if (action === 'enable-instance') {
      response = await postAction(`/dashboard/api/admin/instances/${encodeURIComponent(instanceId)}/enable`);
    } else if (action === 'remove-instance') {
      if (!window.confirm(`确认移除实例 ${instanceId} 吗？`)) return;
      response = await postAction(`/dashboard/api/admin/instances/${encodeURIComponent(instanceId)}`, 'DELETE');
    } else if (action === 'show-logs') {
      await showInstanceLogs(instanceId);
      setActionFeedback(`实例 ${instanceId} 的日志已加载`, 'success');
      return;
    } else if (action === 'cancel-reserve') {
      response = await postAction(`/session/${encodeURIComponent(sessionId)}/cancel-reserve`);
    } else if (action === 'release-session') {
      if (!window.confirm(`确认释放会话 ${sessionId} 绑定的实例吗？`)) return;
      response = await postAction(`/session/${encodeURIComponent(sessionId)}/release`);
    } else if (action === 'disable-server') {
      if (!window.confirm(`确认摘除服务器 ${serverId} 吗？`)) return;
      response = await postAction(`/dashboard/api/admin/servers/${encodeURIComponent(serverId)}/disable`);
    } else if (action === 'enable-server') {
      response = await postAction(`/dashboard/api/admin/servers/${encodeURIComponent(serverId)}/enable`);
    } else {
      return;
    }

    await loadAll();
    setActionFeedback(
      buildActionSuccessMessage(action, response, { instanceId, sessionId, serverId }),
      'success'
    );
  } catch (error) {
    const message = error instanceof Error ? error.message : String(error);
    setActionFeedback(`操作失败：${message}`, 'error');
    elements.lastUpdated.textContent = `操作失败：${message}`;
  } finally {
    button.disabled = false;
  }
}

function restartAutoRefresh() {
  if (autoRefreshTimer) {
    clearInterval(autoRefreshTimer);
    autoRefreshTimer = null;
  }

  if (elements.autoRefresh.checked) {
    autoRefreshTimer = setInterval(loadAll, AUTO_REFRESH_MS);
  }
}

elements.refreshBtn.addEventListener('click', loadAll);
elements.autoRefresh.addEventListener('change', restartAutoRefresh);
elements.repairBadBtn.addEventListener('click', async () => {
  const originalText = elements.repairBadBtn.textContent;
  elements.repairBadBtn.disabled = true;
  try {
    setActionFeedback('正在执行：批量修复 bad', 'info');
    const response = await postAction('/dashboard/api/admin/instances/restart-bad');
    await loadAll();
    setActionFeedback(summarizeBatchRepairResult(response), 'success');
  } catch (error) {
    const message = error instanceof Error ? error.message : String(error);
    setActionFeedback(`批量修复失败：${message}`, 'error');
    elements.lastUpdated.textContent = `批量修复失败：${message}`;
  } finally {
    elements.repairBadBtn.disabled = false;
    elements.repairBadBtn.textContent = originalText;
  }
});
elements.closeLogsBtn.addEventListener('click', () => {
  elements.logsPanel.hidden = true;
  elements.logsOutput.textContent = '';
});
elements.onlyAnomalies.addEventListener('change', () => {
  renderServers(latestState.servers);
  renderInstances(latestState.instances, latestState.anomalies);
  renderSessions(latestState.sessions, latestState.anomalies);
});
elements.sessionSearch.addEventListener('input', () => {
  renderSessions(latestState.sessions, latestState.anomalies);
});
document.body.addEventListener('click', handleTableAction);

restartAutoRefresh();
loadAll();
