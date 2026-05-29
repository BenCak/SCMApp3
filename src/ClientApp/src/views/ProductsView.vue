<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useProductsStore } from '../stores/products'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Textarea from 'primevue/textarea'
import { useToast } from 'primevue/usetoast'

const store = useProductsStore()
const toast = useToast()

const showDialog = ref(false)
const editMode = ref(false)
const form = ref({ id: 0, name: '', description: '' })
const errors = ref({ name: '' })

onMounted(() => store.fetchAll())

function openCreate() {
  form.value = { id: 0, name: '', description: '' }
  editMode.value = false
  errors.value = { name: '' }
  showDialog.value = true
}

function openEdit(row: { id: number; name: string; description?: string }) {
  form.value = { id: row.id, name: row.name, description: row.description ?? '' }
  editMode.value = true
  errors.value = { name: '' }
  showDialog.value = true
}

function validate(): boolean {
  errors.value = { name: '' }
  if (!form.value.name.trim())
    errors.value.name = 'Name is required.'
  else if (form.value.name.length > 200)
    errors.value.name = 'Name must be 200 characters or fewer.'
  return !errors.value.name
}

async function save() {
  if (!validate()) return
  try {
    if (editMode.value) {
      await store.update(form.value.id, form.value.name, form.value.description || undefined)
      toast.add({ severity: 'success', summary: 'Updated', life: 3000 })
    } else {
      await store.create(form.value.name, form.value.description || undefined)
      toast.add({ severity: 'success', summary: 'Created', life: 3000 })
    }
    showDialog.value = false
  } catch (e) {
    toast.add({ severity: 'error', summary: 'Error', detail: String(e), life: 5000 })
  }
}
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">Products</h1>
      <Button label="New Product" icon="pi pi-plus" @click="openCreate" />
    </div>

    <DataTable :value="store.products" :loading="store.loading" stripedRows>
      <Column field="id" header="ID" style="width: 80px" />
      <Column field="name" header="Name" />
      <Column field="description" header="Description" />
      <Column header="Actions" style="width: 80px">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" text @click="openEdit(data)" />
        </template>
      </Column>
    </DataTable>

    <Dialog v-model:visible="showDialog" :header="editMode ? 'Edit Product' : 'New Product'" modal style="width: 400px">
      <div class="flex flex-col gap-3 pt-2">
        <div class="flex flex-col gap-1">
          <label class="font-medium">Name <span style="color: var(--p-red-500)">*</span></label>
          <InputText v-model="form.name" :invalid="!!errors.name" placeholder="e.g. Gray Eagle" />
          <small v-if="errors.name" style="color: var(--p-red-500)">{{ errors.name }}</small>
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">Description</label>
          <Textarea v-model="form.description" rows="3" />
        </div>
      </div>
      <template #footer>
        <Button label="Cancel" text @click="showDialog = false" />
        <Button label="Save" @click="save" />
      </template>
    </Dialog>
  </div>
</template>
