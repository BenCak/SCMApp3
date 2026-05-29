import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { CustomerDto } from '../types'
import { getCustomers, createCustomer, updateCustomer } from '../api/customers'

export const useCustomersStore = defineStore('customers', () => {
  const customers = ref<CustomerDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      customers.value = await getCustomers()
    } catch (e) {
      error.value = String(e)
    } finally {
      loading.value = false
    }
  }

  async function create(name: string, abbreviation?: string) {
    await createCustomer(name, abbreviation)
    await fetchAll()
  }

  async function update(id: number, name: string, abbreviation?: string) {
    await updateCustomer(id, name, abbreviation)
    await fetchAll()
  }

  return { customers, loading, error, fetchAll, create, update }
})
