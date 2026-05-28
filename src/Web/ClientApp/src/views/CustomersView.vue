<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useCustomersStore } from '../stores/customers'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import { useToast } from 'primevue/usetoast'

const store = useCustomersStore()
const toast = useToast()

const showDialog = ref(false)
const editMode = ref(false)
const form = ref({ id: 0, name: '', abbreviation: '' })

onMounted(() => store.fetchAll())

function openCreate() {
  form.value = { id: 0, name: '', abbreviation: '' }
  editMode.value = false
  showDialog.value = true
}

function openEdit(row: { id: number; name: string; abbreviation?: string }) {
  form.value = { id: row.id, name: row.name, abbreviation: row.abbreviation ?? '' }
  editMode.value = true
  showDialog.value = true
}

async function save() {
  try {
    if (editMode.value) {
      await store.update(form.value.id, form.value.name, form.value.abbreviation || undefined)
      toast.add({ severity: 'success', summary: 'Updated', life: 3000 })
    } else {
      await store.create(form.value.name, form.value.abbreviation || undefined)
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
      <h1 class="text-2xl font-bold">Customers</h1>
      <Button label="New Customer" icon="pi pi-plus" @click="openCreate" />
    </div>

    <DataTable :value="store.customers" :loading="store.loading" stripedRows>
      <Column field="id" header="ID" style="width: 80px" />
      <Column field="name" header="Name" />
      <Column field="abbreviation" header="Abbreviation" />
      <Column header="Actions" style="width: 120px">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" text @click="openEdit(data)" />
        </template>
      </Column>
    </DataTable>

    <Dialog v-model:visible="showDialog" :header="editMode ? 'Edit Customer' : 'New Customer'" modal style="width: 400px">
      <div class="flex flex-col gap-3 pt-2">
        <div class="flex flex-col gap-1">
          <label class="font-medium">Name</label>
          <InputText v-model="form.name" placeholder="e.g. US Army" />
        </div>
        <div class="flex flex-col gap-1">
          <label class="font-medium">Abbreviation</label>
          <InputText v-model="form.abbreviation" placeholder="e.g. USA" />
        </div>
      </div>
      <template #footer>
        <Button label="Cancel" text @click="showDialog = false" />
        <Button label="Save" @click="save" />
      </template>
    </Dialog>
  </div>
</template>
