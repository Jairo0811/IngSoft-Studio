import { apiRequest, TOKEN_KEY } from './api'

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

export type ForgotPasswordResponse = {
  resetToken?: string
}

export const authService = {
  async login(email: string, password: string) {
    const result = await apiRequest<AuthResponse>('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    })
    localStorage.setItem(TOKEN_KEY, result.accessToken)
    return result
  },

  async register(fullName: string, email: string, password: string) {
    const result = await apiRequest<AuthResponse>('/api/auth/register', {
      method: 'POST',
      body: JSON.stringify({ fullName, email, password }),
    })
    localStorage.setItem(TOKEN_KEY, result.accessToken)
    return result
  },

  forgotPassword: (email: string) => apiRequest<ForgotPasswordResponse>('/api/auth/forgot-password', { method: 'POST', body: JSON.stringify({ email }) }),
  resetPassword: (email: string, token: string, newPassword: string) => apiRequest<void>('/api/auth/reset-password', { method: 'POST', body: JSON.stringify({ email, token, newPassword }) }),
  me: () => apiRequest<User>('/api/auth/me'),
  updateProfile: (fullName: string) => apiRequest<User>('/api/auth/profile', { method: 'PUT', body: JSON.stringify({ fullName }) }),
  changePassword: (currentPassword: string, newPassword: string) => apiRequest<void>('/api/auth/change-password', { method: 'POST', body: JSON.stringify({ currentPassword, newPassword }) }),
  logout() { localStorage.removeItem(TOKEN_KEY) },
  hasToken: () => Boolean(localStorage.getItem(TOKEN_KEY)),
}
