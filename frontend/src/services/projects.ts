import { apiRequest } from './api'

export type Project = {
  id: string
  name: string
  description?: string | null
  status: 'Draft' | 'Active' | 'Completed' | 'Archived'
  createdAtUtc: string
  updatedAtUtc?: string | null
}

export type Requirement = {
  id: string
  projectId: string
  title: string
  description: string
  type: 'Functional' | 'NonFunctional' | 'UserStory' | 'UseCase'
  priority: 'Must' | 'Should' | 'Could' | 'Wont'
  status: 'Proposed' | 'Approved' | 'InProgress' | 'Implemented' | 'Rejected'
  acceptanceCriteria?: string | null
  source?: string | null
  createdAtUtc: string
  updatedAtUtc?: string | null
}

export type RequirementInput = Pick<Requirement, 'title' | 'description' | 'type' | 'priority'> & {
  acceptanceCriteria?: string
  source?: string
}

export const projectsService = {
  list: () => apiRequest<Project[]>('/api/v1/projects'),
  create: (name: string, description?: string) => apiRequest<Project>('/api/v1/projects', { method: 'POST', body: JSON.stringify({ name, description }) }),
  update: (id: string, name: string, description?: string) => apiRequest<Project>(`/api/v1/projects/${id}`, { method: 'PUT', body: JSON.stringify({ name, description }) }),
  changeStatus: (id: string, status: number) => apiRequest<Project>(`/api/v1/projects/${id}/status`, { method: 'PATCH', body: JSON.stringify({ status }) }),
  remove: (id: string) => apiRequest<void>(`/api/v1/projects/${id}`, { method: 'DELETE' }),
  requirements: (projectId: string) => apiRequest<Requirement[]>(`/api/v1/projects/${projectId}/requirements`),
  createRequirement: (projectId: string, input: RequirementInput) => apiRequest<Requirement>(`/api/v1/projects/${projectId}/requirements`, { method: 'POST', body: JSON.stringify(input) }),
  updateRequirement: (projectId: string, id: string, input: RequirementInput) => apiRequest<Requirement>(`/api/v1/projects/${projectId}/requirements/${id}`, { method: 'PUT', body: JSON.stringify(input) }),
  changeRequirementStatus: (projectId: string, id: string, status: number) => apiRequest<Requirement>(`/api/v1/projects/${projectId}/requirements/${id}/status`, { method: 'PATCH', body: JSON.stringify({ status }) }),
  removeRequirement: (projectId: string, id: string) => apiRequest<void>(`/api/v1/projects/${projectId}/requirements/${id}`, { method: 'DELETE' }),
}
