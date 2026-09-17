export interface CustomerSummary {
  id: number;
  customerNumber: string;
  companyName: string;
  contactName: string;
  email: string;
  phone: string;
  city: string;
  state: string;
  createdDate: string;
  isActive: boolean;
}

export interface CustomerSearchRequest {
  pageNumber: number;
  pageSize: number;
  search: string | null;
  state: string | null;
  isActive: boolean | null;
  sortField: string;
  sortDirection: 'asc' | 'desc';
}
