import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:7000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export interface WarehouseDto {
  id: number;
  warehouseCode: string;
  companyCode: string;
  companyDesc: string;
  countryCode: string;
  locationCode: string;
  abbreviation: string;
  division: string;
  divisionDesc: string;
  defaultCrewOpsEmpno: string;
  defaultCrewOpsName: string;
  defaultCrewOpsEmail: string;
  touringRevenueGroup: string;
  isActive: boolean;
}

export interface WarehouseGroupDto {
  id: number;
  groupName: string;
  description: string;
  isActive: boolean;
  warehouses: WarehouseDto[];
}

export interface ProjectDto {
  id: number;
  projectNumber: string;
  projectName: string;
  description: string;
  startDate: string;
  endDate: string;
  status: string;
  entityNo: string;
  entityDesc: string;
  isActive: boolean;
}

export interface ScheduleItemDto {
  id: number;
  projectId: number;
  projectNumber: string;
  projectName: string;
  warehouseId: number;
  warehouseCode: string;
  warehouseName: string;
  partId?: number;
  partNumber?: string;
  partDescription?: string;
  scheduledDate: string;
  quantity: number;
  status: string;
  isLate: boolean;
  isProposal: boolean;
  notes: string;
}

export interface ScheduleBoardDto {
  scheduleItems: ScheduleItemDto[];
  warehouses: WarehouseDto[];
  projects: ProjectDto[];
  startDate: string;
  endDate: string;
}

export const apiService = {
  async getScheduleBoard(params: {
    startDate?: string;
    endDate?: string;
    warehouses?: string;
    includeProposals?: boolean;
    showDetail?: boolean;
    showFuture?: boolean;
  }): Promise<ScheduleBoardDto> {
    const response = await api.get<ScheduleBoardDto>('/ScheduleBoard', { params });
    return response.data;
  },

  async getWarehouses(includeInactive = false): Promise<WarehouseDto[]> {
    const response = await api.get<WarehouseDto[]>('/Warehouses', { 
      params: { includeInactive } 
    });
    return response.data;
  },

  async getWarehouseGroups(includeInactive = false): Promise<WarehouseGroupDto[]> {
    const response = await api.get<WarehouseGroupDto[]>('/Warehouses/groups', { 
      params: { includeInactive } 
    });
    return response.data;
  },
};

export default api;