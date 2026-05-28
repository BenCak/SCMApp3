import type { SystemDto } from '../types'

const BASE = '/api/systems'

export async function getSystems(customerId?: number, productId?: number): Promise<SystemDto[]> {
  const params = new URLSearchParams()
  if (customerId) params.set('customerId', String(customerId))
  if (productId) params.set('productId', String(productId))
  const res = await fetch(`${BASE}?${params}`)
  if (!res.ok) throw new Error(await res.text())
  return res.json()
}

export async function createSystem(payload: {
  customerId: number
  productId: number
  pocName?: string
  pocEmail?: string
  pocPhone?: string
}): Promise<number> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  })
  if (!res.ok) throw new Error(await res.text())
  const data = await res.json()
  return data.id
}
