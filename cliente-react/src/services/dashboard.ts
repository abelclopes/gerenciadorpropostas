import type { DashboardKpi } from '../types'
import { request } from './http'

export const dashboardApi = {
  getKpis(token: string) {
    return request<DashboardKpi>('/api/dashboard/kpis', { token })
  },
}
