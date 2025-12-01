import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
})

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken')
    if (token) config.headers['Authorization'] = `Bearer ${token}`
    return config
  },
  (error) => Promise.reject(error)
)

export const apiGet = async (url) => (await api.get(url)).data
export const apiPost = async (url, data) => (await api.post(url, data)).data
export const apiPut = async (url, data) => (await api.put(url, data)).data
export const apiDelete = async (url) => (await api.delete(url)).data


function normalizeBrand(raw) {
  if (!raw) return null
  return {
    id: raw.id ?? raw.ID ?? null,
    name: raw.name ?? raw.Nome ?? '',
    imageUrl: raw.imageUrl ?? raw.imageurl ?? raw.logoUrl ?? raw.logoURL ?? ''
  }
}

export async function getBrands() {
  const list = await apiGet('/brands')
  return Array.isArray(list) ? list.map(normalizeBrand) : []
}

export async function getBrandById(id) {
  try {
    const brand = await apiGet(`/brands/${id}`)
    return normalizeBrand(brand)
  } catch {
    const list = await getBrands()
    return list.find(b => String(b?.id) === String(id)) || null
  }
}

const brandCache = new Map()
export async function getBrandCached(id) {
  const key = String(id)
  if (brandCache.has(key)) return brandCache.get(key)
  const data = await getBrandById(key)
  brandCache.set(key, data)
  return data
}

export function createBrand(brandData) {
  return api.post('/brands', brandData)
}

export function updateBrand(brandId, brandData) {
  return api.put(`/brands/${brandId}`, brandData)
}

export function deleteBrand(brandId) {
  return api.delete(`/brands/${brandId}`)
}


function normalizeCategory(raw) {
  if (!raw) return null
  return {
    id: raw.id ?? raw.ID ?? null,
    name: raw.name ?? raw.Nome ?? '',
    products: Array.isArray(raw.products) ? raw.products : []
  }
}

export async function getCategories() {
  const list = await apiGet('/categories')
  return Array.isArray(list) ? list.map(normalizeCategory) : []
}

export async function getCategoryById(id) {
  try {
    const cat = await apiGet(`/categories/${id}`)
    return normalizeCategory(cat)
  } catch {
    const list = await getCategories()
    return list.find(c => String(c?.id) === String(id)) || null
  }
}

export function createCategory(data) {
  return api.post('/categories', data)
}

export function updateCategory(id, data) {
  return api.put(`/categories/${id}`, data)
}

export function deleteCategory(id) {
  return api.delete(`/categories/${id}`)
}

export async function getProductById(id) {
  return await apiGet(`/products/${id}`)
}

export async function getPromotions() {
  return await apiGet('/products/promotions')
}

export async function getRandomProducts(categoryId = null, subCategoryId = null, brandId = null) {
  const params = {}
  if (categoryId) params.categoryId = categoryId
  if (subCategoryId) params.subCategoryId = subCategoryId
  if (brandId) params.brandId = brandId

  return (await api.get('/products/productsGridHomePage', { params })).data
}

export async function getSearchSuggestions(query) {
  return (await api.get('/products/search-suggestions', { params: { query } })).data
}


export async function getProductsByFilter(page, pageSize, categoryId, subCategoryId, brandId, sort) {
  const params = {
    pageNumber: page,
    pageSize: pageSize
  }

  if (categoryId) params.categoryId = categoryId
  if (subCategoryId) params.subCategoryId = subCategoryId
  if (brandId) params.brandId = brandId
  
  if (sort) params.sort = sort 

  return (await api.get('/products/filter', { params })).data
}

export async function getProductsByCategory(categoryId, subCategoryId, sort) {
  const params = {}
  if (subCategoryId) params.subCategoryId = subCategoryId
  if (sort) params.sort = sort

  return (await api.get(`/products/category/${categoryId}`, { params })).data
}


export function login(credentials) {
  return api.post('/auth/login', credentials)
}

export function register(userInfo) {
  return api.post('/auth/register', userInfo)
}

export function calculateShipping(cep) {
  return api.post('/shipping/calculate', { cep })
}

export function validateCoupon(code) {
  return api.get(`/coupon/validate/${code}`)
}

export default api