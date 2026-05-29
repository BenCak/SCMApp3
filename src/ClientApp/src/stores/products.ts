import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { ProductDto } from '../types'
import { getProducts, createProduct, updateProduct } from '../api/products'

export const useProductsStore = defineStore('products', () => {
  const products = ref<ProductDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      products.value = await getProducts()
    } catch (e) {
      error.value = String(e)
    } finally {
      loading.value = false
    }
  }

  async function create(name: string, description?: string) {
    await createProduct(name, description)
    await fetchAll()
  }

  async function update(id: number, name: string, description?: string) {
    await updateProduct(id, name, description)
    await fetchAll()
  }

  return { products, loading, error, fetchAll, create, update }
})
