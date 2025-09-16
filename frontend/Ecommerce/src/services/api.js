export const API_URL = import.meta.env.VITE_API_URL?.replace(/\/+$/, '') || '';

export async function apiGet(path) {
  const res = await fetch(`${API_URL}${path}`, {
    headers: { 'Accept': 'application/json' },
  });
  if (!res.ok) {
    const text = await res.text().catch(() => '');
    throw new Error(`GET ${path} falhou: ${res.status} ${text}`);
  }
  return res.json();
}
