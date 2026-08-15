export const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5000'
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
  const token = localStorage.getItem(TOKEN_KEY)
  const response = await fetch(`${API_URL}${path}`, {
    ...init,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...init?.headers,
    },
  })

  if (!response.ok) {
    const { message, details } = await parseError(response)
    if (response.status === 401 && token) {
      localStorage.removeItem(TOKEN_KEY)
      window.dispatchEvent(new CustomEvent('ingsoftstudio:session-expired'))
    }
    throw new ApiError(response.status, message, details)
  }

  if (response.status === 204) return undefined as T
  const text = await response.text()
  return (text ? JSON.parse(text) : undefined) as T
}
