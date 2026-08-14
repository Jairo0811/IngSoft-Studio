import { apiRequest } from './api'

export type QualityMetrics = {
  totalRequirements: number
  coveredRequirements: number
  requirementCoveragePercent: number
  totalTests: number
  passedTests: number
  failedTests: number
  testPassRatePercent: number
  openDefects: number
  criticalDefects: number
  openRisks: number
  highRisks: number
}

export type TraceabilityRow = {
  requirementId: string
  requirementTitle: string
  requirementStatus: string
  testCases: number
  passedTests: number
  failedTests: number
  openDefects: number
  covered: boolean
}

export type Risk = { id: string; title: string; description: string; probability: string; impact: string; status: string; score: number; mitigation: string }
export type TestCase = { id: string; requirementId?: string | null; title: string; expectedResult: string; actualResult?: string | null; status: string }
export type Defect = { id: string; requirementId?: string | null; testCaseId?: string | null; title: string; severity: string; priority: string; status: string }
export type QualityDashboard = { metrics: QualityMetrics; traceability: TraceabilityRow[]; risks: Risk[]; testCases: TestCase[]; defects: Defect[] }

export const qualityService = {
  dashboard: (projectId: string) => apiRequest<QualityDashboard>(`/api/v1/projects/${projectId}/quality`),
  createRisk: (projectId: string, input: object) => apiRequest<Risk>(`/api/v1/projects/${projectId}/quality/risks`, { method: 'POST', body: JSON.stringify(input) }),
  createTest: (projectId: string, input: object) => apiRequest<TestCase>(`/api/v1/projects/${projectId}/quality/tests`, { method: 'POST', body: JSON.stringify(input) }),
  executeTest: (projectId: string, id: string, status: number, actualResult: string) => apiRequest<TestCase>(`/api/v1/projects/${projectId}/quality/tests/${id}/execute`, { method: 'PATCH', body: JSON.stringify({ status, actualResult }) }),
  createDefect: (projectId: string, input: object) => apiRequest<Defect>(`/api/v1/projects/${projectId}/quality/defects`, { method: 'POST', body: JSON.stringify(input) }),
}
