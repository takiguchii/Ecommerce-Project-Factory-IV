import axios from 'axios';


const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config; 
  },
  (error) => {
    return Promise.reject(error);
  }
);

export const apiGet = async (url) => {
  const response = await api.get(url);
  return response.data;
};

export const apiPost = async (url, data) => {
  const response = await api.post(url, data);
  return response.data;
};

export const apiPut = async (url, data) => {
  const response = await api.put(url, data);
  return response.data;
};

export const apiDelete = async (url) => {
  const response = await api.delete(url);
  return response.data;
};

// Funções específicas (mantidas)
function normalizeBrand(raw) {
  if (!raw) return null;
  return {
    id: raw.id ?? raw.ID ?? null,
    name: raw.name ?? raw.Nome ?? '',
    imageUrl: raw.imageUrl ?? raw.imageurl ?? raw.logoUrl ?? raw.logoURL ?? '',
  };
}

export async function getProductById(id) {
  return await apiGet(`/products/${id}`);
}

export async function getBrands() {
  const list = await apiGet('/brands');
  return Array.isArray(list) ? list.map(normalizeBrand) : [];
}

export async function getBrandById(id) {
  try {
    const brand = await apiGet(`/brands/${id}`);
    return normalizeBrand(brand);
  } catch (_) {
    const list = await getBrands();
    return list.find(b => String(b?.id) === String(id)) || null;
  }
}

const brandCache = new Map();
export async function getBrandCached(id) {
  const key = String(id);
  if (brandCache.has(key)) return brandCache.get(key);
  const data = await getBrandById(key);
  brandCache.set(key, data);
  return data;
}


export function login(credentials) {
  return api.post('/auth/login', credentials);
}

export function register(userInfo) {
  return api.post('/auth/register', userInfo);
}

export function createBrand(brandData) {
  return api.post('/brands', brandData);
}

export function deleteBrand(brandId) {
  return api.delete(`/brands/${brandId}`);
}

export function updateBrand(brandId, brandData) {
  return api.put(`/brands/${brandId}`, brandData);
}

export default api;