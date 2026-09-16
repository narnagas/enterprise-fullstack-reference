export interface ProjectSummary {
  id: number;
  projectNumber: string;
  name: string;
  customerName: string;
  status: string;
  createdDate: string;
  dueDate: string | null;
  isActive: boolean;
}

export interface ProjectDetail extends ProjectSummary {}

export interface UpdateProjectRequest {
  name: string;
  customerName: string;
  status: string;
  dueDate: string | null;
  isActive: boolean;
}

export interface ProjectSearchRequest {
  pageNumber: number;
  pageSize: number;
  search: string | null;
  customer: string | null;
  status: string | null;
  isActive: boolean | null;
  sortField: string;
  sortDirection: 'asc' | 'desc';
}
