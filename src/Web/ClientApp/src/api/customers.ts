import type { CustomerDto } from '../types'

const BASE = '/api/customers'

export async function getCustomers(): Promise<CustomerDto[]> {
  const res = await fetch(BASE)
  if (!res.ok) throw new Error(await res.text())
  return res.json()
}

export async function createCustomer(name: string, abbreviation?: string): Promise<number> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, abbreviation })
  })
  if (!res.ok) throw new Error(await res.text())
  const data = await res.json()
  return data.id
}

export async function updateCustomer(id: number, name: string, abbreviation?: string): Promise<void> {
  const res = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ id, name, abbreviation })
  })
  if (!res.ok) throw new Error(await res.text())
}
