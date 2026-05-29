import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createRouter, createWebHistory } from 'vue-router'
import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura'
import ToastService from 'primevue/toastservice'
import App from './App.vue'
import CustomersView from './views/CustomersView.vue'
import ProductsView from './views/ProductsView.vue'
import SystemsView from './views/SystemsView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/customers' },
    { path: '/customers', component: CustomersView },
    { path: '/products', component: ProductsView },
    { path: '/systems', component: SystemsView },
  ]
})

const pinia = createPinia()

const app = createApp(App)
app.use(pinia)
app.use(router)
app.use(PrimeVue, { theme: { preset: Aura } })
app.use(ToastService)
app.mount('#app')
