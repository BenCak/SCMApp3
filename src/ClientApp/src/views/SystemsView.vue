<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useCustomersStore } from '../stores/customers'
import { useProductsStore } from '../stores/products'
import type { SystemDto } from '../types'
import { getSystems, createSystem, updateSystem } from '../api/systems'
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
const editMode = ref(false)
const form = ref({
  id: 0,
  customerId: null as number | null,
  productId: null as number | null,
  pocName: '',
  pocEmail: '',
  pocPhone: ''
})
const errors = ref({ customerId: '', productId: '' })

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

function openCreate() {
  form.value = { id: 0, customerId: null, productId: null, pocName: '', pocEmail: '', pocPhone: '' }
  editMode.value = false
  errors.value = { customerId: '', productId: '' }
  showDialog.value = true
}

function openEdit(row: SystemDto) {
  form.value = {
    id: row.id,
    customerId: row.customerId,
    productId: row.productId,
    pocName: row.pocName ?? '',
    pocEmail: row.pocEmail ?? '',
    pocPhone: row.pocPhone ?? ''
  }
  editMode.value = true
  errors.value = { customerId: '', productId: '' }
  showDialog.value = true
}

function validate(): boolean {
  errors.value = { customerId: '', productId: '' }
  if (!form.value.customerId) errors.value.customerId = 'Customer is required.'
  if (!form.value.productId) errors.value.productId = 'Product is required.'
  return !errors.value.customerId && !errors.value.productId
}

async function save() {
  if (!validate()) return
  const payload = {
    customerId: form.value.customerId!,
    productId: form.value.productId!,
    pocName: form.value.pocName || undefined,
    pocEmail: form.value.pocEmail || undefined,
    pocPhone: form.value.pocPhone || undefined,
  }
  try {
    if (editMode.value) {
      await updateSystem(form.value.id, payload)
      toast.add({ severity: 'success', summary: 'Updated', life: 3000 })
    } else {
      await createSystem(payload)
      toast.add({ severity: 'success', summary: 'System created', life: 3000 })
    }
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
      <Button label="New System" icon="pi pi-plus" @click="openCreate" />
    </div>

    <DataTable :value="systems" :loading="loading" stripedRows>
      <Column field="id" header="ID" style="width: 80px" />
      <Column field="customerName" header="Customer" />
      <Column field="productName" header="Product" />
      <Column field="pocName" header="POC Name" />
      <Column field="pocEmail" header="POC Email" />
      <Column header="Actions" style="width: 80px">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" text @click="openEdit(data)" />
        </template>
      </Column>
    </DataTable>

    <Dialog v-model:visible="showDialog" :header="editMode ? 'Edit System' : 'New System'" modal style="width: 440px">
      <div class="flex flex-col gap-3 pt-2">
        <div class="flex flex-col gap-1">
          <label class="font-medium">Customer <span style="color: var(--p-red-500)">*</span></label>
          <Select v-model="form.customerId" :options="customersStore.customers" optionLabel="name" optionValue="id"
            placeholder="Select customer" :invalid="!!errors.customerId" />
          <small v-if="errors.customerId" style="color: var(--p-red-500)">{{ errors.customerId }}</small>
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">Product <span style="color: var(--p-red-500)">*</span></label>
          <Select v-model="form.productId" :options="productsStore.products" optionLabel="name" optionValue="id"
            placeholder="Select product" :invalid="!!errors.productId" />
          <small v-if="errors.productId" style="color: var(--p-red-500)">{{ errors.productId }}</small>
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
