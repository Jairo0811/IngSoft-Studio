export const API_URL = import.meta.env.VITE_API_URL ?? ''
export const TOKEN_KEY = 'ingsoftstudio.accessToken'

export class ApiError extends Error {
  constructor(
    public readonly status: number,
    message: string,
    public readonly details?: unknown,
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

async function parseError(response: Response) {
  const text = await response.text()
  if (!text) return { message: `Request failed with status ${response.status}`, details: undefined }

  try {
    const details = JSON.parse(text) as { title?: string; detail?: string; errors?: Record<string, string[]> }
    const validationMessage = details.errors
      ? Object.values(details.errors).flat().join(' ')
      : undefined
    return {
      message: validationMessage || details.detail || details.title || `Request failed with status ${response.status}`,
      details,
    }
  } catch {
    return { message: text, details: text }
  }
}

export async function apiRequest<T>(path: string, init?: RequestInit): Promise<T> {
  const token = sessionStorage.getItem(TOKEN_KEY)
  const headers = new Headers(init?.headers)

  if (init?.body && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  if (token) {
    headers.set('Authorization', `Bearer ${token}`)
  }

  const response = await fetch(buildApiUrl(path), {
    ...init,
    headers,
  })

  if (!response.ok) {
    const { message, details } = await parseError(response)
    if (response.status === 401 && token) {
      sessionStorage.removeItem(TOKEN_KEY)
      if (window.location.pathname !== '/auth') window.location.assign('/auth?expired=1')
    }
    throw new ApiError(response.status, message, details)
  }

  if (response.status === 204) return undefined as T
  const text = await response.text()
  return (text ? JSON.parse(text) : undefined) as T
}

export function buildApiUrl(path: string): string {
  if (!/^\/api(?:\/|$)/.test(path)) {
    throw new Error('API requests must use an internal /api path.')
  }

  const applicationOrigin = window.location.origin
  const configuredBase = new URL(API_URL || applicationOrigin, applicationOrigin)
  const url = new URL(path, configuredBase.origin)

  if (url.origin !== configuredBase.origin) {
    throw new Error('Refusing to send credentials outside the configured API origin.')
  }

  return url.toString()
}
