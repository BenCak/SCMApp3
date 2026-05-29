<script setup lang="ts">
import { RouterView } from 'vue-router'
import Menubar from 'primevue/menubar'
import Toast from 'primevue/toast'
import { ref } from 'vue'

const menuItems = ref([
  { label: 'Customers', icon: 'pi pi-building', route: '/customers' },
  { label: 'Products', icon: 'pi pi-box', route: '/products' },
  { label: 'Systems', icon: 'pi pi-sitemap', route: '/systems' },
])
</script>

<template>
  <div class="min-h-screen surface-ground">
    <Menubar :model="menuItems">
      <template #start>
        <span class="font-bold text-lg mr-4">SCMApp</span>
      </template>
    </Menubar>
    <main class="p-4">
      <RouterView />
    </main>
  </div>
  <Toast />
</template>
