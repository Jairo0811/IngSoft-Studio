export type User = {
  id: string
  fullName: string
  email: string
  roles: string[]
}

export type AuthResponse = {
  accessToken: string
  expiresAtUtc: string
  user: User
}

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5000'
const TOKEN_KEY = 'ingsoftstudio.accessToken'

async function request<T>(path: string, init?: RequestInit): Promise<T> {
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
    const detail = await response.text()
    throw new Error(detail || `Request failed with status ${response.status}`)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return response.json() as Promise<T>
}

export const authService = {
  async login(email: string, password: string) {
    const result = await request<AuthResponse>('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    })
    localStorage.setItem(TOKEN_KEY, result.accessToken)
    return result
  },

  async register(fullName: string, email: string, password: string) {
    const result = await request<AuthResponse>('/api/auth/register', {
      method: 'POST',
      body: JSON.stringify({ fullName, email, password }),
    })
    localStorage.setItem(TOKEN_KEY, result.accessToken)
    return result
  },

  me: () => request<User>('/api/auth/me'),

  updateProfile: (fullName: string) =>
    request<User>('/api/auth/profile', {
      method: 'PUT',
      body: JSON.stringify({ fullName }),
    }),

  changePassword: (currentPassword: string, newPassword: string) =>
    request<void>('/api/auth/change-password', {
      method: 'POST',
      body: JSON.stringify({ currentPassword, newPassword }),
    }),

  logout() {
    localStorage.removeItem(TOKEN_KEY)
  },

  hasToken: () => Boolean(localStorage.getItem(TOKEN_KEY)),
}
