export const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL || 'http://localhost:5205').replace(/\/+$/, '');

async function readJson(response) {
  try {
    return await response.json();
  } catch {
    return null;
  }
}

async function request(path, signal) {
  let response;

  try {
    response = await fetch(`${apiBaseUrl}${path}`, { signal, headers: { Accept: 'application/json' } });
  } catch (error) {
    if (error.name === 'AbortError') throw error;
    throw new Error(`Cannot reach the API at ${apiBaseUrl}. Start the backend and try again.`);
  }

  const body = await readJson(response);

  if (!response.ok) {
    const detail = body?.detail || body?.title || `The API answered with status ${response.status}.`;
    throw new Error(detail);
  }

  return body;
}

function buildQuery(filters) {
  const params = new URLSearchParams();
  params.set('page', String(filters.page));
  params.set('pageSize', String(filters.pageSize));
  if (filters.participantName?.trim()) params.set('participantName', filters.participantName.trim());
  if (filters.type) params.set('type', filters.type);
  if (filters.status) params.set('status', filters.status);
  return params.toString();
}

export function getRegistrations(filters, signal) {
  return request(`/api/registrations?${buildQuery(filters)}`, signal);
}

export function getSummary(signal) {
  return request('/api/registrations/summary', signal);
}
