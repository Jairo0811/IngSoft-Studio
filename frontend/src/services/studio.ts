import { API_URL, TOKEN_KEY, apiRequest } from './api'

export type PortfolioDashboard = {
  totalProjects: number
  draftProjects: number
  activeProjects: number
  completedProjects: number
  archivedProjects: number
  totalRequirements: number
  totalTests: number
  passedTests: number
  openDefects: number
  openRisks: number
  testPassRatePercent: number
  requirementCoveragePercent: number
}

export type ProjectInsight = { projectId: string; projectName: string; status: string; requirements: number; tests: number; passedTests: number; openDefects: number; openRisks: number; coveragePercent: number; passRatePercent: number }
export type PortfolioTrend = { label: string; requirements: number; tests: number; defects: number; risks: number }
export type SimulationOption = { id: string; label: string; score: number; feedback: string }
export type SimulationScenario = { id: string; title: string; context: string; question: string; options: SimulationOption[] }
export type SimulationResult = { scenarioId: string; optionId: string; score: number; feedback: string; level: string }
export type SimulationAttempt = { id: string; scenarioId: string; optionId: string; score: number; level: string; createdAtUtc: string }
export type SimulationSummary = { attempts: number; averageScore: number; bestScore: number; recentAttempts: SimulationAttempt[] }
export type LearningTopic = { id: string; title: string; category: string; summary: string; keyPoints: string[] }

async function downloadReport(path: string, fallbackName: string) {
  const token = localStorage.getItem(TOKEN_KEY)
  const response = await fetch(`${API_URL}${path}`, { headers: token ? { Authorization: `Bearer ${token}` } : {} })
  if (!response.ok) throw new Error('No fue posible generar el reporte.')
  const blob = await response.blob()
  const disposition = response.headers.get('content-disposition') ?? ''
  const match = disposition.match(/filename\*?=(?:UTF-8''|")?([^";]+)/i)
  const fileName = match ? decodeURIComponent(match[1].replace(/"/g, '')) : fallbackName
  const url = URL.createObjectURL(blob)
  const anchor = document.createElement('a')
  anchor.href = url
  anchor.download = fileName
  anchor.click()
  URL.revokeObjectURL(url)
}

export const studioService = {
  dashboard: () => apiRequest<PortfolioDashboard>('/api/v1/studio/dashboard'),
  projectInsights: () => apiRequest<ProjectInsight[]>('/api/v1/studio/projects'),
  trends: () => apiRequest<PortfolioTrend[]>('/api/v1/studio/trends'),
  scenarios: () => apiRequest<SimulationScenario[]>('/api/v1/studio/simulation/scenarios'),
  evaluate: (scenarioId: string, optionId: string) => apiRequest<SimulationResult>('/api/v1/studio/simulation/evaluate', { method: 'POST', body: JSON.stringify({ scenarioId, optionId }) }),
  simulationSummary: () => apiRequest<SimulationSummary>('/api/v1/studio/simulation/summary'),
  learning: () => apiRequest<LearningTopic[]>('/api/v1/studio/learning'),
  downloadPdf: () => downloadReport('/api/v1/studio/reports/pdf', 'ingsoft-studio-report.pdf'),
  downloadExcel: () => downloadReport('/api/v1/studio/reports/excel', 'ingsoft-studio-report.xlsx'),
}
