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
const form = ref({ name: '', description: '' })

onMounted(() => store.fetchAll())

function openCreate() {
  form.value = { name: '', description: '' }
  showDialog.value = true
}

async function save() {
  try {
    await store.create(form.value.name, form.value.description || undefined)
    toast.add({ severity: 'success', summary: 'Created', life: 3000 })
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
    </DataTable>

    <Dialog v-model:visible="showDialog" header="New Product" modal style="width: 400px">
      <div class="flex flex-col gap-3 pt-2">
        <div class="flex flex-col gap-1">
          <label class="font-medium">Name</label>
          <InputText v-model="form.name" placeholder="e.g. Gray Eagle" />
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
