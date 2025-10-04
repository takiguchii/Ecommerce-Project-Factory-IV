import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// API methods
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

api.interceptors.request.use(config => {
  console.log('API Request:', config.method.toUpperCase(), config.url);
  return config;
});

api.interceptors.response.use(
  response => {
    console.log('API Response:', response.status, response.data);
    return response;
  },
  error => {
    console.error('API Error:', error.response?.status, error.response?.data);
    return Promise.reject(error);
  }
);

export async function getProductById(id) {
  return await apiGet(`/products/${id}`);
}

export default api;