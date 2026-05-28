<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useCustomersStore } from '../stores/customers'
import { useProductsStore } from '../stores/products'
import type { SystemDto } from '../types'
import { getSystems, createSystem } from '../api/systems'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import Select from 'primevue/select'
import InputText from 'primevue/inputtext'
import { useToast } from 'primevue/usetoast'

const customersStore = useCustomersStore()
const productsStore = useProductsStore()
const toast = useToast()

const systems = ref<SystemDto[]>([])
const loading = ref(false)
const showDialog = ref(false)
const form = ref({ customerId: null as number | null, productId: null as number | null, pocName: '', pocEmail: '', pocPhone: '' })

onMounted(async () => {
  await Promise.all([customersStore.fetchAll(), productsStore.fetchAll()])
  await loadSystems()
})

async function loadSystems() {
  loading.value = true
  try {
    systems.value = await getSystems()
  } finally {
    loading.value = false
  }
}

async function save() {
  if (!form.value.customerId || !form.value.productId) return
  try {
    await createSystem({
      customerId: form.value.customerId,
      productId: form.value.productId,
      pocName: form.value.pocName || undefined,
      pocEmail: form.value.pocEmail || undefined,
      pocPhone: form.value.pocPhone || undefined,
    })
    toast.add({ severity: 'success', summary: 'System created', life: 3000 })
    showDialog.value = false
    await loadSystems()
  } catch (e) {
    toast.add({ severity: 'error', summary: 'Error', detail: String(e), life: 5000 })
  }
}
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">Systems</h1>
      <Button label="New System" icon="pi pi-plus" @click="showDialog = true" />
    </div>

    <DataTable :value="systems" :loading="loading" stripedRows>
      <Column field="id" header="ID" style="width: 80px" />
      <Column field="customerName" header="Customer" />
      <Column field="productName" header="Product" />
      <Column field="pocName" header="POC Name" />
      <Column field="pocEmail" header="POC Email" />
    </DataTable>

    <Dialog v-model:visible="showDialog" header="New System" modal style="width: 440px">
      <div class="flex flex-col gap-3 pt-2">
        <div class="flex flex-col gap-1">
          <label class="font-medium">Customer</label>
          <Select v-model="form.customerId" :options="customersStore.customers" optionLabel="name" optionValue="id" placeholder="Select customer" />
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">Product</label>
          <Select v-model="form.productId" :options="productsStore.products" optionLabel="name" optionValue="id" placeholder="Select product" />
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">POC Name</label>
          <InputText v-model="form.pocName" />
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">POC Email</label>
          <InputText v-model="form.pocEmail" />
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">POC Phone</label>
          <InputText v-model="form.pocPhone" />
        </div>
      </div>
      <template #footer>
        <Button label="Cancel" text @click="showDialog = false" />
        <Button label="Save" @click="save" />
      </template>
    </Dialog>
  </div>
</template>
