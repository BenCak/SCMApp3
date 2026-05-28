import type { ProductDto } from '../types'

const BASE = '/api/products'

export async function getProducts(): Promise<ProductDto[]> {
  const res = await fetch(BASE)
  if (!res.ok) throw new Error(await res.text())
  return res.json()
}

export async function createProduct(name: string, description?: string): Promise<number> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, description })
  })
  if (!res.ok) throw new Error(await res.text())
  const data = await res.json()
  return data.id
}
