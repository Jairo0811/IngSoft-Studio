import { apiRequest } from './api'

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

export type SimulationOption = { id: string; label: string; score: number; feedback: string }
export type SimulationScenario = { id: string; title: string; context: string; question: string; options: SimulationOption[] }
export type SimulationResult = { scenarioId: string; optionId: string; score: number; feedback: string; level: string }
export type LearningTopic = { id: string; title: string; category: string; summary: string; keyPoints: string[] }

export const studioService = {
  dashboard: () => apiRequest<PortfolioDashboard>('/api/v1/studio/dashboard'),
  scenarios: () => apiRequest<SimulationScenario[]>('/api/v1/studio/simulation/scenarios'),
  evaluate: (scenarioId: string, optionId: string) => apiRequest<SimulationResult>('/api/v1/studio/simulation/evaluate', { method: 'POST', body: JSON.stringify({ scenarioId, optionId }) }),
  learning: () => apiRequest<LearningTopic[]>('/api/v1/studio/learning'),
}
